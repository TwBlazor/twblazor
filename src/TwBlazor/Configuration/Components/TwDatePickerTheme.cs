// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the date picker components (<see cref="TwBlazor.Components.TwDatePicker"/> and
/// <see cref="TwBlazor.Components.TwDateRangePicker"/>, which reuses the same panel/header/day-button
/// styling for visual consistency between the two). Override any property to customize date picker
/// styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwDatePickerTheme
{
    /// <summary>
    /// Gets or sets the classes for the header that displays navigation between days, months, and decades.
    /// </summary>
    public required string Header { get; set; }

    /// <summary>
    /// Gets or sets the classes for the header row that displays the weekdays (e.g. Mon, Tue, Wed).
    /// </summary>
    public required string WeekdaysHeader { get; set; }

    /// <summary>
    /// Gets or sets the base classes for the picker dialog.
    /// </summary>
    public required string Base { get; set; }

    /// <summary>
    /// Gets or sets the classes for active day, month, and year selection buttons.
    /// </summary>
    public required string ActiveClass { get; set; }

    /// <summary>
    /// Gets or sets the classes for the day, month, and year selection buttons.
    /// </summary>
    public required string ButtonClass { get; set; }

    /// <summary>
    /// Gets or sets the classes for days that fall strictly between a selected range's start and
    /// end (exclusive of the endpoints) in <see cref="TwBlazor.Components.TwDateRangePicker"/>.
    /// </summary>
    public required string RangeClass { get; set; }

    /// <summary>
    /// Gets or sets the classes for the per-calendar "Month Year" caption shown above each grid
    /// when <see cref="TwBlazor.Components.TwDateRangePicker"/> displays two months side by side.
    /// </summary>
    public required string RangeMonthCaptionClass { get; set; }

    /// <summary>
    /// Gets or sets the text color classes for a lead-in day from the previous month (the
    /// greyed-out, non-interactive cells shown before day 1). Not applied when the day falls
    /// within a selected range - see <see cref="RangeClass"/> - so it stays legible against that
    /// tint instead of fighting it with a second, muted color.
    /// </summary>
    public required string PrevMonthClass { get; set; }

    /// <summary>
    /// Gets or sets the default .NET custom date format string used by <see cref="TwBlazor.Components.TwDatePicker"/>
    /// and <see cref="TwBlazor.Components.TwDateRangePicker"/> when their own <c>Format</c> parameter
    /// isn't explicitly set. Change this to switch the whole app's default date format in one place
    /// instead of passing <c>Format</c> to every picker individually.
    /// </summary>
    public required string DefaultFormat { get; set; }

    /// <summary>
    /// Gets or sets the default .NET custom date/time format string (24-hour) used by
    /// <see cref="TwBlazor.Components.TwDateTimePicker"/> and <see cref="TwBlazor.Components.TwDateTimeRangePicker"/>
    /// when their own <c>Format</c> parameter isn't explicitly set and <c>Is12HourFormat</c> is
    /// <see langword="false"/>.
    /// </summary>
    public required string DefaultDateTimeFormat { get; set; }

    /// <summary>
    /// Gets or sets the default .NET custom date/time format string (12-hour, with an AM/PM
    /// designator) used by <see cref="TwBlazor.Components.TwDateTimePicker"/> and
    /// <see cref="TwBlazor.Components.TwDateTimeRangePicker"/> when their own <c>Format</c> parameter
    /// isn't explicitly set and <c>Is12HourFormat</c> is <see langword="true"/>.
    /// </summary>
    public required string DefaultDateTimeFormat12Hour { get; set; }

    /// <summary>
    /// Gets or sets the default separator used between the start and end date/time by
    /// <see cref="TwBlazor.Components.TwDateRangePicker"/> and <see cref="TwBlazor.Components.TwDateTimeRangePicker"/>
    /// when their own <c>RangeSeparator</c> parameter isn't explicitly set.
    /// </summary>
    public required string DefaultRangeSeparator { get; set; }
}
