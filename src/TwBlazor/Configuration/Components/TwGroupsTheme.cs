// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for grouped input containers (<see cref="TwBlazor.Components.TwRadioGroup{T}"/>,
/// <see cref="TwBlazor.Components.TwCheckboxGroup{TValue}"/>, <see cref="TwBlazor.Components.TwButtonGroup"/>,
/// <see cref="TwBlazor.Components.TwChipGroup"/>).
/// Override any property to customize group spacing and layout globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwGroupsTheme
{
    /// <summary>
    /// Gets or sets the default gap classes applied between items in a group.
    /// </summary>
    public required string Gap { get; set; }

    /// <summary>
    /// Gets or sets the base classes shared by the fieldset-based groups
    /// (<see cref="TwBlazor.Components.TwCheckboxGroup{TValue}"/>, <see cref="TwBlazor.Components.TwRadioGroup{T}"/>).
    /// </summary>
    public required string FieldsetBase { get; set; }

    /// <summary>
    /// Gets or sets the layout classes for a fieldset-based group in horizontal orientation.
    /// </summary>
    public required string HorizontalLayout { get; set; }

    /// <summary>
    /// Gets or sets the layout classes for a fieldset-based group in vertical orientation (the default).
    /// </summary>
    public required string VerticalLayout { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to a disabled <see cref="TwBlazor.Components.TwCheckboxGroup{TValue}"/>.
    /// </summary>
    public required string CheckboxGroupDisabled { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to a disabled <see cref="TwBlazor.Components.TwRadioGroup{T}"/>.
    /// </summary>
    public required string RadioGroupDisabled { get; set; }

    /// <summary>
    /// Gets or sets the base classes for <see cref="TwBlazor.Components.TwButtonGroup"/>.
    /// </summary>
    public required string ButtonGroupBase { get; set; }

    /// <summary>
    /// Gets or sets the layout classes for a vertically-oriented <see cref="TwBlazor.Components.TwButtonGroup"/>.
    /// </summary>
    public required string ButtonGroupVertical { get; set; }

    /// <summary>
    /// Gets or sets the layout classes for a horizontally-oriented <see cref="TwBlazor.Components.TwButtonGroup"/> (the default).
    /// </summary>
    public required string ButtonGroupHorizontal { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to a full-width <see cref="TwBlazor.Components.TwButtonGroup"/>.
    /// </summary>
    public required string ButtonGroupFullWidth { get; set; }

    /// <summary>
    /// Gets or sets the classes that stretch each button to share the row equally in a full-width,
    /// horizontally-oriented <see cref="TwBlazor.Components.TwButtonGroup"/>.
    /// </summary>
    public required string ButtonGroupFullWidthRow { get; set; }

    /// <summary>
    /// Gets or sets the base classes for <see cref="TwBlazor.Components.TwChipGroup"/>.
    /// </summary>
    public required string ChipGroupBase { get; set; }

    /// <summary>
    /// Gets or sets the classes for a <see cref="TwBlazor.Components.TwChipGroup"/> with <c>Alignment="start"</c> (the default).
    /// </summary>
    public required string ChipGroupAlignStart { get; set; }

    /// <summary>
    /// Gets or sets the classes for a <see cref="TwBlazor.Components.TwChipGroup"/> with <c>Alignment="center"</c>.
    /// </summary>
    public required string ChipGroupAlignCenter { get; set; }

    /// <summary>
    /// Gets or sets the classes for a <see cref="TwBlazor.Components.TwChipGroup"/> with <c>Alignment="end"</c>.
    /// </summary>
    public required string ChipGroupAlignEnd { get; set; }
}
