// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Configuration.Components;

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Enums;

/// <summary>
/// Theme configuration for input components (<see cref="TwBlazor.Components.TwTextfield{T}"/> and
/// <see cref="TwBlazor.Components.TwSelect{T}"/>).
/// Override any property to customize input styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwInputTheme
{
    /// <summary>
    /// Gets or sets the default input variant for text fields and selects.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="InputVariant.Filled"/>.
    /// Individual components can override this setting.
    /// </remarks>
    public InputVariant DefaultInputVariant { get; set; } = InputVariant.Filled;

    /// <summary>
    /// Gets or sets the base classes applied to text fields and text areas.
    /// </summary>
    public required string TextfieldBase { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to selects.
    /// </summary>
    public required string SelectBase { get; set; }

    /// <summary>
    /// Gets or sets the base classes for input labels.
    /// </summary>
    public required string LabelBase { get; set; }

    /// <summary>
    /// Gets or sets the CSS classes applied to the input legend (group) element to define its visual styling.
    /// </summary>
    public required string InputLegendBase { get; set; }

    /// <summary>
    /// Gets or sets the default border for outlined inputs.
    /// </summary>
    public required string OutlinedBorder { get; set; }

    /// <summary>
    /// Gets or sets the default border for filled inputs.
    /// </summary>
    public required string FilledBorder { get; set; }

    /// <summary>
    /// Gets or sets the focus border for inputs.
    /// </summary>
    public required string FocusBorder { get; set; }

    /// <summary>
    /// Gets or sets the filled variant background color.
    /// </summary>
    public required string FilledBackgroundColor { get; set; }
}