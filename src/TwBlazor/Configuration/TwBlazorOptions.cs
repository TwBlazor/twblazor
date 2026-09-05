// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration;

/// <summary>
/// Global configuration options for TwBlazor components.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwBlazorOptions
{
    /// <summary>
    /// Gets or sets the theme configuration for customizing component styles.
    /// </summary>
    /// <remarks>
    /// Use this to override default styles for any component.
    /// Changes here affect all components unless individually overridden.
    /// </remarks>
    public required TwBlazorTheme Theme { get; set; }
}
