using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwBlazor.Enums;
using TwBlazor.Models;

namespace TwBlazor.Docs.Layout;

public partial class Navigation : IDisposable
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public bool MainContentPadding { get; set; }

    private Icon themeIcon = Icon.Moon; // NOSONAR - used in Navigation.razor template

#pragma warning disable S1075 // Fixed external documentation link, not environment-specific
    private static readonly string _apiDocumentationUri = "https://twblazor.github.io/twblazor/";
#pragma warning restore S1075

    // Category order in the sidebar follows components.json's array order.
    private readonly List<NavigationItem> _navigationItems = BuildNavigationItems();

    private static List<NavigationItem> BuildNavigationItems()
    {
        List<NavigationItem> items =
        [
            new() { Id = "home", Label = "Home", Href = "/" },
            new() { Id = "get-started", Label = "Get started", Href = "/get-started" },
        ];

        foreach (var category in LoadComponentCategories())
        {
            var children = category.Items
                .Select(c => new NavigationItem { Id = c.Id, Label = c.Display, Href = c.Url, New = c.IsNew })
                .ToList();

            items.Add(new NavigationItem { Id = category.Category.ToLowerInvariant(), Label = category.Category, NavigationItems = children });
        }

        items.Add(new() { Id = "api-doc", Label = "API Documentation", Href = _apiDocumentationUri });

        return items;
    }

    private static List<ComponentCategory> LoadComponentCategories()
    {
        var assembly = typeof(Navigation).Assembly;
        using var stream = assembly.GetManifestResourceStream("TwBlazor.Docs.components.json")
            ?? throw new InvalidOperationException("Embedded resource 'components.json' was not found.");

        return JsonSerializer.Deserialize<List<ComponentCategory>>(stream, JsonSerializerOptions.Web) ?? [];
    }

    private sealed class ComponentCategory
    {
        public string Category { get; set; } = string.Empty;
        public List<ComponentNavEntry> Items { get; set; } = [];
    }

    private sealed class ComponentNavEntry
    {
        public string Id { get; set; } = string.Empty;
        public string Display { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsNew { get; set; }
    }

    private readonly CancellationTokenSource _cts = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                var isDark = await JS.InvokeAsync<bool>(
                    "themeToggle.isDarkMode",
                    _cts.Token);
                themeIcon = isDark ? Icon.Sun : Icon.Moon;
                StateHasChanged();
            }
            catch (OperationCanceledException)
            {
                // Circuit was interrupted (hot reload, navigation, or disconnect) — expected.
            }
        }
    }

    private async Task ToggleTheme()
    {
        var isDark = await JS.InvokeAsync<bool>("themeToggle.toggle", _cts.Token);
        themeIcon = isDark ? Icon.Sun : Icon.Moon;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
