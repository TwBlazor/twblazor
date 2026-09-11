// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using TwBlazor.Configuration.Components;
using TwBlazor.Models;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

public partial class TwNavbar : TwBlazorComponentBase, IDisposable
{
    [Inject] private NavigationManager navigationManager { get; set; } = null!;

    private TwSidebarTheme theme => options.Theme.Components.Require<TwSidebarTheme>();
    private bool isMenuOpen;
    private string menuId => $"{Id}-menu";

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content rendered before the navigation menu.
    /// </summary>
    [Parameter] public RenderFragment? BrandContent { get; set; }

    /// <summary>
    /// Gets or sets custom content rendered in place of data-driven navigation items.
    /// </summary>
    [Parameter] public RenderFragment? NavigationContent { get; set; }

    /// <summary>
    /// Gets or sets content rendered after the navigation links.
    /// </summary>
    [Parameter] public RenderFragment? ActionsContent { get; set; }

    /// <summary>
    /// Gets or sets the navigation items rendered when <see cref="NavigationContent"/> is not supplied.
    /// </summary>
    [Parameter] public List<NavigationItem> NavigationItems { get; set; } = [];

    /// <summary>
    /// Gets or sets whether the navbar's responsive menu toggle and collapse behavior is disabled.
    /// </summary>
    /// <remarks>Responsive collapse is on by default; set this to <see langword="true"/> (e.g. via the bare
    /// <c>&lt;TwNavbar DisableResponsiveCollapse /&gt;</c> attribute syntax) to always render
    /// <see cref="NavigationContent"/> and <see cref="ActionsContent"/> without a mobile toggle - useful when a
    /// parent layout, such as <see cref="TwBlazor.Components.TwSidebar"/>, already owns one.</remarks>
    [Parameter] public bool DisableResponsiveCollapse { get; set; }

    /// <summary>
    /// Gets or sets the accessible label for the responsive menu toggle.
    /// </summary>
    [Parameter] public string ToggleAriaLabel { get; set; } = "Open navigation menu";

    /// <summary>
    /// Gets or sets a value indicating whether the navbar should be fixed at the top.
    /// </summary>
    [Parameter] public bool Fixed { get; set; }

    private string navbarClasses =>
        new ClassBuilder(theme.Navbar)
            .AddClass("sticky top-0", !Fixed)
            .AddClass("fixed top-0 left-0 right-0", Fixed)
            .AddClass(Class).Build();

    private string navigationContainerClasses =>
        new ClassBuilder(theme.NavbarNavigation)
            .AddClass("lg:flex-1 lg:flex lg:flex-row lg:w-auto lg:bg-transparent lg:shadow-none lg:p-0", !DisableResponsiveCollapse)
            .AddClass("hidden", !DisableResponsiveCollapse && !isMenuOpen)
            .AddClass($"w-full flex flex-col {theme.NavbarMobileMenu}", !DisableResponsiveCollapse && isMenuOpen)
            .AddClass("flex-1 flex", DisableResponsiveCollapse)
            .Build();

    private string GetNavigationItemClasses(NavigationItem item) =>
        new ClassBuilder(theme.NavbarLink)
            .AddClass(theme.NavbarLinkActive, IsActive(item))
            .Build();

    private bool IsActive(NavigationItem item)
    {
        if (item.IsActive)
            return true;

        if (string.IsNullOrWhiteSpace(item.Href))
            return false;

        return string.Equals(
            navigationManager.ToBaseRelativePath(navigationManager.Uri),
            item.Href.TrimStart('/'),
            StringComparison.OrdinalIgnoreCase);
    }

    private void ToggleMenu() => isMenuOpen = !isMenuOpen;

    public void Dispose() => navigationManager.LocationChanged -= OnLocationChanged;

    // "navigation bar" restates the <nav> landmark's own role, which is redundant for assistive
    // technology (already announced as "navigation") and indistinguishable from other <nav> regions
    // on the page (e.g. TwSidebar's "sidebar navigation"). "Top navigation" actually distinguishes it.
    protected override void OnInitialized()
    {
        if (string.IsNullOrWhiteSpace(AriaLabel))
            AriaLabel = "Top navigation";

        navigationManager.LocationChanged += OnLocationChanged;

        base.OnInitialized();
    }

    // A layout that hosts TwNavbar isn't re-rendered by client-side navigation (only the routed
    // content is), so without this the mobile menu would stay open after a link inside it is
    // followed, and the active-link highlight would never refresh to match the new URL.
    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        isMenuOpen = false;
        _ = InvokeAsync(StateHasChanged);
    }
}
