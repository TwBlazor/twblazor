// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Enums;

namespace TwBlazor.Configuration;

/// <summary>
/// Global configuration for the shadow (elevation) scale shared by all components.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwBlazorShadow
{
    /// <summary>
    /// Gets or sets the default shadow (elevation) for all components.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="Shadow.Sm"/> (2dp elevation).
    /// Individual components can override this setting.
    /// </remarks>
    public Shadow DefaultShadow { get; set; } = Shadow.Sm;

    /// <summary>
    /// Gets or sets the value of the "none" shadow, which represents no shadow or elevation.
    /// </summary>
    public string None { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the "sm" shadow, which represents a small shadow or elevation.
    /// </summary>
    public string Sm { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the "md" shadow, which represents a medium shadow or elevation.
    /// </summary>
    public string Md { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the "lg" shadow, which represents a large shadow or elevation.
    /// </summary>
    public string Lg { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hover shadow class for sm shadow level.
    /// </summary>
    public string HoverSm { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hover shadow class for md shadow level.
    /// </summary>
    public string HoverMd { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hover shadow class for lg shadow level.
    /// </summary>
    public string HoverLg { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the active shadow class.
    /// </summary>
    public string ActiveMd { get; set; } = string.Empty;
}