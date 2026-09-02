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
    private static readonly string _apiDocumentationUri = "https://twblazor.github.io/TwBlazor/";
#pragma warning restore S1075

    private readonly List<NavigationItem> _navigationItems =
    [
        new() { Id = "home", Label = "Home", Href = "/" },
        new() { Id = "get-started", Label = "Get started", Href = "/get-started" },
        new()
        {
            Id = "data",
            Label = "Data",
            NavigationItems =
            [
                new() { Id = "data-table", Label = "Data Table", Href = "/data-table" },
                new() { Id = "table", Label = "Table", Href = "/table" },
                new() { Id = "pagination", Label = "Pagination", Href = "/pagination" },
            ]
        },
        new()
        {
            Id = "feedback",
            Label = "Feedback",
            NavigationItems =
            [
                new() { Id = "alert", Label = "Alert", Href = "/alert" },
                new() { Id = "chip", Label = "Chip", Href = "/chip" },
                new() { Id = "icon", Label = "Icon", Href = "/icon" },
                new() { Id = "progress", Label = "Progress", Href = "/progress", New = true },
                new() { Id = "spinner", Label = "Spinner", Href = "/spinner", New = true }
            ]
        },
        new()
        {
            Id = "forms",
            Label = "Forms",
            NavigationItems =
            [
                new() { Id = "button", Label = "Button", Href = "/button" },
                new() { Id = "checkbox", Label = "Checkbox", Href = "/checkbox" },
                new() { Id = "color-picker", Label = "Color Picker", Href = "/color-picker", New = true },
                new() { Id = "date-picker", Label = "Date Picker", Href = "/date-picker" },
                new() { Id = "datetime-picker", Label = "Datetime Picker", Href = "/datetime-picker" },
                new() { Id = "file-upload", Label = "File Upload", Href = "/file-upload" },
                new() { Id = "radio-button", Label = "Radio Button", Href = "/radio-button" },
                new() { Id = "select", Label = "Select", Href = "/select" },
                new() { Id = "slider", Label = "Slider", Href = "/slider", New = true },
                new() { Id = "switch", Label = "Switch", Href = "/switch", New = true },
                new() { Id = "textfield", Label = "Textfield", Href = "/textfield" },
                new() { Id = "time-picker", Label = "Time Picker", Href = "/time-picker" },
            ]
        },
        new()
        {
            Id = "layout",
            Label = "Layout",
            NavigationItems =
            [
                new() { Id = "breadcrumb", Label = "Breadcrumb", Href = "/breadcrumb" },
                new() { Id = "card", Label = "Card", Href = "/card" },
                new() { Id = "collapse", Label = "Collapse", Href = "/collapse" },
                new() { Id = "sidebar", Label = "Sidebar", Href = "/sidebar" },
                new() { Id = "tabs", Label = "Tabs", Href = "/tabs" },
            ]
        },
        new()
        {
            Id = "services",
            Label = "Services",
            NavigationItems =
            [
                new() { Id = "dialog", Label = "Dialog Service", Href = "/dialog" },
                new() { Id = "toast", Label = "Toast Service", Href = "/toast" },
            ]
        },
        new() { Id = "api-doc", Label = "API Documentation", Href = _apiDocumentationUri },
    ];

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
