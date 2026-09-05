// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the collapse component (<see cref="TwBlazor.Components.TwCollapse"/>).
/// Override any property to customize collapse styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwCollapseTheme
{
    /// <summary>
    /// Gets or sets the base classes for the collapse's outer container.
    /// </summary>
    public required string Container { get; set; }

    /// <summary>
    /// Gets or sets the classes for the trigger button that toggles the panel open and closed.
    /// </summary>
    public required string Trigger { get; set; }

    /// <summary>
    /// Gets or sets the classes for the chevron icon within the trigger, before the rotation applied when open.
    /// </summary>
    public required string Icon { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the chevron icon when the panel is open.
    /// </summary>
    public required string IconOpen { get; set; }

    /// <summary>
    /// Gets or sets the classes for the collapsible content panel.
    /// </summary>
    public required string Content { get; set; }
}
