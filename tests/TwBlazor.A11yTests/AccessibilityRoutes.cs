namespace TwBlazor.A11yTests;

/// <summary>
/// Every routable page in TwBlazor.Docs (one demo page per component, showing every color/state
/// variant), paired with whether a dark-mode pass should also be scanned. The two sidebar preview
/// routes always force light mode (see TwBlazor.Docs' themeToggle.js `isPreviewPage`), so a dark
/// pass there wouldn't reflect anything a real user can reach.
/// </summary>
public static class AccessibilityRoutes
{
    public static TheoryData<string, bool> LightAndDark
    {
        get
        {
            var data = new TheoryData<string, bool>();
            foreach (var route in All)
            {
                data.Add(route, false);
                if (!PreviewRoutes.Contains(route))
                {
                    data.Add(route, true);
                }
            }
            return data;
        }
    }

    private static readonly string[] PreviewRoutes =
    [
        "/sidebar/preview",
        "/sidebar/preview-navigation",
    ];

    private static readonly string[] All =
    [
        "/",
        "/alert",
        "/breadcrumb",
        "/button",
        "/card",
        "/checkbox",
        "/chip",
        "/collapse",
        "/color-picker",
        "/data-table",
        "/date-picker",
        "/datetime-picker",
        "/dialog",
        "/file-upload",
        "/get-started",
        "/icon",
        "/pagination",
        "/progress",
        "/radio-button",
        "/select",
        "/sidebar",
        "/sidebar/preview",
        "/sidebar/preview-navigation",
        "/skeleton",
        "/slider",
        "/spinner",
        "/switch",
        "/table",
        "/tabs",
        "/textfield",
        "/theme",
        "/time-picker",
        "/toast",
    ];
}
