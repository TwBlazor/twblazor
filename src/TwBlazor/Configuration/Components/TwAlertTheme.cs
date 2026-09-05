// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Configuration.Color;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Color configuration for alert components (<see cref="TwBlazor.Components.TwAlert"/>), including
/// background, text, and border colors for light and dark modes.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwAlertTheme
{
    /// <summary>
    /// Gets or sets the color palette applied to alerts.
    /// </summary>
    public required TwBlazorPalette Colors { get; set; }
}
