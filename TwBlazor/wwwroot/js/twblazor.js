// Generic picker (used by datepicker, timepicker, etc.)
globalThis.twPicker = {
    registerOutsideClick: function (root, dotnetRef) {
        if (!root) return;
        const handler = function (e) {
            try {
                if (!root.contains(e.target)) {
                    dotnetRef.invokeMethodAsync('Close');
                }
            } catch (err) {
                console.error('twPicker handler error', err);
            }
        };
        root.__twPickerHandler = handler;
        document.addEventListener('pointerdown', handler);
    },

    unregisterOutsideClick: function (root) {
        if (!root) return;
        const handler = root.__twPickerHandler;
        if (handler) {
            document.removeEventListener('pointerdown', handler);
            try { delete root.__twPickerHandler; } catch { root.__twPickerHandler = undefined; }
        }
    },

    // Flips a just-opened popover panel (date/color picker dialog, etc.) away from whichever
    // viewport edge it would otherwise overflow, instead of letting it clip off-screen. Panels are
    // positioned by their own "top-full left-0"-style classes by default; this only overrides that
    // via inline style, and only on the axis that actually overflows, so panels that already fit
    // are left completely alone. Call again (it resets first) whenever the panel reopens, since the
    // available space may have changed (page scroll, trigger moved, viewport resized).
    positionPanel: function (panel) {
        if (!panel) return;

        panel.style.left = '';
        panel.style.right = '';
        panel.style.top = '';
        panel.style.bottom = '';
        panel.style.marginTop = '';
        panel.style.marginBottom = '';

        const rect = panel.getBoundingClientRect();
        const viewportWidth = document.documentElement.clientWidth;
        const viewportHeight = document.documentElement.clientHeight;

        if (rect.right > viewportWidth) {
            panel.style.left = 'auto';
            panel.style.right = '0';
        }

        // Only flip to open upward if doing so would actually fit better - i.e. there's more room
        // above the trigger than below it - otherwise flipping would just clip the opposite edge.
        if (rect.bottom > viewportHeight && rect.top > viewportHeight - rect.bottom) {
            panel.style.top = 'auto';
            panel.style.bottom = '100%';
            panel.style.marginTop = '0';
            panel.style.marginBottom = '0.5rem';
        }
    }
};

// code block
globalThis.twCodeBlock = {
    highlightElement: function (el) {
        hljs.highlightElement(el);
    }
};

// Dialog accessibility helpers: initial focus, a Tab focus trap scoped to the dialog surface,
// background inert-ing while a dialog is open, and restoring focus to the triggering element on close.
globalThis.twDialog = {
    _focusMap: new Map(),

    _focusableSelector: 'a[href], area[href], input:not([disabled]):not([type="hidden"]), ' +
        'select:not([disabled]), textarea:not([disabled]), button:not([disabled]), ' +
        'iframe, object, embed, [contenteditable="true"], [tabindex]:not([tabindex="-1"])',

    getFocusableElements: function (container) {
        if (!container) return [];
        try {
            return Array.from(container.querySelectorAll(globalThis.twDialog._focusableSelector))
                .filter(function (el) {
                    return !el.hasAttribute('inert') && el.getClientRects().length > 0;
                });
        } catch (err) {
            console.error('twDialog.getFocusableElements error', err);
            return [];
        }
    },

    // Focuses the first focusable element within the dialog surface, falling back to the surface
    // itself (which carries tabindex="-1" so it can receive programmatic focus).
    focusSurface: function (surface) {
        if (!surface) return;
        var focusable = globalThis.twDialog.getFocusableElements(surface);
        if (focusable.length > 0) {
            focusable[0].focus();
        } else if (typeof surface.focus === 'function') {
            surface.focus();
        }
    },

    // Traps Tab/Shift+Tab within the dialog surface so focus can't leave it while open.
    trapFocus: function (surface) {
        if (!surface || surface.__twDialogTrapHandler) return;
        var handler = function (e) {
            if (e.key !== 'Tab') return;

            var focusable = globalThis.twDialog.getFocusableElements(surface);
            if (focusable.length === 0) {
                e.preventDefault();
                if (typeof surface.focus === 'function') surface.focus();
                return;
            }

            var first = focusable[0];
            var last = focusable.at(-1);
            var active = document.activeElement;

            if (e.shiftKey) {
                if (active === first || !surface.contains(active)) {
                    e.preventDefault();
                    last.focus();
                }
            } else if (active === last || !surface.contains(active)) {
                e.preventDefault();
                first.focus();
            }
        };
        surface.__twDialogTrapHandler = handler;
        surface.addEventListener('keydown', handler);
    },

    releaseFocusTrap: function (surface) {
        if (!surface?.__twDialogTrapHandler) return;
        surface.removeEventListener('keydown', surface.__twDialogTrapHandler);
        delete surface.__twDialogTrapHandler;
    },

    // Marks everything outside `exceptEl` inert, so background content can't be reached by
    // keyboard, mouse, or a screen reader's browse mode while a dialog is open. Walks upward from
    // exceptEl to <body>, inert-ing siblings at every level (not just exceptEl's own siblings) -
    // this matters because most app hosts (e.g. the WASM/Server templates' single <div id="app">
    // root) wrap the entire app in one element, so only checking direct children of <body> would
    // find nothing to inert (the one body child always contains exceptEl). Tags what it touched so
    // clearBackgroundInert can undo precisely that.
    setBackgroundInert: function (exceptEl) {
        if (!exceptEl || !document.body) return;
        var current = exceptEl;
        while (current && current !== document.body && current.parentElement) {
            var parent = current.parentElement;
            Array.from(parent.children).forEach(function (sibling) {
                if (sibling === current) return;
                if (sibling.hasAttribute('inert')) return;
                sibling.setAttribute('inert', '');
                sibling.dataset.twDialogInert = 'true';
            });
            current = parent;
        }
    },

    clearBackgroundInert: function () {
        if (!document.body) return;
        document.body.querySelectorAll('[data-tw-dialog-inert="true"]').forEach(function (el) {
            el.removeAttribute('inert');
            delete el.dataset.twDialogInert;
        });
    },

    // Records the currently-focused element under an opaque token so it can be refocused later
    // (Blazor/.NET code can't hold a raw DOM element reference).
    captureFocus: function () {
        var active = document.activeElement;
        if (!active || active === document.body) return null;

        var randomPart = crypto.getRandomValues(new Uint32Array(2)).join('');
        var token = 'tw-focus-' + Date.now().toString(36) + '-' + randomPart;
        active.dataset.twFocusToken = token;
        globalThis.twDialog._focusMap.set(token, active);
        return token;
    },

    restoreFocus: function (token) {
        if (!token) return;
        var el = globalThis.twDialog._focusMap.get(token);
        if (!el || !document.body.contains(el)) {
            el = document.querySelector('[data-tw-focus-token="' + token + '"]');
        }

        globalThis.twDialog._focusMap.delete(token);

        if (el) {
            delete el.dataset.twFocusToken;
            if (typeof el.focus === 'function') el.focus();
        }
    }
};

// Custom role="slider" elements (e.g. TwColorPicker's saturation/lightness square, hue and alpha
// strips) need Arrow/Home/End to change their value without also triggering the browser's native
// scroll behavior for those keys. A blanket @onkeydown:preventDefault in Razor would block every
// key on the element - including Tab - trapping keyboard focus inside the control. This attaches a
// plain DOM listener that only calls preventDefault for the specific keys the slider handles,
// leaving Tab (and everything else) completely untouched; it doesn't stop propagation, so Blazor's
// own delegated keydown handling (and the C# handler bound via @onkeydown) still runs normally.
globalThis.twSlider = {
    _scrollKeys: ['ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight', 'Home', 'End'],

    _scrollKeyHandler: function (e) {
        if (globalThis.twSlider._scrollKeys.includes(e.key)) {
            e.preventDefault();
        }
    },

    preventScrollKeys: function (el) {
        if (!el || el.__twSliderScrollHandler) return;
        el.__twSliderScrollHandler = globalThis.twSlider._scrollKeyHandler;
        el.addEventListener('keydown', globalThis.twSlider._scrollKeyHandler);
    }
};

// Tabs: prevents the browser's native scroll behavior for the specific WAI-ARIA APG tablist
// navigation keys (arrow keys, Home, End) without ever calling preventDefault for any other key -
// crucially not for Tab, whose default action (moving focus out of the tablist) must be left alone
// so keyboard users are never trapped inside the tablist. This runs as its own native listener
// alongside Blazor's own keydown binding (which does the actual tab-switching logic in C#); this
// listener's only job is the selective preventDefault that a static `@onkeydown:preventDefault="true"`
// can't safely express, since that directive would apply unconditionally to every key, Tab included.
globalThis.twTabs = {
    _navigationKeys: new Set(['ArrowRight', 'ArrowLeft', 'ArrowUp', 'ArrowDown', 'Home', 'End']),

    _tabsKeydownHandler: function (e) {
        if (globalThis.twTabs._navigationKeys.has(e.key)) {
            e.preventDefault();
        }
    },

    registerKeydownGuard: function (tablist) {
        if (!tablist || tablist.__twTabsKeydownHandler) return;
        tablist.__twTabsKeydownHandler = globalThis.twTabs._tabsKeydownHandler;
        tablist.addEventListener('keydown', globalThis.twTabs._tabsKeydownHandler);
    },

    unregisterKeydownGuard: function (tablist) {
        if (!tablist?.__twTabsKeydownHandler) return;
        tablist.removeEventListener('keydown', tablist.__twTabsKeydownHandler);
        delete tablist.__twTabsKeydownHandler;
    }
};

// Device detection (used by pickers to defer to the platform's native input UI on mobile).
globalThis.twDevice = {
    getPlatform: function () {
        const nav = typeof navigator !== 'undefined' ? navigator : {};
        const userAgent = nav.userAgent || '';
        const platform = nav.platform || '';
        const maxTouchPoints = nav.maxTouchPoints || 0;

        // iPadOS 13+ reports a desktop Mac user agent, so touch-capable "MacIntel" is treated as iOS.
        const isIPadOS = platform === 'MacIntel' && maxTouchPoints > 1;

        if (isIPadOS || /iPhone|iPad|iPod/.test(userAgent)) {
            return 'ios';
        }

        if (/Android/.test(userAgent)) {
            return 'android';
        }

        return 'other';
    },

    prefersNativePicker: function () {
        const platform = globalThis.twDevice.getPlatform();
        return platform === 'ios' || platform === 'android';
    }
};

// Sidebar: viewport check used to decide whether the mobile overlay drawer's Tab focus trap and
// background inert-ing should be armed. Matches Tailwind's default "lg" breakpoint (1024px) - below
// it the sidebar behaves as a modal drawer over the page; at or above it, it's a persistent panel
// beside fully-usable page content, so trapping focus there would be wrong.
globalThis.twSidebar = {
    isMobileViewport: function () {
        try {
            return !window.matchMedia('(min-width: 1024px)').matches;
        } catch (err) {
            console.error('twSidebar.isMobileViewport error', err);
            return true;
        }
    }
};

// Color picker: touch events (Blazor's TouchEventArgs only exposes each touch point's viewport-
// relative clientX/clientY, unlike MouseEventArgs which also gives an element-relative offsetX/
// offsetY) so the saturation/lightness square and hue/alpha strips can translate a touch point into
// a position relative to the slider element itself, the same way their existing mouse handlers
// already do via e.OffsetX/e.OffsetY.
globalThis.twColorPicker = {
    relativePosition: function (el, clientX, clientY) {
        if (!el) return [0, 0];
        var rect = el.getBoundingClientRect();
        return [clientX - rect.left, clientY - rect.top];
    },
    // Measures an element's actual rendered size, so the saturation/lightness square and hue/alpha
    // strips can convert a drag position into a percentage using the size they're really laid out
    // at instead of a hardcoded guess - keeping drag math correct if the picker dialog is resized
    // (responsive breakpoints, browser zoom, a consumer overriding the dialog's width class).
    getSize: function (el) {
        if (!el) return [0, 0];
        var rect = el.getBoundingClientRect();
        return [rect.width, rect.height];
    }
};