// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the time picker components (<see cref="TwBlazor.Components.TwTimePicker"/>,
/// <see cref="TwBlazor.Components.TimePicker.TwTimePickerBody"/>).
/// Override any property to customize time picker styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwTimePickerTheme
{
    /// <summary>
    /// Gets or sets the base classes for the picker's outer wrapper, positioning the trigger icon,
    /// text field, and popover panel.
    /// </summary>
    public required string PickerRoot { get; set; }

    /// <summary>
    /// Gets or sets the classes for the clock icon's clickable wrapper within the trigger text field.
    /// </summary>
    public required string IconWrapper { get; set; }

    /// <summary>
    /// Gets or sets the classes for the clock glyph itself.
    /// </summary>
    public required string IconGlyph { get; set; }

    /// <summary>
    /// Gets or sets the padding classes for the trigger text field, leaving room for the clock icon.
    /// </summary>
    public required string TextfieldPadding { get; set; }

    /// <summary>
    /// Gets or sets the positioning classes for the popover panel's wrapper, anchoring it beneath the trigger.
    /// </summary>
    public required string PanelPosition { get; set; }

    /// <summary>
    /// Gets or sets the classes for the popover panel's surface (background, border, shadow, padding).
    /// </summary>
    public required string BodySurface { get; set; }

    /// <summary>
    /// Gets or sets the base classes for the time picker body's own outer container.
    /// </summary>
    public required string BodyRoot { get; set; }

    /// <summary>
    /// Gets or sets the classes for the time picker body's inner content wrapper.
    /// </summary>
    public required string BodyInner { get; set; }

    /// <summary>
    /// Gets or sets the classes for the row laying out the hour, minute, and AM/PM controls.
    /// </summary>
    public required string ContentRow { get; set; }

    /// <summary>
    /// Gets or sets the classes for each hour/minute column (stepper buttons and number input).
    /// </summary>
    public required string Column { get; set; }

    /// <summary>
    /// Gets or sets the classes for the increment/decrement stepper buttons.
    /// </summary>
    public required string StepButton { get; set; }

    /// <summary>
    /// Gets or sets the classes for the wrapper around each hour/minute number input.
    /// </summary>
    public required string NumberWrapper { get; set; }

    /// <summary>
    /// Gets or sets the base classes for the hour/minute number inputs, excluding hover/focus border and
    /// ring colors - those are resolved dynamically from the shared theme color tokens.
    /// </summary>
    public required string NumberInput { get; set; }

    /// <summary>
    /// Gets or sets the classes for the ":" separator between the hour and minute columns.
    /// </summary>
    public required string Separator { get; set; }

    /// <summary>
    /// Gets or sets the classes for the AM/PM toggle button's wrapper.
    /// </summary>
    public required string AmPmWrapper { get; set; }

    /// <summary>
    /// Gets or sets the typography classes applied to the AM/PM toggle <see cref="TwBlazor.Components.TwButton"/>.
    /// </summary>
    public required string AmPmButtonClass { get; set; }
}
