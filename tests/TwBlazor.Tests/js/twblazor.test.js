// @vitest-environment jsdom

import { beforeEach, afterEach, describe, expect, test, vi } from 'vitest';

// Load the script once — it assigns window.twPicker and window.twCodeBlock on evaluation.
// hljs is accessed at call-time inside twCodeBlock.highlightElement, so it does not need to
// be present when the module loads.
await import('../../../src/TwBlazor/wwwroot/js/twblazor.js');

describe('twPicker', () => {
    afterEach(() => {
        vi.restoreAllMocks();
    });

    describe('registerOutsideClick', () => {
        test('registers a pointerdown listener on document', () => {
            const root = document.createElement('div');
            const dotnetRef = { invokeMethodAsync: vi.fn() };

            const addSpy = vi.spyOn(document, 'addEventListener');

            window.twPicker.registerOutsideClick(root, dotnetRef);

            expect(addSpy).toHaveBeenCalledWith('pointerdown', expect.any(Function));
            expect(root.__twPickerHandler).toBeDefined();

            window.twPicker.unregisterOutsideClick(root);
        });

        test('does nothing when root is null', () => {
            const addSpy = vi.spyOn(document, 'addEventListener');

            window.twPicker.registerOutsideClick(null, {});

            expect(addSpy).not.toHaveBeenCalled();
        });

        test('calls Close on dotnetRef when click is outside root', () => {
            const root = document.createElement('div');
            document.body.appendChild(root);
            const outside = document.createElement('span');
            document.body.appendChild(outside);

            const dotnetRef = { invokeMethodAsync: vi.fn() };

            window.twPicker.registerOutsideClick(root, dotnetRef);
            root.__twPickerHandler({ target: outside });

            expect(dotnetRef.invokeMethodAsync).toHaveBeenCalledWith('Close');

            window.twPicker.unregisterOutsideClick(root);
            document.body.removeChild(root);
            document.body.removeChild(outside);
        });

        test('does not call Close when click is inside root', () => {
            const root = document.createElement('div');
            const inner = document.createElement('span');
            root.appendChild(inner);
            document.body.appendChild(root);

            const dotnetRef = { invokeMethodAsync: vi.fn() };

            window.twPicker.registerOutsideClick(root, dotnetRef);
            root.__twPickerHandler({ target: inner });

            expect(dotnetRef.invokeMethodAsync).not.toHaveBeenCalled();

            window.twPicker.unregisterOutsideClick(root);
            document.body.removeChild(root);
        });

        test('swallows errors thrown by invokeMethodAsync', () => {
            const root = document.createElement('div');
            document.body.appendChild(root);
            const outside = document.createElement('span');
            document.body.appendChild(outside);

            const error = new Error('interop error');
            const dotnetRef = {
                invokeMethodAsync: vi.fn().mockImplementation(() => { throw error; }),
            };
            const consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => {});

            window.twPicker.registerOutsideClick(root, dotnetRef);

            expect(() => root.__twPickerHandler({ target: outside })).not.toThrow();
            expect(consoleErrorSpy).toHaveBeenCalledWith('twPicker handler error', error);

            window.twPicker.unregisterOutsideClick(root);
            document.body.removeChild(root);
            document.body.removeChild(outside);
        });
    });

    describe('positionPanel', () => {
        function mockPanel(rect) {
            const panel = document.createElement('div');
            panel.getBoundingClientRect = () => rect;
            return panel;
        }

        function setViewport(width, height) {
            Object.defineProperty(document.documentElement, 'clientWidth', { value: width, configurable: true });
            Object.defineProperty(document.documentElement, 'clientHeight', { value: height, configurable: true });
        }

        afterEach(() => {
            // Restore real (jsdom-default) viewport getters so other tests aren't affected.
            delete document.documentElement.clientWidth;
            delete document.documentElement.clientHeight;
        });

        test('does nothing when panel is null', () => {
            expect(() => window.twPicker.positionPanel(null)).not.toThrow();
        });

        test('resets any previously-applied inline positioning styles before recomputing', () => {
            setViewport(1000, 800);
            const panel = mockPanel({ left: 10, right: 200, top: 10, bottom: 100 });
            panel.style.left = 'auto';
            panel.style.right = '0';
            panel.style.top = 'auto';
            panel.style.bottom = '100%';
            panel.style.marginTop = '0';
            panel.style.marginBottom = '0.5rem';

            window.twPicker.positionPanel(panel);

            // Panel fits within the viewport, so all overrides should be cleared, not reapplied.
            expect(panel.style.left).toBe('');
            expect(panel.style.right).toBe('');
            expect(panel.style.top).toBe('');
            expect(panel.style.bottom).toBe('');
            expect(panel.style.marginTop).toBe('');
            expect(panel.style.marginBottom).toBe('');
        });

        test('leaves positioning alone when the panel fits entirely within the viewport', () => {
            setViewport(1000, 800);
            const panel = mockPanel({ left: 10, right: 200, top: 10, bottom: 100 });

            window.twPicker.positionPanel(panel);

            expect(panel.style.left).toBe('');
            expect(panel.style.top).toBe('');
        });

        test('flips to the left when the panel overflows the right edge', () => {
            setViewport(1000, 800);
            const panel = mockPanel({ left: 900, right: 1100, top: 10, bottom: 100 });

            window.twPicker.positionPanel(panel);

            expect(panel.style.left).toBe('auto');
            expect(panel.style.right).toBe('0px'); // jsdom normalizes the '0' length to '0px'
        });

        test('flips upward when overflowing the bottom edge and there is more room above', () => {
            setViewport(1000, 800);
            const panel = mockPanel({ left: 10, right: 200, top: 700, bottom: 900 });

            window.twPicker.positionPanel(panel);

            expect(panel.style.top).toBe('auto');
            expect(panel.style.bottom).toBe('100%');
            expect(panel.style.marginTop).toBe('0px'); // jsdom normalizes the '0' length to '0px'
            expect(panel.style.marginBottom).toBe('0.5rem');
        });

        test('does not flip upward when overflowing the bottom edge but there is not more room above', () => {
            setViewport(1000, 800);
            // A panel taller than the viewport, extending well above the top edge too - flipping up
            // wouldn't help since there's even less room above than below.
            const panel = mockPanel({ left: 10, right: 200, top: -150, bottom: 900 });

            window.twPicker.positionPanel(panel);

            expect(panel.style.top).toBe('');
            expect(panel.style.bottom).toBe('');
        });
    });

    describe('unregisterOutsideClick', () => {
        test('removes the registered pointerdown listener', () => {
            const root = document.createElement('div');
            const dotnetRef = { invokeMethodAsync: vi.fn() };

            window.twPicker.registerOutsideClick(root, dotnetRef);

            const removeSpy = vi.spyOn(document, 'removeEventListener');
            const handler = root.__twPickerHandler;

            window.twPicker.unregisterOutsideClick(root);

            expect(removeSpy).toHaveBeenCalledWith('pointerdown', handler);
        });

        test('does nothing when root is null', () => {
            const removeSpy = vi.spyOn(document, 'removeEventListener');

            window.twPicker.unregisterOutsideClick(null);

            expect(removeSpy).not.toHaveBeenCalled();
        });

        test('does nothing when root has no registered handler', () => {
            const root = document.createElement('div');
            const removeSpy = vi.spyOn(document, 'removeEventListener');

            window.twPicker.unregisterOutsideClick(root);

            expect(removeSpy).not.toHaveBeenCalled();
        });

        test('after unregister, handler is cleared from the element', () => {
            const root = document.createElement('div');
            document.body.appendChild(root);
            const outside = document.createElement('span');
            document.body.appendChild(outside);

            const dotnetRef = { invokeMethodAsync: vi.fn() };

            window.twPicker.registerOutsideClick(root, dotnetRef);
            window.twPicker.unregisterOutsideClick(root);

            expect(root.__twPickerHandler).toBeUndefined();

            document.body.removeChild(root);
            document.body.removeChild(outside);
        });
    });
});

describe('twDevice', () => {
    const originalUserAgent = navigator.userAgent;
    const originalPlatform = navigator.platform;
    const originalMaxTouchPoints = navigator.maxTouchPoints;

    function setNavigator({ userAgent, platform, maxTouchPoints }) {
        Object.defineProperty(window.navigator, 'userAgent', { value: userAgent, configurable: true });
        Object.defineProperty(window.navigator, 'platform', { value: platform, configurable: true });
        Object.defineProperty(window.navigator, 'maxTouchPoints', { value: maxTouchPoints, configurable: true });
    }

    afterEach(() => {
        setNavigator({ userAgent: originalUserAgent, platform: originalPlatform, maxTouchPoints: originalMaxTouchPoints });
    });

    describe('getPlatform', () => {
        test('returns "ios" for an iPhone user agent', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15',
                platform: 'iPhone',
                maxTouchPoints: 5,
            });

            expect(window.twDevice.getPlatform()).toBe('ios');
        });

        test('returns "ios" for a classic iPad user agent', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (iPad; CPU OS 17_0 like Mac OS X) AppleWebKit/605.1.15',
                platform: 'iPad',
                maxTouchPoints: 5,
            });

            expect(window.twDevice.getPlatform()).toBe('ios');
        });

        test('returns "ios" for a modern iPad reporting a MacIntel desktop user agent with touch support', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15',
                platform: 'MacIntel',
                maxTouchPoints: 5,
            });

            expect(window.twDevice.getPlatform()).toBe('ios');
        });

        test('returns "other" for a real (non-touch) Mac desktop', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15',
                platform: 'MacIntel',
                maxTouchPoints: 0,
            });

            expect(window.twDevice.getPlatform()).toBe('other');
        });

        test('returns "android" for an Android user agent', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (Linux; Android 14; Pixel 8) AppleWebKit/537.36',
                platform: 'Linux armv8l',
                maxTouchPoints: 5,
            });

            expect(window.twDevice.getPlatform()).toBe('android');
        });

        test('returns "other" for a desktop Windows user agent', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36',
                platform: 'Win32',
                maxTouchPoints: 0,
            });

            expect(window.twDevice.getPlatform()).toBe('other');
        });

        test('returns "other" when userAgent and platform are unavailable', () => {
            setNavigator({ userAgent: '', platform: '', maxTouchPoints: 0 });

            expect(window.twDevice.getPlatform()).toBe('other');
        });

        test('returns "other" when navigator itself is unavailable', () => {
            vi.stubGlobal('navigator', undefined);

            expect(window.twDevice.getPlatform()).toBe('other');

            vi.unstubAllGlobals();
        });
    });

    describe('prefersNativePicker', () => {
        test('returns true on iOS', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X) AppleWebKit/605.1.15',
                platform: 'iPhone',
                maxTouchPoints: 5,
            });

            expect(window.twDevice.prefersNativePicker()).toBe(true);
        });

        test('returns true on Android', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (Linux; Android 14; Pixel 8) AppleWebKit/537.36',
                platform: 'Linux armv8l',
                maxTouchPoints: 5,
            });

            expect(window.twDevice.prefersNativePicker()).toBe(true);
        });

        test('returns false on desktop', () => {
            setNavigator({
                userAgent: 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36',
                platform: 'Win32',
                maxTouchPoints: 0,
            });

            expect(window.twDevice.prefersNativePicker()).toBe(false);
        });
    });
});

describe('twCodeBlock', () => {
    beforeEach(() => {
        window.hljs = { highlightElement: vi.fn() };
    });

    afterEach(() => {
        vi.restoreAllMocks();
        delete window.hljs;
    });

    test('highlightElement delegates to hljs.highlightElement', () => {
        const el = document.createElement('code');

        window.twCodeBlock.highlightElement(el);

        expect(window.hljs.highlightElement).toHaveBeenCalledWith(el);
    });
});

// jsdom doesn't run layout, so a real element's getClientRects() is always empty. twDialog's
// focusable-elements filter treats that the same as "not visible", so tests that need an element
// to be treated as focusable stub getClientRects() to report a non-empty rect list.
function makeFocusable(tag = 'button') {
    const el = document.createElement(tag);
    el.getClientRects = () => [{}];
    return el;
}

describe('twDialog', () => {
    afterEach(() => {
        document.body.innerHTML = '';
        window.twDialog._focusMap.clear();
        vi.restoreAllMocks();
    });

    describe('getFocusableElements', () => {
        test('returns empty array when container is null', () => {
            expect(window.twDialog.getFocusableElements(null)).toEqual([]);
        });

        test('returns focusable elements within the container', () => {
            const container = document.createElement('div');
            const button = makeFocusable('button');
            container.appendChild(button);
            document.body.appendChild(container);

            const result = window.twDialog.getFocusableElements(container);

            expect(result).toEqual([button]);
        });

        test('excludes elements marked inert', () => {
            const container = document.createElement('div');
            const button = makeFocusable('button');
            button.setAttribute('inert', '');
            container.appendChild(button);
            document.body.appendChild(container);

            expect(window.twDialog.getFocusableElements(container)).toEqual([]);
        });

        test('excludes elements with no client rects (not visible)', () => {
            const container = document.createElement('div');
            const button = document.createElement('button'); // no getClientRects stub
            container.appendChild(button);
            document.body.appendChild(container);

            expect(window.twDialog.getFocusableElements(container)).toEqual([]);
        });

        test('excludes disabled inputs and buttons', () => {
            const container = document.createElement('div');
            const button = makeFocusable('button');
            button.disabled = true;
            container.appendChild(button);
            document.body.appendChild(container);

            expect(window.twDialog.getFocusableElements(container)).toEqual([]);
        });

        test('returns empty array and logs error when querySelectorAll throws', () => {
            const consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
            const container = {
                querySelectorAll: () => { throw new Error('boom'); },
            };

            const result = window.twDialog.getFocusableElements(container);

            expect(result).toEqual([]);
            expect(consoleErrorSpy).toHaveBeenCalledWith('twDialog.getFocusableElements error', expect.any(Error));
        });
    });

    describe('focusSurface', () => {
        test('does nothing when surface is null', () => {
            expect(() => window.twDialog.focusSurface(null)).not.toThrow();
        });

        test('focuses the first focusable descendant when one exists', () => {
            const surface = document.createElement('div');
            const first = makeFocusable('button');
            const second = makeFocusable('button');
            surface.append(first, second);
            document.body.appendChild(surface);

            const focusSpy = vi.spyOn(first, 'focus');

            window.twDialog.focusSurface(surface);

            expect(focusSpy).toHaveBeenCalled();
        });

        test('falls back to focusing the surface itself when no descendant is focusable', () => {
            const surface = makeFocusable('div');
            document.body.appendChild(surface);
            const focusSpy = vi.spyOn(surface, 'focus');

            window.twDialog.focusSurface(surface);

            expect(focusSpy).toHaveBeenCalled();
        });
    });

    describe('trapFocus / releaseFocusTrap', () => {
        test('does nothing when surface is null', () => {
            expect(() => window.twDialog.trapFocus(null)).not.toThrow();
        });

        test('registers a keydown listener exactly once per surface', () => {
            const surface = document.createElement('div');
            const addSpy = vi.spyOn(surface, 'addEventListener');

            window.twDialog.trapFocus(surface);
            window.twDialog.trapFocus(surface); // second call should be a no-op

            expect(addSpy).toHaveBeenCalledTimes(1);
            expect(surface.__twDialogTrapHandler).toBeDefined();
        });

        test('ignores non-Tab keys', () => {
            const surface = document.createElement('div');
            const button = makeFocusable('button');
            surface.appendChild(button);
            document.body.appendChild(surface);
            window.twDialog.trapFocus(surface);

            const event = new window.KeyboardEvent('keydown', { key: 'Escape', bubbles: true });
            const preventSpy = vi.spyOn(event, 'preventDefault');

            surface.dispatchEvent(event);

            expect(preventSpy).not.toHaveBeenCalled();
        });

        test('Tab with no focusable elements prevents default and refocuses the surface', () => {
            const surface = makeFocusable('div');
            document.body.appendChild(surface);
            window.twDialog.trapFocus(surface);
            const focusSpy = vi.spyOn(surface, 'focus');

            const event = new window.KeyboardEvent('keydown', { key: 'Tab', bubbles: true });
            const preventSpy = vi.spyOn(event, 'preventDefault');
            surface.dispatchEvent(event);

            expect(preventSpy).toHaveBeenCalled();
            expect(focusSpy).toHaveBeenCalled();
        });

        test('Tab on the last focusable element wraps to the first', () => {
            const surface = document.createElement('div');
            const first = makeFocusable('button');
            const last = makeFocusable('button');
            surface.append(first, last);
            document.body.appendChild(surface);
            window.twDialog.trapFocus(surface);
            last.focus();

            const event = new window.KeyboardEvent('keydown', { key: 'Tab', bubbles: true });
            const preventSpy = vi.spyOn(event, 'preventDefault');
            const focusSpy = vi.spyOn(first, 'focus');
            last.dispatchEvent(event);

            expect(preventSpy).toHaveBeenCalled();
            expect(focusSpy).toHaveBeenCalled();
        });

        test('Tab while focus is outside the surface wraps to the first', () => {
            const surface = document.createElement('div');
            const first = makeFocusable('button');
            const last = makeFocusable('button');
            surface.append(first, last);
            document.body.appendChild(surface);
            window.twDialog.trapFocus(surface);
            // Active element defaults to document.body, which is not contained by `surface`.

            const event = new window.KeyboardEvent('keydown', { key: 'Tab', bubbles: true });
            const preventSpy = vi.spyOn(event, 'preventDefault');
            const focusSpy = vi.spyOn(first, 'focus');
            surface.dispatchEvent(event);

            expect(preventSpy).toHaveBeenCalled();
            expect(focusSpy).toHaveBeenCalled();
        });

        test('Shift+Tab on the first focusable element wraps to the last', () => {
            const surface = document.createElement('div');
            const first = makeFocusable('button');
            const last = makeFocusable('button');
            surface.append(first, last);
            document.body.appendChild(surface);
            window.twDialog.trapFocus(surface);
            first.focus();

            const event = new window.KeyboardEvent('keydown', { key: 'Tab', shiftKey: true, bubbles: true });
            const preventSpy = vi.spyOn(event, 'preventDefault');
            const focusSpy = vi.spyOn(last, 'focus');
            first.dispatchEvent(event);

            expect(preventSpy).toHaveBeenCalled();
            expect(focusSpy).toHaveBeenCalled();
        });

        test('Tab on a middle focusable element does not prevent default', () => {
            const surface = document.createElement('div');
            const first = makeFocusable('button');
            const middle = makeFocusable('button');
            const last = makeFocusable('button');
            surface.append(first, middle, last);
            document.body.appendChild(surface);
            window.twDialog.trapFocus(surface);
            middle.focus();

            const event = new window.KeyboardEvent('keydown', { key: 'Tab', bubbles: true });
            const preventSpy = vi.spyOn(event, 'preventDefault');
            middle.dispatchEvent(event);

            expect(preventSpy).not.toHaveBeenCalled();
        });

        test('releaseFocusTrap removes the listener', () => {
            const surface = document.createElement('div');
            window.twDialog.trapFocus(surface);
            const removeSpy = vi.spyOn(surface, 'removeEventListener');
            const handler = surface.__twDialogTrapHandler;

            window.twDialog.releaseFocusTrap(surface);

            expect(removeSpy).toHaveBeenCalledWith('keydown', handler);
            expect(surface.__twDialogTrapHandler).toBeUndefined();
        });

        test('releaseFocusTrap does nothing when surface is null', () => {
            expect(() => window.twDialog.releaseFocusTrap(null)).not.toThrow();
        });

        test('releaseFocusTrap does nothing when no trap was registered', () => {
            const surface = document.createElement('div');
            const removeSpy = vi.spyOn(surface, 'removeEventListener');

            window.twDialog.releaseFocusTrap(surface);

            expect(removeSpy).not.toHaveBeenCalled();
        });
    });

    describe('setBackgroundInert / clearBackgroundInert', () => {
        test('does nothing when exceptEl is null', () => {
            expect(() => window.twDialog.setBackgroundInert(null)).not.toThrow();
        });

        test('marks siblings at every level up to body as inert, excluding the element itself', () => {
            document.body.innerHTML = `
                <div id="app">
                    <div id="dialog-root"><div id="surface"></div></div>
                    <div id="sibling-of-root">background</div>
                </div>
                <div id="sibling-of-app">also background</div>
            `;
            const exceptEl = document.getElementById('dialog-root');

            window.twDialog.setBackgroundInert(exceptEl);

            expect(document.getElementById('sibling-of-root').hasAttribute('inert')).toBe(true);
            expect(document.getElementById('sibling-of-app').hasAttribute('inert')).toBe(true);
            expect(exceptEl.hasAttribute('inert')).toBe(false);
        });

        test('does not re-mark an already-inert sibling', () => {
            document.body.innerHTML = `
                <div id="root">
                    <div id="surface"></div>
                    <div id="already-inert" inert></div>
                </div>
            `;
            const alreadyInert = document.getElementById('already-inert');

            window.twDialog.setBackgroundInert(document.getElementById('surface'));

            expect(alreadyInert.hasAttribute('data-tw-dialog-inert')).toBe(false);
        });

        test('clearBackgroundInert removes inert markers it previously set', () => {
            document.body.innerHTML = `
                <div id="root">
                    <div id="surface"></div>
                    <div id="sibling">background</div>
                </div>
            `;
            window.twDialog.setBackgroundInert(document.getElementById('surface'));
            const sibling = document.getElementById('sibling');
            expect(sibling.hasAttribute('inert')).toBe(true);

            window.twDialog.clearBackgroundInert();

            expect(sibling.hasAttribute('inert')).toBe(false);
            expect(sibling.hasAttribute('data-tw-dialog-inert')).toBe(false);
        });
    });

    describe('captureFocus / restoreFocus', () => {
        test('returns null when nothing is focused (active element is body)', () => {
            expect(window.twDialog.captureFocus()).toBeNull();
        });

        test('captures the active element and returns an opaque token', () => {
            const button = document.createElement('button');
            document.body.appendChild(button);
            button.focus();

            const token = window.twDialog.captureFocus();

            expect(token).toMatch(/^tw-focus-/);
            expect(button.getAttribute('data-tw-focus-token')).toBe(token);
        });

        test('restoreFocus does nothing when token is falsy', () => {
            expect(() => window.twDialog.restoreFocus(null)).not.toThrow();
            expect(() => window.twDialog.restoreFocus(undefined)).not.toThrow();
        });

        test('restoreFocus focuses the captured element and cleans up its token attribute', () => {
            const button = document.createElement('button');
            document.body.appendChild(button);
            button.focus();
            const token = window.twDialog.captureFocus();
            const focusSpy = vi.spyOn(button, 'focus');

            window.twDialog.restoreFocus(token);

            expect(focusSpy).toHaveBeenCalled();
            expect(button.hasAttribute('data-tw-focus-token')).toBe(false);
        });

        test('restoreFocus falls back to querySelector when the element was removed from the in-memory map', () => {
            const button = document.createElement('button');
            document.body.appendChild(button);
            button.focus();
            const token = window.twDialog.captureFocus();
            window.twDialog._focusMap.delete(token); // simulate the map entry going stale

            const focusSpy = vi.spyOn(button, 'focus');
            window.twDialog.restoreFocus(token);

            expect(focusSpy).toHaveBeenCalled();
        });

        test('restoreFocus does nothing when the element can no longer be found', () => {
            // A token that was never captured (or whose element was already removed).
            expect(() => window.twDialog.restoreFocus('tw-focus-does-not-exist')).not.toThrow();
        });
    });
});

describe('twSlider', () => {
    afterEach(() => {
        document.body.innerHTML = '';
    });

    describe('preventScrollKeys', () => {
        test('does nothing when el is null', () => {
            expect(() => window.twSlider.preventScrollKeys(null)).not.toThrow();
        });

        test('registers a keydown listener exactly once per element', () => {
            const el = document.createElement('div');
            const addSpy = vi.spyOn(el, 'addEventListener');

            window.twSlider.preventScrollKeys(el);
            window.twSlider.preventScrollKeys(el); // second call is a no-op

            expect(addSpy).toHaveBeenCalledTimes(1);
        });

        test.each(['ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight', 'Home', 'End'])(
            'prevents default for %s',
            (key) => {
                const el = document.createElement('div');
                window.twSlider.preventScrollKeys(el);

                const event = new window.KeyboardEvent('keydown', { key, bubbles: true });
                const preventSpy = vi.spyOn(event, 'preventDefault');
                el.dispatchEvent(event);

                expect(preventSpy).toHaveBeenCalled();
            }
        );

        test('does not prevent default for other keys (e.g. Tab)', () => {
            const el = document.createElement('div');
            window.twSlider.preventScrollKeys(el);

            const event = new window.KeyboardEvent('keydown', { key: 'Tab', bubbles: true });
            const preventSpy = vi.spyOn(event, 'preventDefault');
            el.dispatchEvent(event);

            expect(preventSpy).not.toHaveBeenCalled();
        });
    });
});

describe('twTabs', () => {
    afterEach(() => {
        document.body.innerHTML = '';
    });

    describe('registerKeydownGuard / unregisterKeydownGuard', () => {
        test('does nothing when tablist is null', () => {
            expect(() => window.twTabs.registerKeydownGuard(null)).not.toThrow();
        });

        test('registers a keydown listener exactly once per tablist', () => {
            const tablist = document.createElement('div');
            const addSpy = vi.spyOn(tablist, 'addEventListener');

            window.twTabs.registerKeydownGuard(tablist);
            window.twTabs.registerKeydownGuard(tablist); // second call is a no-op

            expect(addSpy).toHaveBeenCalledTimes(1);
        });

        test.each(['ArrowRight', 'ArrowLeft', 'ArrowUp', 'ArrowDown', 'Home', 'End'])(
            'prevents default for %s',
            (key) => {
                const tablist = document.createElement('div');
                window.twTabs.registerKeydownGuard(tablist);

                const event = new window.KeyboardEvent('keydown', { key, bubbles: true });
                const preventSpy = vi.spyOn(event, 'preventDefault');
                tablist.dispatchEvent(event);

                expect(preventSpy).toHaveBeenCalled();
            }
        );

        test('does not prevent default for Tab, leaving focus free to move out of the tablist', () => {
            const tablist = document.createElement('div');
            window.twTabs.registerKeydownGuard(tablist);

            const event = new window.KeyboardEvent('keydown', { key: 'Tab', bubbles: true });
            const preventSpy = vi.spyOn(event, 'preventDefault');
            tablist.dispatchEvent(event);

            expect(preventSpy).not.toHaveBeenCalled();
        });

        test('unregisterKeydownGuard removes the listener', () => {
            const tablist = document.createElement('div');
            window.twTabs.registerKeydownGuard(tablist);
            const removeSpy = vi.spyOn(tablist, 'removeEventListener');
            const handler = tablist.__twTabsKeydownHandler;

            window.twTabs.unregisterKeydownGuard(tablist);

            expect(removeSpy).toHaveBeenCalledWith('keydown', handler);
            expect(tablist.__twTabsKeydownHandler).toBeUndefined();
        });

        test('unregisterKeydownGuard does nothing when tablist is null', () => {
            expect(() => window.twTabs.unregisterKeydownGuard(null)).not.toThrow();
        });

        test('unregisterKeydownGuard does nothing when no guard was registered', () => {
            const tablist = document.createElement('div');
            const removeSpy = vi.spyOn(tablist, 'removeEventListener');

            window.twTabs.unregisterKeydownGuard(tablist);

            expect(removeSpy).not.toHaveBeenCalled();
        });
    });
});

describe('twSidebar', () => {
    afterEach(() => {
        vi.restoreAllMocks();
        vi.unstubAllGlobals();
    });

    describe('isMobileViewport', () => {
        test('returns false when the viewport matches the "lg" breakpoint (desktop)', () => {
            vi.stubGlobal('matchMedia', vi.fn().mockReturnValue({ matches: true }));

            expect(window.twSidebar.isMobileViewport()).toBe(false);
        });

        test('returns true when the viewport does not match the "lg" breakpoint (mobile)', () => {
            vi.stubGlobal('matchMedia', vi.fn().mockReturnValue({ matches: false }));

            expect(window.twSidebar.isMobileViewport()).toBe(true);
        });

        test('returns true and logs an error when matchMedia throws', () => {
            const error = new Error('matchMedia unavailable');
            vi.stubGlobal('matchMedia', vi.fn().mockImplementation(() => { throw error; }));
            const consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => {});

            expect(window.twSidebar.isMobileViewport()).toBe(true);
            expect(consoleErrorSpy).toHaveBeenCalledWith('twSidebar.isMobileViewport error', error);
        });
    });
});

describe('twColorPicker', () => {
    function mockElement(rect) {
        const el = document.createElement('div');
        el.getBoundingClientRect = () => rect;
        return el;
    }

    describe('relativePosition', () => {
        test('returns [0, 0] when el is null', () => {
            expect(window.twColorPicker.relativePosition(null, 50, 60)).toEqual([0, 0]);
        });

        test('returns the client point translated into element-relative coordinates', () => {
            const el = mockElement({ left: 20, top: 10 });

            expect(window.twColorPicker.relativePosition(el, 50, 60)).toEqual([30, 50]);
        });

        test('clamps to negative offsets when the point is above/left of the element origin', () => {
            const el = mockElement({ left: 100, top: 100 });

            expect(window.twColorPicker.relativePosition(el, 10, 20)).toEqual([-90, -80]);
        });
    });

    describe('getSize', () => {
        test('returns [0, 0] when el is null', () => {
            expect(window.twColorPicker.getSize(null)).toEqual([0, 0]);
        });

        test('returns the element\'s rendered width and height', () => {
            const el = mockElement({ width: 240, height: 32 });

            expect(window.twColorPicker.getSize(el)).toEqual([240, 32]);
        });
    });
});
