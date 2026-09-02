globalThis.themeToggle = {
    highlightJsLinkId: "highlight-js-theme",

    loadHighlightTheme: function (isDark) {
        const theme = isDark ? "codeblockDark" : "codeblockLight";
        const href = `_content/TwBlazor.Docs/css/${theme}.css`;

        let link = document.getElementById(this.highlightJsLinkId);

        if (!link) {
            link = document.createElement("link");
            link.id = this.highlightJsLinkId;
            link.rel = "stylesheet";
            document.head.appendChild(link);
        }

        link.href = href;
    },

    toggle: function () {
        const isDark = document.documentElement.classList.toggle("dark");

        if (isDark) {
            localStorage.theme = "dark";
        } else {
            localStorage.theme = "light";
        }

        this.loadHighlightTheme(isDark);

        return isDark;
    },

    isDarkMode: function () {
        return document.documentElement.classList.contains("dark");
    },

    isPreviewPage: function () {
        // Iframe demo pages (e.g. /sidebar/preview, /sidebar/preview-navigation) always
        // render in light mode, regardless of the parent site's theme, to keep the
        // embedded examples simple and consistent.
        return globalThis.location.pathname.endsWith("/preview") ||
            globalThis.location.pathname.includes("/preview-");
    },

    init: function () {
        if (this.isPreviewPage()) {
            document.documentElement.classList.remove("dark");
            this.loadHighlightTheme(false);
            return;
        }

        // Initialize theme on page load
        const isDark = localStorage.theme === "dark" ||
            (!("theme" in localStorage) && globalThis.matchMedia("(prefers-color-scheme: dark)").matches);

        document.documentElement.classList.toggle("dark", isDark);
        this.loadHighlightTheme(isDark);
    }
};

// Initialize on load
themeToggle.init();