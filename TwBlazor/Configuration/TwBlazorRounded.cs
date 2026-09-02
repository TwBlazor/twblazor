// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Enums;

namespace TwBlazor.Configuration;

/// <summary>
/// Global configuration for the border-radius (rounded corner) scale shared by all components.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwBlazorRounded
{
    /// <summary>
    /// Gets or sets the default border radius for all components.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="Rounded.Lg"/>.
    /// Individual components can override this setting.
    /// </remarks>
    public Rounded DefaultRounded { get; set; } = Rounded.Lg;

    /// <summary>
    /// Gets or sets the value of the "none" rounded corner style, which represents no rounding.
    /// </summary>
    public string None { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the value of the "sm" rounded corner style, which represents a small rounding.
    /// </summary>
    public string Sm { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the value of the "md" rounded corner style, which represents a medium rounding.
    /// </summary>
    public string Md { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the value of the "lg" rounded corner style, which represents a large rounding.
    /// </summary>
    public string Lg { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the "full" rounded corner style, which represents a fully rounded corner.
    /// </summary>
    public string Full { get; set; } = string.Empty;


    /// <inheritdoc cref="TwBlazorRoundedScale" />
    public TwBlazorRoundedScale RoundedTop { get; set; } = new();

    /// <inheritdoc cref="TwBlazorRoundedScale" />
    public TwBlazorRoundedScale RoundedBottom { get; set; } = new();

    /// <inheritdoc cref="TwBlazorRoundedScale" />
    public TwBlazorRoundedScale RoundedStart { get; set; } = new();

    /// <inheritdoc cref="TwBlazorRoundedScale" />
    public TwBlazorRoundedScale RoundedEnd { get; set; } = new();
}

/// <summary>
/// A reusable set of border-radius classes for one edge/corner group (e.g. top, start).
/// Used by <see cref="TwBlazorRounded"/> for its directional variants.
/// </summary>
public class TwBlazorRoundedScale
{
    /// <summary>
    /// Gets or sets the value of the "none" rounded corner style, which represents no rounding.
    /// </summary>
    public string None { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the "sm" rounded corner style, which represents a small rounding.
    /// </summary>
    public string Sm { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the "md" rounded corner style, which represents a medium rounding.
    /// </summary>
    public string Md { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the "lg" rounded corner style, which represents a large rounding.
    /// </summary>
    public string Lg { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the "full" rounded corner style, which represents a fully rounded corner.
    /// </summary>
    public string Full { get; set; } = string.Empty;
}
