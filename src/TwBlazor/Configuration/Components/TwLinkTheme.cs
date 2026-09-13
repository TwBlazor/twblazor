// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the link component (<see cref="TwBlazor.Components.TwLink"/>).
/// Override any property to customize link styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwLinkTheme
{
    /// <summary>
    /// Gets or sets the classes applied to a link when it has no explicit <see cref="TwBlazor.TwBlazorComponentBase.Class"/> -
    /// the default underline/hover/transition treatment layered on top of its color.
    /// </summary>
    public required string Default { get; set; }
}
