// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;

namespace TwBlazor.Components;

/// <summary>
/// Represents a date and time picker component that allows users to select and edit date and time values, supporting
/// both 12-hour and 24-hour formats.
/// </summary>
/// <remarks>Use the TwDateTimePicker component to provide an interactive UI for selecting date and time values in
/// Blazor applications. The component supports two-way binding for both the selected DateTime value and its string
/// representation, and can be configured to display time in either 12-hour (AM/PM) or 24-hour format. The placeholder
/// text can be customized to guide users when no value is selected.</remarks>
public partial class TwDateTimePicker : TwBlazorTextInputComponentBase
{
    /// <summary>
    /// The string format used to display the <see cref="SelectedDateTime"/>, default value is 'dd/MM/yyyy HH:mm'.
    /// </summary>
    private string format { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the time should be displayed in 12-hour format.
    /// </summary>
    /// <remarks>Set this property to <see langword="true"/> to use 12-hour time representation (with AM/PM);
    /// otherwise, 24-hour format will be used.</remarks>
    [Parameter] public bool Is12HourFormat { get; set; }
    /// <summary>
    /// The selected <see cref="DateTime"/> value.
    /// </summary>
    [Parameter] public DateTime SelectedDateTime { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// The selected bound <see cref="DateTime"/> value.
    /// </summary>
    [Parameter] public EventCallback<DateTime> SelectedDateTimeChanged { get; set; }
    /// <summary>
    /// The <see cref="string" /> value displayed in the textbox to the user.
    /// </summary>
    [Parameter] public string? Value { get; set; }
    /// <summary>
    /// The <see cref="string" /> bound value displayed in the textbox to the user.
    /// </summary>
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    /// <summary>
    /// The placeholder text to display when no date is selected.
    /// </summary>
    [Parameter] public string Placeholder { get; set; } = "Select a datetime";

    /// <summary>
    /// Overrides automatic device detection for whether the browser's native datetime picker should be used
    /// instead of the custom popover. Leave unset (<see langword="null"/>) to auto-detect based on the
    /// client platform (iOS and Android use the native picker by default).
    /// </summary>
    [Parameter] public bool? PreferNativePicker { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TwDateTimePicker"/> class.
    /// </summary>
    public TwDateTimePicker()
    {
        format = "dd/MM/yyyy HH:mm";
    }

    protected override Task OnParametersSetAsync()
    {
        format = Is12HourFormat ? "dd/MM/yyyy hh:mm tt" : "dd/MM/yyyy HH:mm";
        return base.OnParametersSetAsync();
    }

    /// <summary>
    /// Gets the time portion of the SelectedDateTime as TimeOnly.
    /// </summary>
    private TimeOnly currentTime => TimeOnly.FromDateTime(SelectedDateTime);

    /// <summary>
    /// Handles time changes from the TwTimePickerBody component.
    /// </summary>
    private async Task OnTimePickerChanged(TimeOnly newTime)
    {
        // Combine the existing date with the new time
        var date = DateOnly.FromDateTime(SelectedDateTime);
        SelectedDateTime = DateTime.SpecifyKind(date.ToDateTime(newTime), SelectedDateTime.Kind);
        Value = SelectedDateTime.ToString(format);

        // Captured before either callback fires: a caller binding both SelectedDateTime and
        // Value can trigger a reentrant render from the first InvokeAsync that pushes its
        // (still stale) Value parameter back onto this component, clobbering the field before
        // it's read below - see the identical fix in TwDatePicker.SelectDateAsync.
        var selectedDateTime = SelectedDateTime;
        var value = Value;

        if (SelectedDateTimeChanged.HasDelegate)
            await SelectedDateTimeChanged.InvokeAsync(selectedDateTime);

        if (ValueChanged.HasDelegate)
            await ValueChanged.InvokeAsync(value);

        StateHasChanged();
    }
}