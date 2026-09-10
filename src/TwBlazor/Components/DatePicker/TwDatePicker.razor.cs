// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using TwBlazor.Configuration.Components;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a date picker component that allows users to select a date from a calendar interface or enter a date
/// manually.
/// </summary>
/// <remarks>The TwDatePicker component supports two-way binding for both the selected date and its string
/// representation. It provides customizable display formatting and placeholder text. The component is designed for use
/// in Blazor applications and can be integrated with other components, such as TwDateTimePicker, via the
/// FocusedChildContent parameter. Thread safety is not guaranteed; use the component only within the Blazor UI
/// thread.</remarks>
public partial class TwDatePicker : TwPopoverPickerComponentBase
{
    private TwDatePickerTheme theme => options.Theme.Components.Require<TwDatePickerTheme>();

    /// <summary>
    /// Reference to the trigger <see cref="TwTextfield{T}"/> instance, used to focus its actual
    /// &lt;input&gt; element directly - see <see cref="TwPopoverPickerComponentBase.triggerInputRef"/>.
    /// </summary>
    private TwTextfield<string>? trigger;

    /// <inheritdoc />
    protected override ElementReference? triggerInputRef => trigger?.InputRef;

    /// <summary>
    /// Gets or sets the underlying view used to display and interact with the date picker control.
    /// </summary>
    private DatePickerView view { get; set; }

    /// <summary>
    /// The month/year/decade currently displayed - a pure navigation position, independent of
    /// <see cref="SelectedDate"/>. Browsing with the header's Previous/Next controls (or drilling
    /// through the year/month quick-pick grids) only moves this; <see cref="SelectedDate"/> itself
    /// changes only when a day is actually picked (or a valid date is typed). Re-seeded to
    /// <see cref="SelectedDate"/> every time the panel opens (see the <see cref="OnFocusAsync"/>
    /// override) so browsing that was abandoned without picking anything doesn't linger into the
    /// next time the picker is opened.
    /// </summary>
    /// <remarks>
    /// Before this existed, navigation wrote directly into <see cref="SelectedDate"/> itself, which
    /// made the year/month quick-pick grids always show whatever page they'd been navigated to as
    /// "selected" - since that page's year/month was, by construction, always equal to the
    /// (silently drifted) <see cref="SelectedDate"/> being compared against.
    /// </remarks>
    private DateTime anchorDate;

    /// <summary>
    /// The placeholder text to display when no date is selected.
    /// </summary>
    /// <remarks>
    /// If not set, defaults to <see cref="Format"/> lower-cased (e.g. "dd/mm/yyyy") so the placeholder
    /// itself communicates the exact pattern typed input is parsed against, rather than a generic
    /// instruction that gives no clue what's actually expected.
    /// </remarks>
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>
    /// Gets the placeholder actually rendered: <see cref="Placeholder"/> when explicitly set, otherwise
    /// <see cref="Format"/> lower-cased.
    /// </summary>
    private string effectivePlaceholder => Placeholder ?? Format.ToLower(effectiveCulture);

    /// <summary>
    /// The <see cref="DateTime"/> value of the selected date.
    /// </summary>
    [Parameter] public DateTime SelectedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The <see cref="DateTime"/> bound value of the selected date.
    /// </summary>
    [Parameter] public EventCallback<DateTime> SelectedDateChanged { get; set; }

    /// <summary>
    /// The <see cref="string" /> value displayed in the textbox to the user.
    /// </summary>
    [Parameter] public string? Value { get; set; }

    /// <summary>
    /// The <see cref="string" /> bound value displayed in the textbox to the user.
    /// </summary>
    [Parameter] public EventCallback<string> ValueChanged { get; set; }

    /// <summary>
    /// The string format used to display the <see cref="SelectedDate"/>, default value is 'dd/MM/yyyy'.
    /// </summary>
    [Parameter] public string Format { get; set; } = "dd/MM/yyyy";

    /// <summary>
    /// The HTML input type to use when the native picker is active, default value is 'date'.
    /// Set to 'datetime-local' by <see cref="TwDateTimePicker"/> so it can reuse this component's
    /// native-picker support.
    /// </summary>
    [Parameter] public string NativeInputType { get; set; } = "date";

    /// <summary>
    /// The string format used for <see cref="Value"/> when the native picker is active, default value is
    /// 'yyyy-MM-dd'. This must match the ISO format expected by <see cref="NativeInputType"/>.
    /// </summary>
    [Parameter] public string NativeFormat { get; set; } = "yyyy-MM-dd";

    /// <summary>
    /// Gets the format currently used to parse and render <see cref="Value"/>, switching to
    /// <see cref="NativeFormat"/> when the native picker is active.
    /// </summary>
    private string effectiveFormat => UseNativePicker ? NativeFormat : Format;

    /// <summary>
    /// Gets the culture used to format/parse <see cref="Value"/>. The native browser date input
    /// always sends/receives its value in an invariant ISO format regardless of locale, so that
    /// path must stay culture-invariant; the custom popover instead formats using
    /// <see cref="CultureInfo.CurrentCulture"/> so a value the user sees rendered in their locale
    /// (e.g. a non-Gregorian digit set or different date ordering) also parses back correctly when
    /// they type it in - which requires parsing with that same culture rather than always
    /// <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    private CultureInfo effectiveCulture => UseNativePicker ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;

    /// <summary>
    /// Child content to be rendered when the date picker is focused, this is used for <see cref="TwDateTimePicker"/>.
    /// </summary>
    [Parameter] public RenderFragment? FocusedChildContent { get; set; }

    /// <summary>
    /// Set when the panel's view switches (year/month/day) while it's already open, so the next
    /// <see cref="OnAfterRenderAsync"/> reclaims focus inside the panel. Unlike <see cref="TwPopoverPickerComponentBase.PendingOpenFocus"/>,
    /// this one does move focus - the button that had it just got torn down by the view switch, so without
    /// this focus would otherwise fall back to the document body.
    /// </summary>
    private bool pendingViewFocus;

    // RootClass and Class are intentionally both applied here rather than split between TwInputRoot and the input.
    private string classes => new ClassBuilder("relative flex flex-col")
        .AddClass(RootClass)
        .AddClass(Class).Build();

    // Safari (iOS) renders native date/time/datetime-local inputs with a special control
    // path that can ignore percentage widths; appearance-none drops it into normal box-model
    // layout without affecting the native picker UI that opens on tap.
    private string textfieldClasses => new ClassBuilder("pl-10 pr-3")
        .AddClass("appearance-none", UseNativePicker).Build();

    // Caps the panel to the viewport height and lets it scroll vertically if it doesn't fit -
    // twPicker.positionPanel's flip logic picks the better of "below" or "above" the trigger, but
    // on a phone with the on-screen keyboard open there may not be enough room on either side, so
    // this is a safety net that keeps the whole panel reachable regardless (see the matching,
    // more detailed remarks on TwDateRangePicker.datepickerContainerClasses, where its much taller
    // two-month panel makes this matter more).
    private string datepickerContainerClasses => new ClassBuilder("max-h-[calc(100vh-2rem)] overflow-y-auto")
        .AddClass(shadowBuilder.GetShadow(effectiveShadow))
        .AddClass(roundedBuilder.GetRounded(effectiveRounded))
        .AddClass(theme.Base)
        .Build();

    /// <summary>
    /// Gets or sets the CSS class names to apply to the body element of the component.
    /// </summary>
    /// <remarks>Use this property to customize the styling of the component's body by specifying one or more
    /// CSS class names. Multiple classes can be separated by spaces. This allows dynamic styling based on the
    /// component's state or context.</remarks>
    [Parameter] public string BodyClasses { get; set; } = string.Empty;

    private string bodyClasses => new ClassBuilder()
        .AddClass("decade", view == DatePickerView.Year)
        .AddClass("months", view == DatePickerView.Month)
        .AddClass("days", view == DatePickerView.Day)
        .AddClass(BodyClasses).Build();

    /// <summary>
    /// Gets "today" for comparison against the year/month quick-pick grids, honoring
    /// <see cref="SelectedDate"/>'s <see cref="DateTime.Kind"/> the same way
    /// <see cref="DatePicker.TwDatePickerDayView"/> does for its own today indicator.
    /// </summary>
    private DateTime today => (SelectedDate.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now).Date;

    /// <summary>
    /// Gets the CSS classes for the buttons present in the dialog.
    /// </summary>
    /// <remarks>
    /// <paramref name="isToday"/> gives the year/month quick-pick grids the same distinct "this is
    /// the actual current year/month" indicator (<see cref="TwDatePickerTheme.ActiveClass"/>) the
    /// day grid already has for today's date - previously only the selected year/month was ever
    /// highlighted here, with nothing marking which one is actually current when browsing away
    /// from it.
    /// </remarks>
    private string GetButtonClasses(string name, bool isSelected, bool isToday) =>
        new ClassBuilder($"{name} cursor-pointer")
        .AddClass(roundedBuilder.GetRounded())
        .AddClass(theme.ButtonClass)
        .AddClass(options.Theme.Colors.HoverColors.Primary)
        .AddClass($"{theme.ActiveClass} {options.Theme.Colors.TextColors.Medium.Primary} {options.Theme.Colors.DarkTextColors.Light.Primary}", isToday && !isSelected)
        .AddClass(options.Theme.Colors.LightBackground.Light.Primary, isSelected)
        .AddClass(options.Theme.Colors.DarkBackground.Light.Primary, isSelected)
        .AddClass(options.Theme.Colors.TextColors.Medium.Primary, isSelected)
        .AddClass(options.Theme.Colors.DarkTextColors.Medium.Primary, isSelected)
        .Build();

    /// <summary>
    /// Initializes the component and sets the initial value from the selected date.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <see cref="Format"/> is null or empty.</exception>
    protected override void OnInitialized()
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);
        base.OnInitialized();
        anchorDate = SelectedDate;
        Value = SelectedDate.ToString(effectiveFormat, effectiveCulture);
    }

    /// <summary>
    /// Re-seeds <see cref="anchorDate"/> to the actual <see cref="SelectedDate"/> every time the
    /// panel opens, so navigation abandoned without picking anything (browsing to another
    /// month/year, then clicking away) doesn't linger the next time the picker is opened - it
    /// always starts back at the real selection's own month.
    /// </summary>
    protected override async Task OnFocusAsync()
    {
        anchorDate = SelectedDate;
        await base.OnFocusAsync();
    }

    /// <summary>
    /// Determines, via <see cref="TwPopoverPickerComponentBase.PreferNativePicker"/> or JS-based device
    /// detection, whether the browser's native date input should be used instead of the custom popover, then re-renders if that changes the
    /// input's format/type.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            UseNativePicker = PreferNativePicker ?? await DeviceDetector.PrefersNativePickerAsync(JSRuntime);
            if (UseNativePicker)
            {
                Value = SelectedDate.ToString(NativeFormat, effectiveCulture);
                StateHasChanged();
            }
        }

        if (isFocused && PanelRef.Context != null)
        {
            // Arm the Tab focus trap and background inert-ing once, when the panel first mounts.
            // Deliberately does not move focus into the panel - see PendingOpenFocus's remarks.
            if (PendingOpenFocus)
            {
                PendingOpenFocus = false;
                await JSRuntime.InvokeVoidAsync("twPicker.positionPanel", PanelRef);
                await JSRuntime.InvokeVoidAsync("twDialog.trapFocus", PanelRef);
                await JSRuntime.InvokeVoidAsync("twDialog.setBackgroundInert", InputRoot?.RootRef);
            }

            // Reclaim focus inside the panel after a view switch, since the button that had it was
            // just replaced by the new view's grid - see pendingViewFocus's remarks.
            if (pendingViewFocus)
            {
                pendingViewFocus = false;
                await JSRuntime.InvokeVoidAsync("twDialog.focusSurface", PanelRef);
            }
        }
    }

    /// <summary>
    /// Handles text input changes and attempts to parse the date using the specified format.
    /// </summary>
    /// <param name="date">The date string entered by the user.</param>
    /// <remarks>
    /// If the date cannot be parsed according to the <see cref="Format"/>, the input is left as-is and
    /// <see cref="TwBlazorInputComponentBase.Invalid"/>/<see cref="TwBlazorInputComponentBase.ErrorMessage"/>
    /// are set so the field renders an accessible (<c>aria-invalid</c>/<c>role="alert"</c>) error instead of
    /// silently discarding what the user typed and substituting today's date.
    /// The date picker dialog is closed after processing the input.
    /// If the component is readonly or disabled, no action is taken.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnTextValueChanged(string? date)
    {
        if (ReadOnly || Disabled)
            return;

        if (isFocused)
        {
            await ReleasePanelTrapAsync();
        }
        isFocused = false;

        // This handler fires on blur (TwTextfield's BindEvent defaults to "onchange"), meaning the
        // browser has already moved focus away from the trigger - by the user tabbing to the next
        // field, for instance. Forcing focus back here (as every other close path correctly does)
        // would fight that: it'd not only reopen the panel (guarded against separately by
        // suppressNextFocusOpen) but also yank focus away from wherever the user just tabbed to.
        // So this path only clears the captured token and otherwise leaves focus alone.
        FocusReturnToken = null;

        if (string.IsNullOrWhiteSpace(date))
        {
            return;
        }

        // Native date/datetime-local inputs are supposed to always send a strict ISO value on
        // change, but some WebKit (Safari/iOS) versions append a ":00" seconds component that a
        // few browsers omit - so an exact match against effectiveFormat (which has no seconds
        // placeholder) fails even though the value is a perfectly valid ISO date/time. The custom
        // popover's typed-text path still needs the strict, exact-format check (that's what makes
        // the placeholder's format promise meaningful), but the native path can safely fall back to
        // a lenient parse since that string was never typed by the user in the first place - it's
        // whatever the browser's own picker produced.
        var success = DateTime.TryParseExact(
            date.Trim(),
            effectiveFormat,
            effectiveCulture,
            DateTimeStyles.None,
            out var parsedDate);

        if (!success && UseNativePicker)
        {
            success = DateTime.TryParse(date.Trim(), effectiveCulture, DateTimeStyles.None, out parsedDate);
        }

        if (!success)
        {
            Invalid = true;
            ErrorMessage = "Enter a valid date";
            Value = date;
            return;
        }

        Invalid = false;
        ErrorMessage = string.Empty;
        await SelectDateAsync(DateTime.SpecifyKind(parsedDate, SelectedDate.Kind), restoreFocusToTrigger: false);
    }

    /// <summary>
    /// Advances the displayed decade by 10 years, without changing <see cref="SelectedDate"/>.
    /// </summary>
    private void NextDecade() => anchorDate = anchorDate.AddYears(10);

    /// <summary>
    /// Moves the displayed decade back by 10 years, without changing <see cref="SelectedDate"/>.
    /// </summary>
    private void PreviousDecade() => anchorDate = anchorDate.AddYears(-10);

    /// <summary>
    /// Advances the displayed year by one, without changing <see cref="SelectedDate"/>.
    /// </summary>
    private void NextYear() => anchorDate = anchorDate.AddYears(1);

    /// <summary>
    /// Moves the displayed year back by one, without changing <see cref="SelectedDate"/>.
    /// </summary>
    private void PreviousYear() => anchorDate = anchorDate.AddYears(-1);

    /// <summary>
    /// Advances the displayed month by one, without changing <see cref="SelectedDate"/>.
    /// </summary>
    private void NextMonth() => anchorDate = anchorDate.AddMonths(1);

    /// <summary>
    /// Moves the displayed month back by one, without changing <see cref="SelectedDate"/>.
    /// </summary>
    private void PreviousMonth() => anchorDate = anchorDate.AddMonths(-1);

    /// <summary>
    /// Selects a date and updates both the selected date and its string representation.
    /// </summary>
    /// <param name="dateTime">The date to select.</param>
    /// <remarks>
    /// This method closes the date picker dialog, updates the <see cref="SelectedDate"/> and <see cref="Value"/> properties,
    /// and invokes the <see cref="SelectedDateChanged"/> and <see cref="ValueChanged"/> callbacks if they have delegates.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private Task SelectDateAsync(DateTime dateTime) => SelectDateAsync(dateTime, restoreFocusToTrigger: true);

    /// <param name="dateTime">The date to select.</param>
    /// <param name="restoreFocusToTrigger">
    /// Whether to force focus back onto the trigger input after closing. True for panel-driven
    /// selections (day/month/year button clicks), where focus is still inside the panel and needs to
    /// be reclaimed. False when called from a blur-driven path (typing a valid date then tabbing
    /// away), where focus has already moved on its own and forcing it back would fight the user.
    /// </param>
    private async Task SelectDateAsync(DateTime dateTime, bool restoreFocusToTrigger)
    {
        if (isFocused)
        {
            await ReleasePanelTrapAsync();
        }
        isFocused = false;
        SelectedDate = dateTime;
        anchorDate = dateTime;
        Value = SelectedDate.ToString(effectiveFormat, effectiveCulture);
        Invalid = false;
        ErrorMessage = string.Empty;

        // Captured before either callback fires: TwDateTimePicker binds both SelectedDate and
        // Value to this same instance, so invoking SelectedDateChanged can trigger a reentrant
        // render that pushes its (still stale) Value parameter back down onto this component,
        // clobbering the field before ValueChanged would otherwise read it.
        var selectedDate = SelectedDate;
        var value = Value;

        if (restoreFocusToTrigger)
        {
            await RestoreFocusAsync();
        }
        else
        {
            FocusReturnToken = null;
        }

        if (SelectedDateChanged.HasDelegate)
        {
            await SelectedDateChanged.InvokeAsync(selectedDate);
        }

        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    /// Navigates the displayed month to <paramref name="selectedMonth"/> and switches to the day
    /// view - this is further browsing, not a final selection, so it moves <see cref="anchorDate"/>
    /// rather than <see cref="SelectedDate"/> (which only changes once an actual day is picked).
    /// </summary>
    /// <param name="selectedMonth">The month to display.</param>
    private void SelectMonth(DateTime selectedMonth)
    {
        anchorDate = new DateTime(anchorDate.Year, selectedMonth.Month, anchorDate.Day, anchorDate.Hour, anchorDate.Minute, anchorDate.Second, anchorDate.Kind);
        view = DatePickerView.Day;
        pendingViewFocus = true;
    }

    /// <summary>
    /// Navigates the displayed year to <paramref name="selectedYear"/> and switches to the month
    /// view - see <see cref="SelectMonth"/>'s remarks for why this moves <see cref="anchorDate"/>
    /// rather than <see cref="SelectedDate"/>.
    /// </summary>
    /// <param name="selectedYear">The year to display.</param>
    private void SelectYear(DateTime selectedYear)
    {
        anchorDate = new DateTime(selectedYear.Year, anchorDate.Month, anchorDate.Day, anchorDate.Hour, anchorDate.Minute, anchorDate.Second, anchorDate.Kind);
        view = DatePickerView.Month;
        pendingViewFocus = true;
    }

    /// <summary>
    /// Switches the date picker to the specified view.
    /// </summary>
    /// <param name="datePickerView">The view to switch to.</param>
    private void SwitchView(DatePickerView datePickerView)
    {
        view = datePickerView;
        pendingViewFocus = true;
    }

    /// <summary>
    /// Defines the available views for the date picker component.
    /// </summary>
    private enum DatePickerView
    {
        /// <summary>
        /// Day selection view showing a calendar grid of days in a month.
        /// </summary>
        Day,

        /// <summary>
        /// Month selection view showing a grid of months in a year.
        /// </summary>
        Month,

        /// <summary>
        /// Year selection view showing a grid of years in a decade.
        /// </summary>
        Year
    }
}