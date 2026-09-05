// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Models;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a responsive sidebar layout component with customizable header, navbar, and sidebar content areas.
/// </summary>
/// <remarks>Use <c>TwSidebar</c> to create a page layout with a collapsible sidebar, optional header, and
/// navigation bar. The component supports dynamic content injection via <see cref="RenderFragment"/> parameters and
/// provides properties to control appearance, layout, and interactivity. Sidebar open state and navigation items can be
/// data-bound for integration with application state. This component is intended for use in Blazor applications and is
/// designed to be flexible for a variety of layout scenarios.</remarks>
public partial class TwSidebar : TwBlazorComponentBase
{
    private TwSidebarTheme theme => options.Theme.Components.Require<TwSidebarTheme>();

    /// <summary>
    /// Gets or sets a value indicating whether the sidebar is currently open.
    /// </summary>
    [Parameter] public bool IsSidebarOpen { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the sidebar open state changes.
    /// </summary>
    /// <remarks>Use this callback to respond to changes in the sidebar's visibility, such as updating
    /// application state or triggering additional actions when the sidebar is opened or closed.</remarks>
    [Parameter] public EventCallback<bool> IsSidebarOpenChanged { get; set; }

    /// <summary>
    /// Binds the searchable state of the sidebar, allows for sidebar items to be searched.
    /// </summary>
    [Parameter] public bool IsSearchable { get; set; }

    /// <summary>
    /// The page body content.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to render in the header area of the sidebar.
    /// </summary>
    /// <remarks>Assign a <see cref="RenderFragment"/> to customize the sidebar header's appearance and layout. If
    /// <see langword="null"/>, the sidebars header will be hidden.</remarks>
    [Parameter] public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered in the navbar area of the component.
    /// </summary>
    [Parameter] public RenderFragment? NavbarContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered in the sidebar area of the component.
    /// </summary>
    /// <remarks>Assign a <see cref="RenderFragment"/> to customize the sidebar's appearance and layout. If
    /// <see langword="null"/>, the sidebar will not display any content.</remarks>
    [Parameter] public RenderFragment? SidebarContent { get; set; }

    /// <summary>
    /// Gets or sets the collection of sidebar items to be displayed in the component.
    /// </summary>
    /// <remarks>If <see langword="null"/> or empty, no sidebar items will be rendered. Changes to this
    /// collection will update the displayed sidebar items accordingly.</remarks>
    [Parameter] public List<NavigationItem> NavigationItems { get; set; } = [];

    /// <summary>
    /// Gets the items currently displayed in the sidebar, either filtered or the full list.
    /// </summary>
    private List<NavigationItem> displayedNavigationItems { get; set; } = [];

    /// <summary>
    /// Gets the unfiltered collection of sidebar items, used for search functionality to maintain the original list of items.
    /// </summary>
    private List<NavigationItem> unfilteredNavigationItems { get; set; } = [];

    /// <summary>
    /// The classes that are applied to the parent div for the sidebar.
    /// </summary>
    [Parameter] public string SidebarClass { get; set; } = string.Empty;

    /// <summary>
    /// The classes that are applied to the parent div for the navbar.
    /// </summary>
    [Parameter] public string NavbarClass { get; set; } = string.Empty;

    /// <summary>
    /// The classes that are applied to the toggle snackbar.
    /// </summary>
    [Parameter] public string ToggleButtonClass { get; set; } = string.Empty;

    /// <summary>
    /// The classes that are applied to the main content div.
    /// </summary>
    [Parameter] public string MainContentClass { get; set; } = string.Empty;

    /// <summary>
    /// The classes that are applied to the main content root div.
    /// </summary>
    [Parameter] public string MainContentRootClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the navbar should be fixed at the top.
    /// </summary>
    [Parameter] public bool IsNavbarFixed { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        unfilteredNavigationItems = NavigationItems ?? [];
        ApplyFilter();
    }

    private async Task ToggleSidebar()
    {
        IsSidebarOpen = !IsSidebarOpen;

        if (IsSidebarOpenChanged.HasDelegate)
        {
            await IsSidebarOpenChanged.InvokeAsync(IsSidebarOpen);
        }
    }

    private string searchTerm { get; set; } = string.Empty;

    private void OnSearchInput(string value)
    {
        searchTerm = value;
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            displayedNavigationItems = unfilteredNavigationItems;
        }
        else
        {
            displayedNavigationItems = unfilteredNavigationItems
                .Select(item => FilterNavigationItem(item, searchTerm))
                .Where(item => item is not null)
                .Cast<NavigationItem>()
                .ToList();
        }
    }

    private static NavigationItem? FilterNavigationItem(NavigationItem item, string term)
    {
        var labelMatches = item.Label?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false;

        var matchingChildren = item.NavigationItems
            .Select(child => FilterNavigationItem(child, term))
            .Where(child => child is not null)
            .Cast<NavigationItem>()
            .ToList();

        if (labelMatches)
        {
            return new NavigationItem
            {
                Id = item.Id,
                Href = item.Href,
                Icon = item.Icon,
                Label = item.Label,
                Collapsed = item.Collapsed,
                TopNavigation = item.TopNavigation,
                Hidden = item.Hidden,
                NavigationItems = item.NavigationItems
            };
        }

        if (matchingChildren.Count > 0)
        {
            return new NavigationItem
            {
                Id = item.Id,
                Href = item.Href,
                Icon = item.Icon,
                Label = item.Label,
                Collapsed = false,
                TopNavigation = item.TopNavigation,
                Hidden = item.Hidden,
                NavigationItems = matchingChildren
            };
        }

        return null;
    }

    private string rootClasses =>
        new ClassBuilder("relative flex w-full flex-row")
            .AddClass(Class)
            .Build();

    private string sidebarClasses =>
        new ClassBuilder(theme.Sidebar)
            .AddClass("fixed -translate-x-full", !IsSidebarOpen)
            .AddClass("fixed lg:relative translate-x-0", IsSidebarOpen)
            .AddClass(SidebarClass).Build();

    private string mainContentClasses =>
        new ClassBuilder(theme.MainContent)
            .AddClass("absolute h-full", IsNavbarFixed)
            .AddClass("relative", !IsNavbarFixed)
            .AddClass(MainContentClass).Build();

    private string mainContentRootClasses =>
        new ClassBuilder(theme.MainContentRoot)
            .AddClass(MainContentRootClass).Build();

    private string GetChildContainerClasses(bool collapsed) =>
        new ClassBuilder(theme.NavigationDropdownContainer)
            .AddClass("hidden", collapsed)
            .AddClass(roundedBuilder.GetRoundedBottom(effectiveRounded), !collapsed)
            .Build();

    /// <summary>
    /// Gets a stable identifier for a parent navigation item, used to link the toggle button to its
    /// collapsible child container via <c>aria-controls</c>. Falls back to a value derived from the item
    /// instance when <see cref="NavigationItem.Id"/> is not supplied by the consumer.
    /// </summary>
    private static string GetParentItemId(NavigationItem item) =>
        !string.IsNullOrWhiteSpace(item.Id) ? item.Id : $"sidebar-item-{item.GetHashCode()}";
}
