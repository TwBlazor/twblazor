// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the color picker components (<see cref="TwBlazor.Components.TwColorPicker"/>,
/// <see cref="TwBlazor.Components.ColorPicker.TwColorPickerBody"/>).
/// Override any property to customize color picker styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwColorPickerTheme
{
    /// <summary>
    /// Gets or sets the base classes for the color swatch button/input shown next to the text field.
    /// </summary>
    public required string Swatch { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the swatch when the color picker is disabled.
    /// </summary>
    public required string SwatchDisabled { get; set; }

    /// <summary>
    /// Gets or sets the hover classes applied to the swatch when the color picker is enabled.
    /// </summary>
    public required string SwatchHover { get; set; }

    /// <summary>
    /// Gets or sets the classes for the container that lays out the swatch next to the text field.
    /// </summary>
    public required string InputContainer { get; set; }

    /// <summary>
    /// Gets or sets the positioning classes for the dialog's wrapper, anchoring it beneath the swatch.
    /// </summary>
    public required string DialogPosition { get; set; }

    /// <summary>
    /// Gets or sets the classes for the dialog's surface (background, border radius, shadow, padding).
    /// </summary>
    public required string DialogSurface { get; set; }

    /// <summary>
    /// Gets or sets the classes for the large color preview swatch shown at the top of the dialog.
    /// </summary>
    public required string PreviewSwatch { get; set; }

    /// <summary>
    /// Gets or sets the classes for the saturation/lightness selector square.
    /// </summary>
    public required string SelectorSquare { get; set; }

    /// <summary>
    /// Gets or sets the classes for the draggable thumb on the saturation/lightness selector square.
    /// </summary>
    public required string SelectorThumb { get; set; }

    /// <summary>
    /// Gets or sets the classes for the hue and alpha slider tracks.
    /// </summary>
    public required string SliderTrack { get; set; }

    /// <summary>
    /// Gets or sets the classes for the draggable thumb on the hue and alpha sliders.
    /// </summary>
    public required string SliderThumb { get; set; }

    /// <summary>
    /// Gets or sets the classes for the "Alpha" label and percentage readout beside the alpha slider.
    /// </summary>
    public required string AlphaLabel { get; set; }

    /// <summary>
    /// Gets or sets the classes for the action bar containing the Cancel/Confirm buttons.
    /// </summary>
    public required string ActionBar { get; set; }
}
