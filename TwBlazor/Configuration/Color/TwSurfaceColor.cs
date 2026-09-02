// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Color;

/// <summary>
/// Color configuration for button surface variants, covering filled, text, and outlined buttons.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwSurfaceColor
{
    /// <summary>
    /// Gets or sets the color palette for filled button surfaces.
    /// </summary>
    public TwBlazorPalette Filled { get; set; } = new();
    /// <summary>
    /// Gets or sets the color palette for text button surfaces.
    /// </summary>
    public TwBlazorPalette Text { get; set; } = new();
    /// <summary>
    /// Gets or sets the color palette for outlined button surfaces.
    /// </summary>
    public TwBlazorPalette Outlined { get; set; } = new();
}
