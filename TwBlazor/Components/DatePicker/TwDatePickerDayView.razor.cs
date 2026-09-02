// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Globalization;
using TwBlazor.Configuration.Components;
using TwBlazor.Utilities;

namespace TwBlazor.Components.DatePicker;

public partial class TwDatePickerDayView : TwBlazorComponentBase, IAsyncDisposable
{
    [Inject] public IJSRuntime JSRuntime { get; set; } = default!;

    private TwDatePickerTheme theme => options.Theme.Components.Require<TwDatePickerTheme>();

    [Parameter] public DateTime Value { get; set; }
    [Parameter] public EventCallback<DateTime> ValueChanged { get; set; }

    private string dayHeaderClasses => new ClassBuilder(theme.WeekdaysHeader).Build();

    /// <summary>
    /// Localized weekday column headers (abbreviation shown, full name in the <c>&lt;abbr&gt;</c>'s
    /// <c>title</c>), Sunday-first to match <see cref="GetWeekRows"/>'s column order. Sourced from
    /// <see cref="CultureInfo.CurrentCulture"/> instead of hardcoded English strings so the calendar
    /// reads correctly for other locales.
    /// </summary>
    private static IReadOnlyList<(string Abbreviation, string FullName)> weekdayHeaders
    {
        get
        {
            var format = CultureInfo.CurrentCulture.DateTimeFormat;
            var headers = new (string, string)[7];

            for (var day = 0; day < 7; day++)
            {
                headers[day] = (format.ShortestDayNames[day], format.DayNames[day]);
            }

            return headers;
        }
    }

    /// <summary>
    /// Reference to the &lt;table role="grid"&gt; element, used to register a native keydown guard
    /// that suppresses the browser's default scroll behavior for the grid navigation keys (arrow
    /// keys, Home, End) - the same generic guard used by <see cref="TwTabContainer"/>'s tablist.
    /// </summary>
    private ElementReference gridRef;

    private bool keydownGuardRegistered;

    /// <summary>
    /// Holds a settable <see cref="ElementReference"/> for one day-of-month button, referenced by
    /// day number via <see cref="_dayCellRefs"/> so <see cref="OnGridKeyDown"/> can move focus there
    /// after the roving tabindex changes. A plain class (rather than a raw <see cref="ElementReference"/>
    /// field per entry) so <c>@ref</c> has a stable settable target to bind to from within the
    /// day-cell loop, mirroring the pattern <see cref="TwTabContainer"/> uses for its tab buttons.
    /// </summary>
    private sealed class DayCellRef
    {
        public ElementReference Element;
    }

    private readonly Dictionary<int, DayCellRef> _dayCellRefs = [];

    /// <summary>
    /// The day-of-month (1-based) currently holding the grid's roving tabindex - i.e. the one
    /// focusable cell reachable by Tab. Arrow keys move this within the currently displayed month;
    /// it does not itself change the selected date (that still only happens via click, Enter, or
    /// Space on the focused cell).
    /// </summary>
    private int focusedDay;

    private int trackedYear = int.MinValue;
    private int trackedMonth = int.MinValue;

    private DayCellRef GetDayCellRef(int day)
    {
        if (!_dayCellRefs.TryGetValue(day, out var cellRef))
        {
            cellRef = new DayCellRef();
            _dayCellRefs[day] = cellRef;
        }

        return cellRef;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // Only reset the roving tabindex when the displayed month actually changes (including on
        // first render) - not on every render, which would otherwise fight OnGridKeyDown's own
        // updates to focusedDay as the user arrows around within the same month.
        if (Value.Year != trackedYear || Value.Month != trackedMonth)
        {
            trackedYear = Value.Year;
            trackedMonth = Value.Month;
            focusedDay = Value.Day;
            _dayCellRefs.Clear();
        }
    }

    /// <summary>
    /// Registers the same generic keydown guard <see cref="TwTabContainer"/> uses for its tablist,
    /// so ArrowUp/ArrowDown/ArrowLeft/ArrowRight/Home/End don't also scroll the page while
    /// navigating the day grid, without ever touching Tab's default behavior.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("twTabs.registerKeydownGuard", gridRef);
                keydownGuardRegistered = true;
            }
            catch (JSDisconnectedException)
            {
                // The circuit disconnected before the script could run; nothing to register.
            }
        }
    }

    /// <summary>
    /// Implements the WAI-ARIA APG grid keyboard pattern for the day-of-month grid: ArrowRight/
    /// ArrowLeft move the roving tabindex by one day, ArrowDown/ArrowUp by one week, and Home/End
    /// jump to the first/last day of the current week row. Movement is clamped to the days in the
    /// currently displayed month - crossing into an adjacent month requires switching the calendar
    /// page first (via the header's Previous/Next controls), same as native OS date pickers.
    /// </summary>
    private async Task OnGridKeyDown(KeyboardEventArgs e)
    {
        var daysInMonth = DateTime.DaysInMonth(Value.Year, Value.Month);
        var dayOfWeek = (int)new DateTime(Value.Year, Value.Month, focusedDay, 0, 0, 0, Value.Kind).DayOfWeek;

        int? target = e.Key switch
        {
            "ArrowRight" => focusedDay + 1,
            "ArrowLeft" => focusedDay - 1,
            "ArrowDown" => focusedDay + 7,
            "ArrowUp" => focusedDay - 7,
            "Home" => focusedDay - dayOfWeek,
            "End" => focusedDay + (6 - dayOfWeek),
            _ => null
        };

        if (target is null)
        {
            return;
        }

        var clamped = Math.Clamp(target.Value, 1, daysInMonth);
        if (clamped == focusedDay)
        {
            return;
        }

        focusedDay = clamped;
        StateHasChanged();

        if (_dayCellRefs.TryGetValue(focusedDay, out var cellRef))
        {
            await cellRef.Element.FocusAsync();
        }
    }

    /// <summary>
    /// Unregisters the JS-side keydown guard registered for this grid.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (!keydownGuardRegistered)
        {
            GC.SuppressFinalize(this);
            return;
        }

        try
        {
            await JSRuntime.InvokeVoidAsync("twTabs.unregisterKeydownGuard", gridRef);
        }
        catch (JSDisconnectedException)
        {
            // The circuit is already gone; nothing left to clean up.
        }
        catch (InvalidOperationException)
        {
            // JS interop unavailable during teardown (e.g. prerendering); safe to ignore.
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Gets the CSS classes for the buttons present in the dialog.
    /// </summary>
    private string GetButtonClasses(bool isSelected, bool isToday) =>
        new ClassBuilder($"day cursor-pointer")
        .AddClass(roundedBuilder.GetRounded())
        .AddClass(theme.ButtonClass)
        .AddClass(options.Theme.Colors.HoverColors.Primary)
        .AddClass($"{theme.ActiveClass} {options.Theme.Colors.TextColors.Medium.Primary} {options.Theme.Colors.DarkTextColors.Light.Primary}", isToday && !isSelected)
        .AddClass($"{options.Theme.Colors.LightBackground.Light.Primary} {options.Theme.Colors.DarkBackground.Light.Primary} {options.Theme.Colors.TextColors.Medium.Primary} {options.Theme.Colors.DarkTextColors.Dark.Primary}", isSelected)
        .Build();

    /// <summary>
    /// Gets the accessible name for a day-of-month button, giving it full date context (e.g.
    /// "August 14, 2026") instead of just the bare day number, and prefixing "Selected" when it is
    /// the currently selected date.
    /// </summary>
    private static string GetDayAriaLabel(DateTime date, bool isSelected) =>
        isSelected ? $"Selected, {date:MMMM d, yyyy}" : date.ToString("MMMM d, yyyy");

    /// <summary>
    /// Splits the month's day cells (including leading days from the previous month) into rows of 7,
    /// so the calendar can be rendered as a real &lt;table&gt; with &lt;tr&gt; rows - giving assistive
    /// tech a structural association between each day cell and its weekday column header, instead of
    /// two visually-aligned but structurally unrelated CSS grids. The final row is padded with null
    /// cells (rendered as empty, aria-hidden placeholders) so every row has exactly 7 columns.
    /// </summary>
    private List<List<(int DayNumber, bool IsCurrentMonth)?>> GetWeekRows()
    {
        var daysInMonth = DateTime.DaysInMonth(Value.Year, Value.Month);
        var firstOfMonth = new DateTime(Value.Year, Value.Month, 1, 0, 0, 0, Value.Kind);
        // sunday = 0 - saturday = 6
        var offset = (int)firstOfMonth.DayOfWeek;

        var prevMonth = Value.AddMonths(-1);
        var prevDaysInMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
        var prevStart = prevDaysInMonth - offset + 1;

        List<(int DayNumber, bool IsCurrentMonth)?> cells = [];

        for (var i = 0; i < offset; i++)
        {
            cells.Add((prevStart + i, false));
        }

        for (var day = 1; day <= daysInMonth; day++)
        {
            cells.Add((day, true));
        }

        while (cells.Count % 7 != 0)
        {
            cells.Add(null);
        }

        List<List<(int DayNumber, bool IsCurrentMonth)?>> rows = [];
        for (var i = 0; i < cells.Count; i += 7)
        {
            rows.Add(cells.GetRange(i, 7));
        }

        return rows;
    }

    private async Task SelectDateAsync(DateTime dateTime)
    {
        Value = dateTime;

        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }
}
