// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Configuration.Color;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for radio button components (<see cref="TwBlazor.Components.TwRadioButton{T}"/>),
/// including checked-state colors and the structural classes for the input, label, and radio dot icon.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwRadioButtonTheme
{
    /// <summary>
    /// Gets or sets the color palette applied to radio buttons.
    /// </summary>
    public required TwBlazorPalette Colors { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to the native radio input.
    /// </summary>
    public required string Base { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to a disabled radio input.
    /// </summary>
    public required string Disabled { get; set; }

    /// <summary>
    /// Gets or sets the hover classes applied to an enabled radio input.
    /// </summary>
    public required string Hover { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to the radio button's label wrapper.
    /// </summary>
    public required string LabelBase { get; set; }

    /// <summary>
    /// Gets or sets the cursor class applied to the label when the radio button is interactive.
    /// </summary>
    public required string LabelInteractiveCursor { get; set; }

    /// <summary>
    /// Gets or sets the cursor class applied to the label when the radio button is read-only or disabled.
    /// </summary>
    public required string LabelNonInteractiveCursor { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the label when the radio button is disabled.
    /// </summary>
    public required string LabelDisabled { get; set; }

    /// <summary>
    /// Gets or sets the classes for the radio dot icon wrapper shown when the radio button is checked.
    /// </summary>
    public required string IconWrapper { get; set; }
}
