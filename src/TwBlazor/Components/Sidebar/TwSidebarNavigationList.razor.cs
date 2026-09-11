// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Models;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Recursively renders a list of <see cref="NavigationItem"/>s for <see cref="TwSidebar"/>, expanding any item
/// with nested <see cref="NavigationItem.NavigationItems"/> into its own collapsible group - to any depth.
/// </summary>
/// <remarks>An internal building block of <see cref="TwSidebar"/>, not intended to be used directly by
/// consumers.</remarks>
public partial class TwSidebarNavigationList : TwBlazorComponentBase
{
    private TwSidebarTheme theme => options.Theme.Components.Require<TwSidebarTheme>();

    /// <summary>
    /// Gets or sets the navigation items to render at this level.
    /// </summary>
    [Parameter, EditorRequired] public List<NavigationItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets whether this list is itself the children of a parent item (as opposed to the sidebar's
    /// top-level items).
    /// </summary>
    /// <remarks>Changes how each item is wrapped (in an <c>&lt;li&gt;</c>, to be valid inside the parent's
    /// <c>&lt;ul&gt;</c>) and rounded (grouped with its siblings rather than independently rounded). Always
    /// <see langword="true"/> for a recursive call rendering a parent item's own children, regardless of depth.</remarks>
    [Parameter] public bool IsNested { get; set; }

    /// <summary>
    /// Gets or sets how many levels deep <see cref="Items"/> sit (0 for the sidebar's top-level items,
    /// 1 for a parent's children, 2 for grandchildren, and so on).
    /// </summary>
    /// <remarks>Passed straight through to each rendered <see cref="TwSidebarItem.Depth"/>, and
    /// incremented by one on every recursive call rendering a parent item's own children.</remarks>
    [Parameter] public int Depth { get; set; }

    private string GetChildContainerClasses(bool collapsed, int childDepth) =>
        new ClassBuilder(theme.NavigationDropdownContainer)
            .AddClass(theme.NavigationDropdownContainerDeep, childDepth >= 2)
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
