// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
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

    private string datepickerContainerClasses => new ClassBuilder()
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
    /// Gets the CSS classes for the buttons present in the dialog.
    /// </summary>
    private string GetButtonClasses(string name, bool isSelected) =>
        new ClassBuilder($"{name} cursor-pointer")
        .AddClass(roundedBuilder.GetRounded())
        .AddClass(theme.ButtonClass)
        .AddClass(options.Theme.Colors.HoverColors.Primary)
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
        Value = SelectedDate.ToString(effectiveFormat, effectiveCulture);
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
    /// Advances the selected date by 10 years.
    /// </summary>
    private void NextDecade() => SelectedDate = SelectedDate.AddYears(10);

    /// <summary>
    /// Moves the selected date back by 10 years.
    /// </summary>
    private void PreviousDecade() => SelectedDate = SelectedDate.AddYears(-10);

    /// <summary>
    /// Advances the selected date by one year.
    /// </summary>
    private void NextYear() => SelectedDate = SelectedDate.AddYears(1);

    /// <summary>
    /// Moves the selected date back by one year.
    /// </summary>
    private void PreviousYear() => SelectedDate = SelectedDate.AddYears(-1);

    /// <summary>
    /// Advances the selected date by one month.
    /// </summary>
    private void NextMonth() => SelectedDate = SelectedDate.AddMonths(1);

    /// <summary>
    /// Moves the selected date back by one month.
    /// </summary>
    private void PreviousMonth() => SelectedDate = SelectedDate.AddMonths(-1);

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
    /// Selects a month and switches the view to the day view.
    /// </summary>
    /// <param name="selectedMonth">The month to select.</param>
    /// <remarks>
    /// Updates the <see cref="SelectedDate"/> to use the selected month while preserving the day, hour, minute, and second components.
    /// </remarks>
    private void SelectMonth(DateTime selectedMonth)
    {
        SelectedDate = new DateTime(SelectedDate.Year, selectedMonth.Month, SelectedDate.Day, SelectedDate.Hour, SelectedDate.Minute, SelectedDate.Second, SelectedDate.Kind);
        view = DatePickerView.Day;
        pendingViewFocus = true;
    }

    /// <summary>
    /// Selects a year and switches the view to the month view.
    /// </summary>
    /// <param name="selectedYear">The year to select.</param>
    /// <remarks>
    /// Updates the <see cref="SelectedDate"/> to use the selected year while preserving the month, day, hour, minute, and second components.
    /// </remarks>
    private void SelectYear(DateTime selectedYear)
    {
        SelectedDate = new DateTime(selectedYear.Year, SelectedDate.Month, SelectedDate.Day, SelectedDate.Hour, SelectedDate.Minute, SelectedDate.Second, SelectedDate.Kind);
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