// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Color;

/// <summary>
/// A palette of colors for a single semantic role (e.g. primary, success), covering both light and dark modes.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwBlazorPalette
{
    /// <summary>
    /// Gets or sets the primary (blue) color for both light and dark modes.
    /// </summary>
    public string Primary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the accent (brand) color for both light and dark modes.
    /// </summary>
    public string Accent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the success (green) color for both light and dark modes.
    /// </summary>
    public string Success { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the danger (red) color for both light and dark modes.
    /// </summary>
    public string Danger { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the warning (yellow) color for both light and dark modes.
    /// </summary>
    public string Warning { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the info (purple) color for both light and dark modes.
    /// </summary>
    public string Info { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the light (white) color for both light and dark modes.
    /// </summary>
    public string Light { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dark (black) color for both light and dark modes.
    /// </summary>
    public string Dark { get; set; } = string.Empty;
}
