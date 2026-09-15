// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Configuration.Components;

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Enums;

/// <summary>
/// Theme configuration for input components (<see cref="TwBlazor.Components.TwTextfield{T}"/> and
/// <see cref="TwBlazor.Components.TwSelect{T}"/>).
/// Override any property to customize input styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwInputTheme
{
    /// <summary>
    /// Gets or sets the default input variant for text fields and selects.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="InputVariant.Filled"/>.
    /// Individual components can override this setting.
    /// </remarks>
    public InputVariant DefaultInputVariant { get; set; } = InputVariant.Filled;

    /// <summary>
    /// Gets or sets the base classes applied to text fields and text areas.
    /// </summary>
    public required string TextfieldBase { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to selects.
    /// </summary>
    public required string SelectBase { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to each <c>&lt;option&gt;</c> in a select.
    /// </summary>
    public required string SelectOption { get; set; }

    /// <summary>
    /// Gets or sets the background classes forced onto a select using the Default/Outlined variant.
    /// A native <c>&lt;select&gt;</c> popup renders using the element's own background/text colors, so
    /// a transparent one (those variants' usual background) falls back to the OS's native, often
    /// light, popup surface and can pair unreadable white dark-mode text onto it.
    /// </summary>
    public required string SelectNativeBackground { get; set; }

    /// <summary>
    /// Gets or sets the horizontal padding applied to a select using the <see cref="Enums.InputVariant.Default"/>
    /// variant, which otherwise has none of its own (unlike Filled/Outlined).
    /// </summary>
    public required string SelectDefaultPadding { get; set; }

    /// <summary>
    /// Gets or sets the background override applied to a read-only select, suppressing its dropdown
    /// arrow background image so it doesn't imply the value can still be changed. Also applied to a
    /// multi-select (<see cref="TwBlazor.Components.TwSelect{T}.Multiple"/>), since that renders as
    /// an inline scrollable listbox rather than a closed dropdown, so the same "no arrow" treatment applies.
    /// </summary>
    public required string SelectReadOnlyBackground { get; set; }

    /// <summary>
    /// Gets or sets the base classes for input labels.
    /// </summary>
    public required string LabelBase { get; set; }

    /// <summary>
    /// Gets or sets the CSS classes applied to the input legend (group) element to define its visual styling.
    /// </summary>
    public required string InputLegendBase { get; set; }

    /// <summary>
    /// Gets or sets the default border for outlined inputs.
    /// </summary>
    public required string OutlinedBorder { get; set; }

    /// <summary>
    /// Gets or sets the default border for filled inputs.
    /// </summary>
    public required string FilledBorder { get; set; }

    /// <summary>
    /// Gets or sets the focus border for inputs.
    /// </summary>
    public required string FocusBorder { get; set; }

    /// <summary>
    /// Gets or sets the filled variant background color.
    /// </summary>
    public required string FilledBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the classes for an input's validation error message, rendered by <see cref="TwBlazor.Components.TwInputRoot"/>.
    /// </summary>
    public required string ErrorMessage { get; set; }
}