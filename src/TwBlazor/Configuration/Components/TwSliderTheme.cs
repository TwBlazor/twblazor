// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Configuration.Color;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for slider components.
/// Override any property to customize slider styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwSliderTheme
{
    /// <summary>
    /// Gets or sets the color palette applied to sliders.
    /// </summary>
    public required TwBlazorPalette Colors { get; set; }

    /// <summary>
    /// Gets or sets the classes for the outer wrapper that hosts the interactive input and the custom visual track.
    /// </summary>
    public required string Wrapper { get; set; }

    /// <summary>
    /// Gets or sets the classes for the native range input. It is stretched over the full wrapper and made
    /// invisible so that pointer, keyboard and touch interaction (and accessibility semantics) are still
    /// handled natively, while the visible track/fill/thumb are drawn separately for full styling control.
    /// </summary>
    public required string Base { get; set; }

    /// <summary>
    /// Gets or sets the classes for the background track behind the filled portion of the slider.
    /// </summary>
    public required string Fill { get; set; }

    /// <summary>
    /// Gets or sets the classes for the filled portion of the track, from the start up to the current value.
    /// </summary>
    public required string Track { get; set; }

    /// <summary>
    /// Gets or sets the classes for the draggable thumb positioned at the current value.
    /// </summary>
    public required string Thumb { get; set; }

    /// <summary>
    /// Gets or sets the classes for the floating value tooltip shown above the thumb on hover/focus.
    /// </summary>
    public required string Bubble { get; set; }
}