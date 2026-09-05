using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TwBlazor.Components.DatePicker;

namespace TwBlazor.Tests.Components.DatePicker;

public class TwDatePickerDayViewTests : TwBlazorTestBase
{
    [Fact]
    public void RendersCorrectNumberOfCurrentMonthDays()
    {
        // Arrange
        var value = new DateTime(2025, 11, 15, 13, 30, 0);

        // Act
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, value)
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<DateTime>(this, _ => { }))
        );

        var daysInMonth = DateTime.DaysInMonth(2025, 11);
        var dayButtons = cut.FindAll("button.day");

        // Assert
        Assert.Equal(daysInMonth, dayButtons.Count);
    }

    [Fact]
    public void LeadingPreviousMonthDays_RenderedWithPrevClass()
    {
        // Arrange
        var value = new DateTime(2025, 11, 1);
        var firstOfMonth = new DateTime(2025, 11, 1);
        var offset = (int)firstOfMonth.DayOfWeek;

        // Act
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, value)
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<DateTime>(this, _ => { }))
        );

        var prevSpans = cut.FindAll("span.day.prev");

        // Assert
        Assert.Equal(offset, prevSpans.Count);
    }

    [Fact]
    public void ClickingDay_InvokesValueChangedWithNewDatePreservingTime()
    {
        // Arrange
        DateTime? callbackValue = null;
        var initial = new DateTime(2025, 11, 10, 9, 45, 30);

        // Act
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, initial)
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<DateTime>(this, d => callbackValue = d))
        );

        var targetDay = "20";
        var dayButton = cut.FindAll("button.day").First(b => b.TextContent.Trim() == targetDay);
        dayButton.Click();

        // Assert
        Assert.NotNull(callbackValue);
        Assert.Equal(20, callbackValue!.Value.Day);
        Assert.Equal(initial.Hour, callbackValue.Value.Hour);
        Assert.Equal(initial.Minute, callbackValue.Value.Minute);
    }

    private static EventCallback<DateTime> NoOpCallback(TwDatePickerDayViewTests owner) =>
        EventCallback.Factory.Create<DateTime>(owner, _ => { });

    [Fact]
    public void ArrowRight_MovesRovingTabIndexToNextDay()
    {
        // Arrange — Nov 15 2025 is not a week boundary, so a plain +1 day move applies.
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "ArrowRight" });

        // Assert
        var day15 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15");
        var day16 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "16");
        Assert.Equal("-1", day15.GetAttribute("tabindex"));
        Assert.Equal("0", day16.GetAttribute("tabindex"));
    }

    [Fact]
    public void ArrowLeft_MovesRovingTabIndexToPreviousDay()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" });

        // Assert
        var day14 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "14");
        var day15 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15");
        Assert.Equal("0", day14.GetAttribute("tabindex"));
        Assert.Equal("-1", day15.GetAttribute("tabindex"));
    }

    [Fact]
    public void ArrowDown_MovesRovingTabIndexByOneWeek()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });

        // Assert
        var day22 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "22");
        Assert.Equal("0", day22.GetAttribute("tabindex"));
    }

    [Fact]
    public void ArrowUp_MovesRovingTabIndexByOneWeekBack()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "ArrowUp" });

        // Assert
        var day8 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "8");
        Assert.Equal("0", day8.GetAttribute("tabindex"));
    }

    [Fact]
    public void Home_MovesRovingTabIndexToFirstDayOfWeekRow()
    {
        // Arrange — Nov 15 2025 is a Saturday, the last column of its row; Home should land on
        // Nov 9, the Sunday that starts that same row.
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "Home" });

        // Assert
        var day9 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "9");
        var day15 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15");
        Assert.Equal("0", day9.GetAttribute("tabindex"));
        Assert.Equal("-1", day15.GetAttribute("tabindex"));
    }

    [Fact]
    public void End_MovesRovingTabIndexToLastDayOfWeekRow()
    {
        // Arrange — Nov 11 2025 is a Tuesday; End should land on Nov 15, the Saturday that ends
        // that same row.
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 11))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "End" });

        // Assert
        var day11 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "11");
        var day15 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15");
        Assert.Equal("-1", day11.GetAttribute("tabindex"));
        Assert.Equal("0", day15.GetAttribute("tabindex"));
    }

    [Fact]
    public void ArrowLeft_AtFirstDayOfMonth_ClampsWithoutChangingFocus()
    {
        // Arrange — moving left from day 1 would target day 0; Math.Clamp pulls it back to 1,
        // which equals the already-focused day, so OnGridKeyDown returns before re-rendering.
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 1))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" });

        // Assert
        var day1 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "1");
        Assert.Equal("0", day1.GetAttribute("tabindex"));
    }

    [Fact]
    public void UnhandledKey_DoesNotChangeRovingTabIndex()
    {
        // Arrange — a key outside the WAI-ARIA grid pattern's switch falls through to the "target is
        // null" branch and returns immediately.
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "a" });

        // Assert
        var day15 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15");
        Assert.Equal("0", day15.GetAttribute("tabindex"));
    }

    [Fact]
    public void ChangingDayWithinSameMonth_PreservesRovingTabIndexFromKeyboardNavigation()
    {
        // Arrange — move the roving tabindex via the keyboard first.
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );
        cut.Find("table[role='grid']").KeyDown(new KeyboardEventArgs { Key = "ArrowRight" }); // -> day 16

        // Act — re-render with a different day in the SAME month/year. OnParametersSet should skip
        // resetting focusedDay back to Value.Day here, since only the month/year change is meant to
        // reset the roving tabindex - otherwise this would fight the arrow-key navigation just above.
        cut.Render(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 22))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Assert
        var day16 = cut.FindAll("button.day").First(b => b.TextContent.Trim() == "16");
        Assert.Equal("0", day16.GetAttribute("tabindex"));
    }

    [Fact]
    public void SelectedDay_ThatIsNotToday_StillReceivesHighlightClasses()
    {
        // Arrange — a selected-but-not-today date must get its own highlight (the solid
        // LightBackground/DarkBackground classes) rather than depending on also being today.
        // A today-but-not-selected date instead gets the theme's (lighter) ActiveClass treatment,
        // and an unrelated day gets neither - the three states are each visually distinct.
        var today = DateTime.Today;
        var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
        var selectedDay = today.Day == 1 ? 2 : 1;
        var neutralDay = Enumerable.Range(1, daysInMonth).First(d => d != today.Day && d != selectedDay);

        var value = new DateTime(today.Year, today.Month, selectedDay, 0, 0, 0, DateTimeKind.Unspecified);
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, value)
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        var datePickerTheme = Theme.Components.Require<TwBlazor.Configuration.Components.TwDatePickerTheme>();

        // Act
        var selectedButton = cut.FindAll("button.day").First(b => b.TextContent.Trim() == selectedDay.ToString());
        var todayButton = cut.FindAll("button.day").First(b => b.TextContent.Trim() == today.Day.ToString());
        var neutralButton = cut.FindAll("button.day").First(b => b.TextContent.Trim() == neutralDay.ToString());

        // Assert
        Assert.Equal("true", selectedButton.GetAttribute("aria-pressed"));
        Assert.Null(selectedButton.GetAttribute("aria-current"));
        Assert.Contains(Theme.Colors.LightBackground.Light.Primary, selectedButton.GetAttribute("class"));
        Assert.Contains(Theme.Colors.DarkBackground.Light.Primary, selectedButton.GetAttribute("class"));

        Assert.Equal("date", todayButton.GetAttribute("aria-current"));
        Assert.Contains(datePickerTheme.ActiveClass, todayButton.GetAttribute("class"));
        Assert.DoesNotContain(Theme.Colors.LightBackground.Light.Primary, todayButton.GetAttribute("class"));
        Assert.DoesNotContain(Theme.Colors.DarkBackground.Light.Primary, todayButton.GetAttribute("class"));

        Assert.DoesNotContain(Theme.Colors.LightBackground.Light.Primary, neutralButton.GetAttribute("class"));
        Assert.DoesNotContain(Theme.Colors.DarkBackground.Light.Primary, neutralButton.GetAttribute("class"));
        Assert.DoesNotContain(datePickerTheme.ActiveClass, neutralButton.GetAttribute("class"));
    }

    [Fact]
    public void OnAfterRender_SwallowsJSDisconnectedException_WhenRegisteringGuard()
    {
        // Arrange
        TestContext.JSInterop.SetupVoid("twTabs.registerKeydownGuard", _ => true)
            .SetException(new JSDisconnectedException("Circuit disconnected"));

        // Act & Assert — should not throw/propagate during rendering
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        Assert.NotNull(cut.Find("table[role='grid']"));
    }

    [Fact]
    public async Task DisposeAsync_UnregistersKeydownGuard_WhenRegistered()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        await cut.Instance.DisposeAsync();

        // Assert
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twTabs.unregisterKeydownGuard");
    }

    [Fact]
    public async Task DisposeAsync_DoesNothing_WhenGuardWasNeverRegistered()
    {
        // Arrange — registration fails during OnAfterRenderAsync, so keydownGuardRegistered stays
        // false and DisposeAsync's early-return guard should skip the JS call entirely.
        TestContext.JSInterop.SetupVoid("twTabs.registerKeydownGuard", _ => true)
            .SetException(new JSDisconnectedException("Circuit disconnected"));

        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );

        // Act
        await cut.Instance.DisposeAsync();

        // Assert
        Assert.DoesNotContain(TestContext.JSInterop.Invocations, i => i.Identifier == "twTabs.unregisterKeydownGuard");
    }

    [Fact]
    public async Task DisposeAsync_SwallowsJSDisconnectedException()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );
        TestContext.JSInterop.SetupVoid("twTabs.unregisterKeydownGuard", _ => true)
            .SetException(new JSDisconnectedException("Circuit disconnected"));

        // Act & Assert — should not throw
        await cut.Instance.DisposeAsync();
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public async Task DisposeAsync_SwallowsInvalidOperationException()
    {
        // Arrange
        var cut = TestContext.Render<TwDatePickerDayView>(p => p
            .Add(x => x.Value, new DateTime(2025, 11, 15))
            .Add(x => x.ValueChanged, NoOpCallback(this))
        );
        TestContext.JSInterop.SetupVoid("twTabs.unregisterKeydownGuard", _ => true)
            .SetException(new InvalidOperationException("JS interop unavailable"));

        // Act & Assert — should not throw
        await cut.Instance.DisposeAsync();
        Assert.NotNull(cut.Instance);
    }
}