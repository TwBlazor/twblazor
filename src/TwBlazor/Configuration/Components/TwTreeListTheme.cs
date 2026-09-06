// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the tree list component (<see cref="TwBlazor.Components.TwTreeList"/>).
/// Override any property to customize tree list styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwTreeListTheme
{
    /// <summary>
    /// Gets or sets the base classes for the root <c>&lt;ul role="tree"&gt;</c> element.
    /// </summary>
    public required string Container { get; set; }

    /// <summary>
    /// Gets or sets the classes for a nested <c>&lt;ul role="group"&gt;</c> of child nodes.
    /// </summary>
    public required string Group { get; set; }

    /// <summary>
    /// Gets or sets the base classes for a single node's clickable row (the icon + label, excluding its
    /// nested children group).
    /// </summary>
    public required string Row { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to a node's row when <see cref="TwBlazor.Components.TwTreeListItem.Disabled"/> is true.
    /// </summary>
    public required string RowDisabled { get; set; }

    /// <summary>
    /// Gets or sets the classes for the expand/collapse chevron icon, before the rotation applied when open.
    /// </summary>
    public required string ToggleIcon { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the chevron icon when the node's children are expanded.
    /// </summary>
    public required string ToggleIconOpen { get; set; }

    /// <summary>
    /// Gets or sets the classes for a node's label text.
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// Gets or sets the classes for a node's leading icon (folder/file by default, or
    /// <see cref="TwBlazor.Components.TwTreeListItem.Icon"/> when set), shown unless <c>HideIcons</c> is set.
    /// </summary>
    public required string ItemIcon { get; set; }
}
