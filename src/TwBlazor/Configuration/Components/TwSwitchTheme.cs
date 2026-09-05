// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Configuration.Color;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for switch components (<see cref="TwBlazor.Components.TwSwitch{T}"/>), including
/// checked-state colors and the structural classes for the input, label, track, and toggle.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwSwitchTheme
{
    /// <summary>
    /// Gets or sets the color palette applied to switches.
    /// </summary>
    public required TwBlazorPalette Colors { get; set; }

    /// <summary>
    /// Gets or sets the classes for the toggle (the draggable knob).
    /// </summary>
    public required string Switch { get; set; }

    /// <summary>
    /// Gets or sets the classes for the track (the background beneath the toggle).
    /// </summary>
    public required string Track { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to the native (visually hidden) checkbox input.
    /// </summary>
    public required string Base { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to the switch's label wrapper.
    /// </summary>
    public required string LabelBase { get; set; }

    /// <summary>
    /// Gets or sets the cursor class applied to the label when the switch is interactive.
    /// </summary>
    public required string LabelInteractiveCursor { get; set; }

    /// <summary>
    /// Gets or sets the cursor class applied to the label when the switch is read-only or disabled.
    /// </summary>
    public required string LabelNonInteractiveCursor { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the label when the switch is disabled.
    /// </summary>
    public required string LabelDisabled { get; set; }
}
