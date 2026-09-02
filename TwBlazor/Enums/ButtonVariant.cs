// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Enums;

/// <summary>
/// Defines the button variants.
/// </summary>
/// <remarks>
/// - Elevated: Medium emphasis with shadow elevation
/// - Filled: High emphasis with filled background (default)
/// - Tonal: Medium emphasis with tonal background
/// - Outlined: Medium emphasis with outline border
/// - Text: Low emphasis with no background or border
/// </remarks>
public enum ButtonVariant
{
    /// <summary>
    /// Elevated button with shadow elevation (Medium emphasis).
    /// </summary>
    /// <remarks>
    /// Uses elevation Level 1 (shadow-sm) at rest, Level 2 (shadow-md) on hover.
    /// Best for actions that need separation from the background.
    /// </remarks>
    Elevated,

    /// <summary>
    /// Filled button with solid background color (High emphasis) - Default.
    /// </summary>
    /// <remarks>
    /// The most prominent button style. Uses no shadow at rest, subtle shadow on hover.
    /// Use for primary actions like "Save", "Submit", "Continue".
    /// </remarks>
    Filled,

    /// <summary>
    /// Outlined button with border (Medium emphasis).
    /// </summary>
    /// <remarks>
    /// Uses transparent background with colored border and text.
    /// Best for secondary actions or paired with filled buttons.
    /// </remarks>
    Outlined,

    /// <summary>
    /// Text button with no background or border (Low emphasis).
    /// </summary>
    /// <remarks>
    /// Most subtle button style with only text and state layer effects.
    /// Best for tertiary actions or inline with content.
    /// </remarks>
    Text
}
