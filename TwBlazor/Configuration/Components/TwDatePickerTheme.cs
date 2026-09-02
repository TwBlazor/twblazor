// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the date picker component (<see cref="TwBlazor.Components.TwDatePicker"/>).
/// Override any property to customize date picker styles globally.
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
}
