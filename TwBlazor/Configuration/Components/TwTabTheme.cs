// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the tabs components (<see cref="TwBlazor.Components.TwTabContainer"/>,
/// <see cref="TwBlazor.Components.TwTab"/>).
/// Override any property to customize tab styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwTabTheme
{
    /// <summary>
    /// Gets or sets the base classes applied to every tab, regardless of state.
    /// </summary>
    public required string TabBase { get; set; }

    /// <summary>
    /// Gets or sets the padding classes for a tab when the container's Dense mode is disabled.
    /// </summary>
    public required string TabPadding { get; set; }

    /// <summary>
    /// Gets or sets the padding classes for a tab when the container's Dense mode is enabled.
    /// </summary>
    public required string TabDensePadding { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the currently active, enabled tab's underline indicator.
    /// </summary>
    public required string ActiveIndicator { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to an inactive, enabled tab, including its hover text color
    /// and the hover-revealed underline indicator.
    /// </summary>
    public required string InactiveIndicator { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to a disabled tab.
    /// </summary>
    public required string DisabledTab { get; set; }

    /// <summary>
    /// Gets or sets the classes for the tablist container that wraps the tab buttons.
    /// </summary>
    public required string TabListContainer { get; set; }

    /// <summary>
    /// Gets or sets the classes for the tab panel container that wraps the active tab's content.
    /// </summary>
    public required string PanelContainer { get; set; }

    /// <summary>
    /// Gets or sets the background classes applied to both the tablist and panel containers, unless
    /// <see cref="TwBlazor.Components.TwTabContainer.TransparentContainer"/> is set.
    /// </summary>
    public required string Background { get; set; }
}
