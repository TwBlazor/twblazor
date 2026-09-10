using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components;

namespace TwBlazor.Tests.Components.DateRangePicker;

public class TwDateRangePickerTests : TwBlazorTestBase
{
    public TwDateRangePickerTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
        TestContext.JSInterop.SetupVoid("twPicker.registerOutsideClick");
        TestContext.JSInterop.SetupVoid("twPicker.unregisterOutsideClick");
    }

    private static EventCallback<KeyValuePair<DateTime?, DateTime?>> NoOpRangeCallback(TwDateRangePickerTests owner) =>
        EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(owner, _ => { });

    [Fact]
    public void ClickingTwoDays_SelectsRange_AndInvokesCallbacks()
    {
        // Arrange
        KeyValuePair<DateTime?, DateTime?>? rangeFromCallback = null;
        string? valueFromCallback = null;

        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, default(KeyValuePair<DateTime?, DateTime?>))
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, r => rangeFromCallback = r))
            .Add(x => x.ValueChanged, EventCallback.Factory.Create<string>(this, v => valueFromCallback = v))
            .Add(x => x.AriaLabel, "Trip dates")
        );

        // Act
        cut.Find("input").Focus();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "5").Click();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15").Click();

        // Assert
        Assert.NotNull(rangeFromCallback);
        Assert.Equal(5, rangeFromCallback!.Value.Key!.Value.Day);
        Assert.Equal(15, rangeFromCallback.Value.Value!.Value.Day);
        Assert.NotNull(valueFromCallback);
        Assert.Contains(" - ", valueFromCallback); // default RangeSeparator - locale-neutral, no translation needed
    }

    [Fact]
    public void ClickingEarlierSecondDay_SwapsStartAndEnd()
    {
        // Arrange
        KeyValuePair<DateTime?, DateTime?>? rangeFromCallback = null;
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, default(KeyValuePair<DateTime?, DateTime?>))
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, r => rangeFromCallback = r))
        );

        // Act
        cut.Find("input").Focus();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15").Click();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "5").Click();

        // Assert — the earlier day becomes the start (Key) regardless of click order.
        Assert.Equal(5, rangeFromCallback!.Value.Key!.Value.Day);
        Assert.Equal(15, rangeFromCallback.Value.Value!.Value.Day);
    }

    [Fact]
    public void ClickingFirstDay_KeepsPanelOpen_ForSecondPick()
    {
        // Arrange
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, default(KeyValuePair<DateTime?, DateTime?>))
            .Add(x => x.SelectedRangeChanged, NoOpRangeCallback(this))
        );

        // Act
        cut.Find("input").Focus();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "5").Click();

        // Assert
        Assert.Contains("datepicker-grid", cut.Markup);
    }

    [Fact]
    public void ClickingThirdDay_AfterCompleteRange_StartsNewRange()
    {
        // Arrange
        KeyValuePair<DateTime?, DateTime?>? rangeFromCallback = null;
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, default(KeyValuePair<DateTime?, DateTime?>))
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, r => rangeFromCallback = r))
        );

        cut.Find("input").Focus();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "5").Click();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "15").Click();

        // Act — panel reopens for a fresh selection.
        cut.Find("input").Focus();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "20").Click();

        // Assert
        Assert.Equal(20, rangeFromCallback!.Value.Key!.Value.Day);
        Assert.Null(rangeFromCallback.Value.Value);
    }

    [Fact]
    public void RangeMode_RendersTwoCalendarMonths()
    {
        // Arrange
        var start = new DateTime(2025, 11, 1);

        // Act
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(start, null))
            .Add(x => x.SelectedRangeChanged, NoOpRangeCallback(this))
        );
        cut.Find("input").Focus();

        // Assert
        var grids = cut.FindAll("table[role='grid']");
        Assert.Equal(2, grids.Count);
        Assert.Contains("November 2025", cut.Markup);
        Assert.Contains("December 2025", cut.Markup);
    }

    [Fact]
    public void TypingValidRangeText_ParsesAndSelectsRange()
    {
        // Arrange
        KeyValuePair<DateTime?, DateTime?>? rangeFromCallback = null;

        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, r => rangeFromCallback = r))
        );

        // Act
        cut.Find("input").Change("05/11/2025 - 15/11/2025");

        // Assert
        Assert.NotNull(rangeFromCallback);
        Assert.Equal(new DateTime(2025, 11, 5), rangeFromCallback!.Value.Key);
        Assert.Equal(new DateTime(2025, 11, 15), rangeFromCallback.Value.Value);
    }

    [Fact]
    public void TypingInvalidRangeText_ShowsError_DoesNotSelect()
    {
        // Arrange
        var callbackInvoked = false;
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, _ => callbackInvoked = true))
        );

        // Act
        cut.Find("input").Change("not a range");

        // Assert
        Assert.False(callbackInvoked);
        Assert.True(cut.Instance.Invalid);
        Assert.Equal("Enter a valid date range", cut.Instance.ErrorMessage);
    }

    [Fact]
    public void RangeSeparator_DefaultsToDash_NotToWord()
    {
        // Arrange & Act — locale-neutral by default: a symbol needs no translation the way an
        // English word like "to" would.
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(new DateTime(2025, 11, 5), new DateTime(2025, 11, 15)))
        );

        // Assert
        Assert.Equal("05/11/2025 - 15/11/2025", cut.Find("input").GetAttribute("value"));
    }

    [Fact]
    public void CustomRangeSeparator_IsReflectedInDisplayedValue()
    {
        // Arrange & Act — whatever separator a consumer overrides RangeSeparator to must show up
        // in the rendered value, not just the default " - ".
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.RangeSeparator, " | ")
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(new DateTime(2025, 11, 5), new DateTime(2025, 11, 15)))
        );

        // Assert
        Assert.Equal("05/11/2025 | 15/11/2025", cut.Find("input").GetAttribute("value"));
    }

    [Fact]
    public void EffectivePlaceholder_UsesFormatAndDefaultSeparator()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDateRangePicker>();

        // Assert
        Assert.Equal("dd/mm/yyyy - dd/mm/yyyy", cut.Find("input").GetAttribute("placeholder"));
    }

    [Fact]
    public void CustomFormat_AppliesToBothStartAndEndDate()
    {
        // Arrange — Format is passed straight through to both dates, the same way TwDatePicker
        // uses it for its single date.
        KeyValuePair<DateTime?, DateTime?>? rangeFromCallback = null;

        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.Format, "MM/dd/yyyy")
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, r => rangeFromCallback = r))
        );

        // Act
        cut.Find("input").Change("12/24/2026 - 01/05/2027");

        // Assert
        Assert.NotNull(rangeFromCallback);
        Assert.Equal(new DateTime(2026, 12, 24), rangeFromCallback!.Value.Key);
        Assert.Equal(new DateTime(2027, 1, 5), rangeFromCallback.Value.Value);
    }

    [Fact]
    public void CustomFormat_DisplaysBothDatesInThatPattern()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.Format, "MM/dd/yyyy")
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(new DateTime(2026, 12, 24), new DateTime(2027, 1, 5)))
        );

        // Assert
        Assert.Equal("12/24/2026 - 01/05/2027", cut.Find("input").GetAttribute("value"));
    }

    [Fact]
    public void DashFormat_WithDefaultSeparator_StillParsesUnambiguously()
    {
        // Arrange — the default RangeSeparator (" - ", with surrounding spaces) doesn't collide
        // with a Format that itself uses "-" without spaces around it (e.g. "dd-MM-yyyy"), so
        // typed text still splits into exactly two parts.
        KeyValuePair<DateTime?, DateTime?>? rangeFromCallback = null;
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.Format, "dd-MM-yyyy")
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, r => rangeFromCallback = r))
        );

        // Act
        cut.Find("input").Change("05-11-2025 - 15-11-2025");

        // Assert
        Assert.NotNull(rangeFromCallback);
        Assert.Equal(new DateTime(2025, 11, 5), rangeFromCallback!.Value.Key);
        Assert.Equal(new DateTime(2025, 11, 15), rangeFromCallback.Value.Value);
    }

    [Fact]
    public void FormatEmbeddingSpacedDash_CollidesWithDefaultSeparator()
    {
        // Arrange — documents the real remaining tradeoff noted on RangeSeparator's summary: a
        // Format that itself embeds " - " (spaces around a dash, e.g. "dd - MM - yyyy") does
        // collide with the default separator, splitting a correctly-typed value into more than
        // two parts. An everyday dash format without spaces (see the test above) does not.
        var callbackInvoked = false;
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.Format, "dd - MM - yyyy")
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, _ => callbackInvoked = true))
        );

        // Act
        cut.Find("input").Change("05 - 11 - 2025 - 15 - 11 - 2025");

        // Assert
        Assert.False(callbackInvoked);
        Assert.True(cut.Instance.Invalid);
    }

    [Fact]
    public void SwitchingViews_MonthThenYear_ThenSelectingMonth_ReturnsToDayView()
    {
        // Arrange
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(new DateTime(2025, 11, 1), null))
            .Add(x => x.SelectedRangeChanged, NoOpRangeCallback(this))
        );

        // Act & Assert
        cut.Find("input").Focus();
        cut.Find("button.view-switch").Click(); // Day -> Month
        Assert.Contains("months-of-the-year", cut.Markup);

        cut.Find("button.view-switch").Click(); // Month -> Year
        Assert.Contains("years-of-the-decade", cut.Markup);

        var yearButtons = cut.FindAll("button.year");
        Assert.Equal(10, yearButtons.Count);
    }

    [Fact]
    public void NavigatingToAYearWithoutTheRangeStart_DoesNotHighlightAnyMonthAsSelected()
    {
        // Arrange — the range's start is September 2026; navigating the month grid to a different
        // year must not make that year's months falsely show one as selected (this was the bug:
        // isSelectedMonth used to compare against anchorMonth - the navigation position itself -
        // instead of the actual range start, so whichever page you'd browsed to always highlighted
        // itself).
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(new DateTime(2026, 9, 8), null))
            .Add(x => x.SelectedRangeChanged, NoOpRangeCallback(this))
        );

        // Act
        cut.Find("input").Focus();
        cut.Find("button.view-switch").Click(); // Day -> Month
        cut.Find("button.prev-btn").Click(); // 2026 -> 2025

        // Assert
        var monthButtons = cut.FindAll("button.month");
        Assert.All(monthButtons, b => Assert.Equal("false", b.GetAttribute("aria-pressed")));
    }

    [Fact]
    public void MonthGrid_HighlightsMonthContainingRangeStart()
    {
        // Arrange
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(new DateTime(2026, 9, 8), null))
            .Add(x => x.SelectedRangeChanged, NoOpRangeCallback(this))
        );

        // Act
        cut.Find("input").Focus();
        cut.Find("button.view-switch").Click(); // Day -> Month

        // Assert
        var septemberButton = cut.FindAll("button.month").First(b => b.TextContent.Trim() == "Sept" || b.TextContent.Trim() == "Sep");
        Assert.Equal("true", septemberButton.GetAttribute("aria-pressed"));
    }

    [Fact]
    public void NavigatingToADecadeWithoutTheRangeStart_DoesNotHighlightAnyYearAsSelected()
    {
        // Arrange — same bug as the month-grid case above, one level up.
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(new DateTime(2026, 9, 8), null))
            .Add(x => x.SelectedRangeChanged, NoOpRangeCallback(this))
        );

        // Act
        cut.Find("input").Focus();
        cut.Find("button.view-switch").Click(); // Day -> Month
        cut.Find("button.view-switch").Click(); // Month -> Year
        cut.Find("button.next-btn").Click(); // 2026-2035 -> 2036-2045, well past the range start

        // Assert
        var yearButtons = cut.FindAll("button.year");
        Assert.All(yearButtons, b => Assert.Equal("false", b.GetAttribute("aria-pressed")));
    }

    [Fact]
    public void ClickingDayBeforeMinDate_DoesNotSelect()
    {
        // Arrange
        var callbackInvoked = false;
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, default(KeyValuePair<DateTime?, DateTime?>))
            .Add(x => x.MinDate, new DateTime(2025, 11, 10))
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, _ => callbackInvoked = true))
        );

        // Act
        cut.Find("input").Focus();
        cut.FindAll("button.day").First(b => b.TextContent.Trim() == "5").Click();

        // Assert
        Assert.False(callbackInvoked);
    }

    [Fact]
    public void PreviousMonthButton_IsDisabled_WhenAnchorMonthIsMinDateMonth()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, new KeyValuePair<DateTime?, DateTime?>(new DateTime(2025, 11, 1), null))
            .Add(x => x.MinDate, new DateTime(2025, 11, 1))
            .Add(x => x.SelectedRangeChanged, NoOpRangeCallback(this))
        );
        cut.Find("input").Focus();

        // Assert
        Assert.True(cut.Find(".prev-btn").HasAttribute("disabled"));

        // Act — clicking a disabled button should not move the displayed month.
        cut.Find(".prev-btn").Click();
        Assert.Contains("November 2025", cut.Markup);
    }

    [Fact]
    public void TypingRangeOutsideMinMaxBounds_ShowsError_DoesNotSelect()
    {
        // Arrange
        var callbackInvoked = false;
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.MinDate, new DateTime(2025, 11, 10))
            .Add(x => x.MaxDate, new DateTime(2025, 11, 20))
            .Add(x => x.SelectedRangeChanged, EventCallback.Factory.Create<KeyValuePair<DateTime?, DateTime?>>(this, _ => callbackInvoked = true))
        );

        // Act
        cut.Find("input").Change("01/11/2025 - 15/11/2025");

        // Assert
        Assert.False(callbackInvoked);
        Assert.True(cut.Instance.Invalid);
    }

    [Fact]
    public async Task Close_UnregistersJS_And_HidesPopup()
    {
        // Arrange
        var cut = TestContext.Render<TwDateRangePicker>(p => p
            .Add(x => x.SelectedRange, default(KeyValuePair<DateTime?, DateTime?>))
            .Add(x => x.SelectedRangeChanged, NoOpRangeCallback(this))
        );

        // Act
        cut.Find("input").Focus();
        Assert.Contains("datepicker-grid", cut.Markup);
        await cut.Instance.Close();

        // Assert
        Assert.DoesNotContain("datepicker-grid", cut.Markup);
        Assert.Contains(TestContext.JSInterop.Invocations, i => i.Identifier == "twPicker.unregisterOutsideClick");
    }
}
