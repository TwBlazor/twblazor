// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Configuration.Color;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the spinner component (<see cref="TwBlazor.Components.TwSpinner"/>).
/// Override any property to customize spinner styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwSpinnerTheme
{
    /// <summary>
    /// Gets or sets the color palette applied to spinners.
    /// </summary>
    public required TwBlazorPalette Colors { get; set; }

    /// <summary>
    /// Gets or sets the classes for the wrapper that hosts the spinning indicator and its optional visible label.
    /// </summary>
    public required string Wrapper { get; set; }

    /// <summary>
    /// Gets or sets the base classes (shape and animation) applied to the spinning indicator itself.
    /// </summary>
    public required string Base { get; set; }

    /// <summary>
    /// Gets or sets the classes for the dimmed ring that sits behind the colored spinning arc.
    /// </summary>
    public required string Track { get; set; }

    /// <summary>
    /// Gets or sets the size and border-width classes for <see cref="Enums.SpinnerSize.Small"/>.
    /// </summary>
    public required string Small { get; set; }

    /// <summary>
    /// Gets or sets the size and border-width classes for <see cref="Enums.SpinnerSize.Medium"/>.
    /// </summary>
    public required string Medium { get; set; }

    /// <summary>
    /// Gets or sets the size and border-width classes for <see cref="Enums.SpinnerSize.Large"/>.
    /// </summary>
    public required string Large { get; set; }

    /// <summary>
    /// Gets or sets the classes for the visible label text shown when <c>ShowLabel</c> is <c>true</c>.
    /// </summary>
    public required string Label { get; set; }
}