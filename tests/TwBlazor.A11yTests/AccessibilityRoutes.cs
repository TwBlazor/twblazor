using System.Reflection;
using System.Text.Json;

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
            foreach (var route in _all)
            {
                data.Add(route, false);
                if (!_previewRoutes.Contains(route))
                {
                    data.Add(route, true);
                }
            }
            return data;
        }
    }

    private static readonly string[] _previewRoutes =
    [
        "/sidebar/preview",
        "/sidebar/preview-navigation",
    ];

    // Routable pages that aren't a documented component and so have no entry in components.json.
    private static readonly string[] _nonComponentRoutes =
    [
        "/",
        "/get-started",
        "/theme",
    ];

    private static readonly string[] _all = BuildAllRoutes();

    private static string[] BuildAllRoutes()
    {
        List<string> routes = [.. _nonComponentRoutes, .. _previewRoutes, .. LoadComponentRoutes()];
        return [.. routes.OrderBy(route => route, StringComparer.Ordinal)];
    }

    private static IEnumerable<string> LoadComponentRoutes()
    {
        var docsAssembly = Assembly.Load("TwBlazor.Docs");
        using var stream = docsAssembly.GetManifestResourceStream("TwBlazor.Docs.components.json")
            ?? throw new InvalidOperationException("Embedded resource 'components.json' was not found.");

        var categories = JsonSerializer.Deserialize<List<ComponentCategory>>(stream, JsonSerializerOptions.Web) ?? [];
        return categories.SelectMany(c => c.Items).Select(e => e.Url);
    }

    private sealed class ComponentCategory
    {
        public List<ComponentEntry> Items { get; set; } = [];
    }

    private sealed class ComponentEntry
    {
        public string Url { get; set; } = string.Empty;
    }
}
