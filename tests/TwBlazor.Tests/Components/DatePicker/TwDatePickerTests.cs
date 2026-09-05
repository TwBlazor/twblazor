using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;
using TwBlazor.Builders;
using TwBlazor.Components;
using TwBlazor.Configuration.Components;

namespace TwBlazor.Tests.Components.DatePicker;

public class TwDatePickerTests : TwBlazorTestBase
{
    private TwInputTheme inputTheme => Theme.Components.Require<TwInputTheme>();

    public TwDatePickerTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
        TestContext.JSInterop.SetupVoid("twPicker.registerOutsideClick");
        TestContext.JSInterop.SetupVoid("twPicker.unregisterOutsideClick");
    }

    [Fact]
    public void ShouldRender_Input_WithPlaceholderAndIcon()
    {
        // Arrange
        var placeholder = "Pick a day";

        // Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.Placeholder, placeholder)
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        var input = cut.Find("input");

        // Assert
        Assert.NotNull(input);
        Assert.Equal(placeholder, input.GetAttribute("placeholder"));
        Assert.NotNull(cut.Find("svg"));
    }

    [Fact]
    public void FocusingInput_ShowsDatePicker()
    {
        // Arrange
        var date = new DateTime(2025, 11, 1);


        // Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, date)
        );

        cut.Find("input").Focus();

        // Assert
        Assert.Contains("datepicker-grid", cut.Markup);
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twPicker.registerOutsideClick");
    }

    [Fact]
    public void ClickingDay_SelectsDate_And_InvokesCallbacks()
    {
        // Arrange
        DateTime? selectedFromCallback = null;
        string? valueFromCallback = null;

        var selectedCallback = EventCallback.Factory.Create(this, (DateTime d) => selectedFromCallback = d);
        var valueCallback = EventCallback.Factory.Create(this, (string s) => valueFromCallback = s);

        var startDate = new DateTime(2025, 11, 1);

        // Act & Assert
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, startDate)
            .Add(x => x.SelectedDateChanged, selectedCallback)
            .Add(x => x.ValueChanged, valueCallback)
        );

        cut.Find("input").Focus();

        var dayButton = cut.FindAll("button.day").FirstOrDefault(b => b.TextContent.Trim() == "15");

        Assert.NotNull(dayButton);

        dayButton!.Click();

        Assert.Equal(new DateTime(2025, 11, 15), selectedFromCallback);
        Assert.Equal("15/11/2025", valueFromCallback); // Format default: dd/MM/yyyy
    }

    [Fact]
    public void TypingValidDate_InInput_ParsesAndSelectsDate()
    {
        // Arrange
        DateTime? selectedFromCallback = null;
        string? valueFromCallback = null;

        var selectedCallback = EventCallback.Factory.Create(this, (DateTime d) => selectedFromCallback = d);
        var valueCallback = EventCallback.Factory.Create(this, (string s) => valueFromCallback = s);

        // Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDateChanged, selectedCallback)
            .Add(x => x.ValueChanged, valueCallback)
        );

        cut.Find("input").Change("18/11/2025");

        // Assert
        Assert.NotNull(selectedFromCallback);
        Assert.Equal(new DateTime(2025, 11, 18).Date, selectedFromCallback!.Value.Date);
        Assert.Equal("18/11/2025", valueFromCallback); // dd/MM/yyyy
    }

    [Fact]
    public void TypingInvalidDate_InInput_DoesNotSilentlySelectToday_AndShowsError()
    {
        // Arrange
        DateTime? selectedFromCallback = null;
        string? valueFromCallback = null;

        var selectedCallback = EventCallback.Factory.Create(this, (DateTime d) => selectedFromCallback = d);
        var valueCallback = EventCallback.Factory.Create(this, (string s) => valueFromCallback = s);

        // Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDateChanged, selectedCallback)
            .Add(x => x.ValueChanged, valueCallback)
        );

        cut.Find("input").Change("not-a-date");

        // Assert: an unparsable date must not be silently swapped for today - neither callback fires,
        // and the field surfaces an accessible (role="alert") error instead.
        Assert.Null(selectedFromCallback);
        Assert.Null(valueFromCallback);

        var input = cut.Find("input");
        Assert.Equal("true", input.GetAttribute("aria-invalid"));

        var error = cut.Find("[role='alert']");
        Assert.Equal("Enter a valid date", error.TextContent);
    }

    [Fact]
    public void SwitchingViews_Month_Then_Day_RendersExpectedSections()
    {
        // Arrange
        var start = new DateTime(2025, 11, 3);
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, start)
        );

        cut.Find("input").Focus();
        cut.Find("button.view-switch").Click(); // Day -> Month


        // Act & Assert
        Assert.Contains("months-of-the-year", cut.Markup);

        var monthButton = cut.FindAll("button.month").First(b => b.TextContent.Trim() == "Jan");
        monthButton.Click();

        Assert.Contains("datepicker-grid", cut.Markup);
    }

    [Fact]
    public void SwitchingToYearView_ShowsDecadeGrid_WithTenYears()
    {
        // Arrange
        var start = new DateTime(2025, 11, 1);
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, start)
        );

        // Act & Assert
        cut.Find("input").Focus();
        cut.Find("button.view-switch").Click(); // Day -> Month
        cut.Find("button.view-switch").Click(); // Month -> Year

        Assert.Contains("years-of-the-decade", cut.Markup);
        var yearButtons = cut.FindAll("button.year");
        Assert.Equal(10, yearButtons.Count);
        Assert.Equal("2025", yearButtons[0].TextContent.Trim());
    }

    [Fact]
    public void NavigationButtons_WorkAcrossMonthYearDecade()
    {
        // Arrange
        var start = new DateTime(2025, 11, 1);
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, start)
        );

        // Act & Assert
        cut.Find("input").Focus();

        // Next month
        cut.Find("button.next-btn").Click();
        Assert.Contains(start.AddMonths(1).ToString("MMMM yyyy"), cut.Markup);

        // Previous twice
        cut.Find("button.prev-btn").Click();
        cut.Find("button.prev-btn").Click();
        Assert.Contains(start.AddMonths(-1).ToString("MMMM yyyy"), cut.Markup);

        // Month -> Year
        cut.Find("button.view-switch").Click(); // Day->Month
        cut.Find("button.view-switch").Click(); // Month->Year
        Assert.Contains("years-of-the-decade", cut.Markup);

        cut.Find("button.next-btn").Click();
        Assert.Contains((start.Year + 10).ToString(), cut.Markup);
    }

    [Fact]
    public async Task Close_UnregistersJS_And_HidesPopup()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        // Act & Assert
        cut.Find("input").Focus();
        Assert.Contains("datepicker-grid", cut.Markup);

        await cut.Instance.Close();

        Assert.DoesNotContain("datepicker-grid", cut.Markup);
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twPicker.unregisterOutsideClick");
    }

    [Fact]
    public void DefaultVariant_RendersCorrectClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.Variant, TwBlazor.Enums.InputVariant.Default)
        );

        var input = cut.Find("input");

        // Assert
        Assert.NotNull(input);
        Assert.Contains("border-b", input.GetAttribute("class"));
    }

    [Fact]
    public void OutlinedVariant_RendersCorrectClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.Variant, TwBlazor.Enums.InputVariant.Outlined)
        );

        var input = cut.Find("input");

        // Assert
        Assert.NotNull(input);
        Assert.Contains("border", input.GetAttribute("class"));
        Assert.Contains("rounded", input.GetAttribute("class"));
    }

    [Fact]
    public void FilledVariant_RendersCorrectClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.Variant, TwBlazor.Enums.InputVariant.Filled)
        );

        var input = cut.Find("input");

        // Assert
        Assert.NotNull(input);
        Assert.Contains("bg-gray-100", input.GetAttribute("class"));
    }

    [Fact]
    public void UsesGlobalDefaultVariant_WhenNotSet()
    {
        // Arrange - no Variant set on the component, so it must follow TwInputTheme.DefaultInputVariant
        // (inherited via TwBlazorInputComponentBase.effectiveVariant), even after the theme changes.
        inputTheme.DefaultInputVariant = TwBlazor.Enums.InputVariant.Outlined;

        // Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        // Assert
        var input = cut.Find("input");
        Assert.Contains(InputVariantBuilder.GetClasses(TwBlazor.Enums.InputVariant.Outlined, inputTheme), input.GetAttribute("class"));
    }

    [Fact]
    public async Task DisposeAsync_UnregistersOutsideClick()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        cut.Find("input").Focus(); // registers outside click handler

        // Act
        await cut.Instance.DisposeAsync();

        // Assert
        Assert.Contains(TestContext.JSInterop.Invocations,
            i => i.Identifier == "twPicker.unregisterOutsideClick");
    }

    [Fact]
    public void ReadOnly_FocusingInput_DoesNotShowDatePicker()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.ReadOnly, true)
        );

        // Act
        cut.Find("input").Focus();

        // Assert
        Assert.DoesNotContain("datepicker-grid", cut.Markup);
    }

    [Fact]
    public void Disabled_FocusingInput_DoesNotShowDatePicker()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.Disabled, true)
        );

        // Act
        cut.Find("input").Focus();

        // Assert
        Assert.DoesNotContain("datepicker-grid", cut.Markup);
    }

    [Fact]
    public void ReadOnly_TypingDate_DoesNotUpdateSelectedDate()
    {
        // Arrange
        DateTime? selectedFromCallback = null;
        var selectedCallback = EventCallback.Factory.Create(this, (DateTime d) => selectedFromCallback = d);

        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.SelectedDateChanged, selectedCallback)
            .Add(x => x.ReadOnly, true)
        );

        // Act
        cut.Find("input").Change("18/11/2025");

        // Assert
        Assert.Null(selectedFromCallback);
    }

    [Fact]
    public void TypingEmptyDate_InInput_DoesNotUpdateSelectedDate()
    {
        // Arrange
        DateTime? selectedFromCallback = null;
        var selectedCallback = EventCallback.Factory.Create(this, (DateTime d) => selectedFromCallback = d);

        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.SelectedDateChanged, selectedCallback)
        );

        // Act
        cut.Find("input").Change(string.Empty);

        // Assert
        Assert.Null(selectedFromCallback);
    }

    [Fact]
    public void Disabled_TypingDate_DoesNotUpdateSelectedDate()
    {
        // Arrange
        DateTime? selectedFromCallback = null;
        var selectedCallback = EventCallback.Factory.Create(this, (DateTime d) => selectedFromCallback = d);

        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.SelectedDateChanged, selectedCallback)
            .Add(x => x.Disabled, true)
        );

        // Act
        cut.Find("input").Change("18/11/2025");

        // Assert
        Assert.Null(selectedFromCallback);
    }

    [Fact]
    public void PreferNativePickerTrue_RendersNativeDateInput_WithIsoValue()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 18))
            .Add(x => x.PreferNativePicker, true)
        );

        var input = cut.Find("input");

        // Assert
        Assert.Equal("date", input.GetAttribute("type"));
        Assert.Equal("2025-11-18", input.GetAttribute("value"));
    }

    [Fact]
    public void PreferNativePickerTrue_FocusingInput_DoesNotShowCustomPopup()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.PreferNativePicker, true)
        );

        // Act
        cut.Find("input").Focus();

        // Assert
        Assert.DoesNotContain("datepicker-grid", cut.Markup);
        Assert.DoesNotContain(TestContext.JSInterop.Invocations, i => i.Identifier == "twPicker.registerOutsideClick");
    }

    [Fact]
    public void PreferNativePickerTrue_ChangingNativeInput_ParsesIsoValue_AndInvokesCallbacks()
    {
        // Arrange
        DateTime? selectedFromCallback = null;
        string? valueFromCallback = null;

        var selectedCallback = EventCallback.Factory.Create(this, (DateTime d) => selectedFromCallback = d);
        var valueCallback = EventCallback.Factory.Create(this, (string s) => valueFromCallback = s);

        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.PreferNativePicker, true)
            .Add(x => x.SelectedDateChanged, selectedCallback)
            .Add(x => x.ValueChanged, valueCallback)
        );

        // Act — native <input type="date"> always sends an ISO "yyyy-MM-dd" value on change.
        cut.Find("input").Change("2025-12-25");

        // Assert
        Assert.Equal(new DateTime(2025, 12, 25), selectedFromCallback);
        Assert.Equal("2025-12-25", valueFromCallback);
    }

    [Fact]
    public void PreferNativePickerTrue_ChangingNativeDateTimeInput_WithSecondsSuffix_StillParses()
    {
        // Arrange - some WebKit (Safari/iOS) versions append a ":00" seconds component to the
        // datetime-local value on change even though no `step` attribute requests it, which doesn't
        // exactly match NativeFormat's "yyyy-MM-ddTHH:mm" (issue #158). The native path should
        // tolerate this instead of surfacing "Enter a valid date" for a value the browser itself sent.
        DateTime? selectedFromCallback = null;
        string? valueFromCallback = null;
        string? errorMessage = null;

        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1, 14, 30, 0))
            .Add(x => x.PreferNativePicker, true)
            .Add(x => x.NativeInputType, "datetime-local")
            .Add(x => x.NativeFormat, "yyyy-MM-ddTHH:mm")
            .Add(x => x.SelectedDateChanged, EventCallback.Factory.Create<DateTime>(this, d => selectedFromCallback = d))
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<string>(this, s => valueFromCallback = s))
        );

        // Act
        cut.Find("input").Change("2025-12-25T13:45:00");
        errorMessage = cut.Instance.ErrorMessage;

        // Assert
        Assert.False(cut.Instance.Invalid);
        Assert.True(string.IsNullOrEmpty(errorMessage));
        Assert.Equal(new DateTime(2025, 12, 25, 13, 45, 0), selectedFromCallback);
        Assert.NotNull(valueFromCallback);
    }

    [Fact]
    public void PreferNativePickerFalse_TypedText_WithExtraSeconds_StaysInvalid()
    {
        // Arrange - the leniency added for native-picker values must not weaken validation of text
        // the user actually typed into the custom popover's trigger; that still has to match Format
        // exactly, so an unexpected trailing seconds component is correctly rejected here.
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.PreferNativePicker, false)
        );

        var input = cut.Find("input[type='text']");

        // Act
        input.Change("01/11/2025 14:30:00");

        // Assert
        Assert.True(cut.Instance.Invalid);
        Assert.Equal("Enter a valid date", cut.Instance.ErrorMessage);
    }

    [Fact]
    public void PreferNativePickerFalse_RendersTextInput_AndOpensCustomPopupOnFocus()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.PreferNativePicker, false)
        );

        var input = cut.Find("input");
        Assert.Equal("text", input.GetAttribute("type"));

        // Act
        input.Focus();

        // Assert
        Assert.Contains("datepicker-grid", cut.Markup);
    }

    [Fact]
    public void NativePickerNotSpecified_DetectsViaJsInterop_AndSwitchesToDateInput()
    {
        // Arrange
        TestContext.JSInterop.Setup<bool>("twDevice.prefersNativePicker").SetResult(true);

        // Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 18))
        );

        // Assert
        var input = cut.Find("input");
        Assert.Equal("date", input.GetAttribute("type"));
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twDevice.prefersNativePicker");
    }

    [Fact]
    public void NativePickerNotSpecified_JsInteropReturnsFalse_KeepsCustomPopup()
    {
        // Arrange
        TestContext.JSInterop.Setup<bool>("twDevice.prefersNativePicker").SetResult(false);

        // Act
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        var input = cut.Find("input");
        Assert.Equal("text", input.GetAttribute("type"));

        input.Focus();

        // Assert
        Assert.Contains("datepicker-grid", cut.Markup);
    }

    [Fact]
    public void PreferNativePickerTrue_CustomNativeInputTypeAndFormat_AreRespected()
    {
        // Arrange & Act — this is how TwDateTimePicker configures the inner TwDatePicker for datetime-local.
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 18, 14, 30, 0))
            .Add(x => x.PreferNativePicker, true)
            .Add(x => x.NativeInputType, "datetime-local")
            .Add(x => x.NativeFormat, "yyyy-MM-ddTHH:mm")
        );

        var input = cut.Find("input");

        // Assert
        Assert.Equal("datetime-local", input.GetAttribute("type"));
        Assert.Equal("2025-11-18T14:30", input.GetAttribute("value"));
    }

    [Fact]
    public void FocusedChildContent_WhenSet_IsRenderedInsideOpenPanel()
    {
        // Arrange — TwDateTimePicker composes TwDatePicker with a time picker via this render
        // fragment; the .razor's "@if (FocusedChildContent is not null)" branch is otherwise never
        // exercised because no existing test supplies it.
        const string marker = "focused-child-marker";
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.FocusedChildContent, RenderFragmentBuilder(marker))
        );

        // Act
        cut.Find("input").Focus();

        // Assert
        Assert.Contains("datepicker-grid", cut.Markup);
        Assert.Contains(marker, cut.Markup);
    }

    [Fact]
    public void ClickingCalendarIcon_WhenEnabled_FocusesTriggerViaJsInterop()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        // Act
        cut.Find("[aria-label='Open date picker']").Click();

        // Assert
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.focusSurface");
    }

    [Fact]
    public void ClickingCalendarIcon_FocusesTriggerInput_NotTheRootOrTheIconItself()
    {
        // Arrange - regression test (reported: the calendar icon didn't open the panel on click).
        // twDialog.focusSurface used to be called with the whole InputRoot, but the icon is rendered
        // ahead of the trigger <input> in DOM order and (role="button" tabindex="0") is itself
        // focusable - so scanning the root for the first focusable descendant found the icon that was
        // just clicked and refocused it, a no-op that never fired the input's focus event, so the
        // panel never opened. The JS call must target the trigger's actual <input> element instead.
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        // Act
        cut.Find("[aria-label='Open date picker']").Click();

        // Assert
        var invocation = TestContext.JSInterop.Invocations.Single(i => i.Identifier == "twDialog.focusSurface");
        var passedRef = Assert.IsType<ElementReference>(invocation.Arguments[0]);

        var baseType = typeof(TwPopoverPickerComponentBase);
        var triggerInputRefProperty = baseType.GetProperty("triggerInputRef", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var inputRootField = baseType.GetField("InputRoot", BindingFlags.NonPublic | BindingFlags.Instance)!;

        var expectedRef = (ElementReference?)triggerInputRefProperty.GetValue(cut.Instance);
        var rootRef = ((TwInputRoot?)inputRootField.GetValue(cut.Instance))?.RootRef;

        Assert.NotNull(expectedRef);
        Assert.Equal(expectedRef!.Value.Id, passedRef.Id);
        Assert.NotEqual(rootRef?.Id, passedRef.Id);
    }

    [Fact]
    public void ClickingCalendarIcon_WhenDisabled_DoesNotInvokeJsInterop()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
            .Add(x => x.Disabled, true)
        );

        // Act
        cut.Find("[aria-label='Open date picker']").Click();

        // Assert
        Assert.DoesNotContain(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.focusSurface");
    }

    [Theory]
    [InlineData("Enter")]
    [InlineData(" ")]
    public void CalendarIconKeyDown_EnterOrSpace_FocusesTriggerViaJsInterop(string key)
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        // Act
        cut.Find("[aria-label='Open date picker']").KeyDown(new KeyboardEventArgs { Key = key });

        // Assert
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.focusSurface");
    }

    [Fact]
    public void CalendarIconKeyDown_OtherKey_DoesNotInvokeJsInterop()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        // Act
        cut.Find("[aria-label='Open date picker']").KeyDown(new KeyboardEventArgs { Key = "Tab" });

        // Assert
        Assert.DoesNotContain(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.focusSurface");
    }

    [Fact]
    public void PanelKeyDown_Escape_ClosesPanelAndReleasesFocusTrap()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );
        cut.Find("input").Focus();
        Assert.Contains("datepicker-grid", cut.Markup);

        // Act
        cut.Find("[role='dialog']").KeyDown(new KeyboardEventArgs { Key = "Escape" });

        // Assert
        Assert.DoesNotContain("datepicker-grid", cut.Markup);
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.releaseFocusTrap");
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twPicker.unregisterOutsideClick");
    }

    [Fact]
    public void PanelKeyDown_NonEscapeKey_KeepsPanelOpen()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );
        cut.Find("input").Focus();

        // Act
        cut.Find("[role='dialog']").KeyDown(new KeyboardEventArgs { Key = "a" });

        // Assert
        Assert.Contains("datepicker-grid", cut.Markup);
    }

    [Fact]
    public void SelectingDay_WithCapturedFocusToken_RestoresFocusViaJsInterop()
    {
        // Arrange — simulate the trigger having genuinely had focus captured before the panel
        // opened, so SelectDateAsync's RestoreFocusAsync call actually reaches the JS restore-focus
        // path instead of no-oping on a null token (the default in every other test here).
        TestContext.JSInterop.Setup<string?>("twDialog.captureFocus").SetResult("captured-token");

        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );
        cut.Find("input").Focus();

        // Act
        var dayButton = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15");
        dayButton.Click();

        // Assert
        var invocation = TestContext.JSInterop.Invocations.Single(i => i.Identifier == "twDialog.restoreFocus");
        Assert.Equal("captured-token", invocation.Arguments[0]);
    }

    [Fact]
    public void TypingValidDate_WhilePanelIsOpen_ReleasesFocusTrapBeforeClosing()
    {
        // Arrange — every other typed-date test types without ever opening the panel first, so
        // OnTextValueChanged's "if (isFocused) await ReleasePanelTrapAsync()" branch is otherwise
        // never taken.
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );
        cut.Find("input").Focus();
        Assert.Contains("datepicker-grid", cut.Markup);

        // Act
        cut.Find("input").Change("18/11/2025");

        // Assert
        Assert.DoesNotContain("datepicker-grid", cut.Markup);
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.releaseFocusTrap");
    }

    [Fact]
    public void EmptyFormat_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            TestContext.Render<TwDatePicker>(p => p
                .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
                .Add(x => x.Format, string.Empty)));
    }

    [Fact]
    public async Task Close_WhenPanelNeverOpened_SkipsUnregisterAndDoesNotThrow()
    {
        // Arrange — Close() is reachable even if the panel was never focused open (e.g. called
        // defensively by a composing component); registeredOutsideHandler is still false here, so
        // UnregisterOutsideClickAsync's guard clause should short-circuit rather than call JS.
        var cut = TestContext.Render<TwDatePicker>(p => p
            .Add(x => x.SelectedDate, new DateTime(2025, 11, 1))
        );

        // Act
        await cut.Instance.Close();

        // Assert
        Assert.DoesNotContain(TestContext.JSInterop.Invocations, i => i.Identifier == "twPicker.unregisterOutsideClick");
    }
}