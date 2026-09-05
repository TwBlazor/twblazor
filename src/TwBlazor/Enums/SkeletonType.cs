// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Enums;

/// <summary>
/// Defines the placeholder shape rendered by <see cref="TwBlazor.Components.TwSkeleton"/> when it has
/// no <c>ChildContent</c> to mirror.
/// </summary>
public enum SkeletonType
{
    /// <summary>
    /// A single full-width text line placeholder - Default.
    /// </summary>
    Text,

    /// <summary>
    /// A circular placeholder, typically used for avatars and icons.
    /// </summary>
    Circle,

    /// <summary>
    /// A rectangular block placeholder, typically used for images and cards.
    /// </summary>
    Rectangle
}
