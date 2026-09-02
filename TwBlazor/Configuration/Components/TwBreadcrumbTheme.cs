// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the breadcrumb components (<see cref="TwBlazor.Components.TwBreadcrumb"/>,
/// <see cref="TwBlazor.Components.TwBreadcrumbItem"/>).
/// Override any property to customize breadcrumb styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwBreadcrumbTheme
{
    /// <summary>
    /// Gets or sets the classes for the breadcrumb's outer &lt;ol&gt; list.
    /// </summary>
    public required string List { get; set; }

    /// <summary>
    /// Gets or sets the classes for each &lt;li&gt; item, laying out its separator, optional icon,
    /// and label in a row.
    /// </summary>
    public required string Item { get; set; }

    /// <summary>
    /// Gets or sets the classes for the "/" separator rendered ahead of every item except the first.
    /// </summary>
    public required string Separator { get; set; }

    /// <summary>
    /// Gets or sets the classes for an item's label text (the &lt;a&gt; or current-page &lt;span&gt;).
    /// </summary>
    public required string Label { get; set; }
}
