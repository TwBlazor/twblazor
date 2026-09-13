// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the code block component (<see cref="TwBlazor.Components.TwCodeBlock"/>).
/// Override any property to customize code block styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwCodeBlockTheme
{
    /// <summary>
    /// Gets or sets the background/overflow classes applied to the block's root container.
    /// </summary>
    public required string Container { get; set; }

    /// <summary>
    /// Gets or sets the classes for the copy button's positioning wrapper, in the block's top-right corner.
    /// </summary>
    public required string CopyButtonWrapper { get; set; }
}
