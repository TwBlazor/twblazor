// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Enums;

/// <summary>
/// Defines the loading animation played by <see cref="TwBlazor.Components.TwSkeleton"/> placeholders.
/// </summary>
public enum SkeletonAnimation
{
    /// <summary>
    /// A soft opacity pulse - Default.
    /// </summary>
    Pulse,

    /// <summary>
    /// A shimmering highlight that sweeps left-to-right across the placeholder.
    /// </summary>
    Wave,

    /// <summary>
    /// No animation; the placeholder is rendered as a static block.
    /// </summary>
    None
}
