using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;

namespace TwBlazor.Tests.Components.Slider;

public class TwSliderTests : TwBlazorTestBase
{
    [Fact]
    public void TwSlider_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input[type='range']");
        Assert.NotNull(input);
        Assert.Equal("50", input.GetAttribute("value"));
    }

    [Fact]
    public void TwSlider_GeneratesId_WhenNotProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        var id = input.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("slider-", id);
        Assert.DoesNotContain("`", id); // Should not contain generic type indicator
    }

    [Fact]
    public void TwSlider_UsesProvidedId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Id, "custom-slider-id")
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("custom-slider-id", input.GetAttribute("id"));
    }

    [Fact]
    public void TwSlider_GeneratesUniqueIds_ForMultipleInstances()
    {
        // Arrange & Act
        var cut1 = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));
        var cut2 = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var id1 = cut1.Find("input").GetAttribute("id");
        var id2 = cut2.Find("input").GetAttribute("id");
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void TwSlider_SetsMinAttribute()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 10)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("10", input.GetAttribute("min"));
    }

    [Fact]
    public void TwSlider_SetsMaxAttribute()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 200)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("200", input.GetAttribute("max"));
    }

    [Fact]
    public void TwSlider_SetsStepAttribute()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 5));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("5", input.GetAttribute("step"));
    }

    [Fact]
    public void TwSlider_SetsName_WhenProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Name, "volume")
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("volume", input.GetAttribute("name"));
    }

    [Fact]
    public void TwSlider_RendersLabel_WhenLabelProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Label, "Volume Control")
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var label = cut.Find("label");
        Assert.NotNull(label);
        Assert.Contains("Volume Control", label.TextContent);
    }

    [Fact]
    public void TwSlider_LabelFor_MatchesSliderId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Id, "volume-slider")
            .Add(p => p.Label, "Volume")
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var label = cut.Find("label");
        var input = cut.Find("input");
        Assert.Equal("volume-slider", label.GetAttribute("for"));
        Assert.Equal("volume-slider", input.GetAttribute("id"));
    }

    [Fact]
    public void TwSlider_InvokesValueChanged_WhenValueChanges()
    {
        // Arrange
        int? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input(75);

        // Assert
        Assert.NotNull(valueFromCallback);
        Assert.Equal(75, valueFromCallback.Value);
    }

    [Fact]
    public void TwSlider_AppliesCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Class, "custom-slider-class")
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        Assert.Contains("custom-slider-class", input.GetAttribute("class"));
    }

    [Fact]
    public void TwSlider_AppliesCustomLabelClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Label, "Test Label")
            .Add(p => p.LabelClass, "text-blue-600")
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("text-blue-600", label.GetAttribute("class"));
    }

    [Fact]
    public void TwSlider_ReadOnly_DoesNotApplyDisabledAttribute()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert - Readonly should NOT apply disabled attribute to maintain color visibility
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("disabled"));
        Assert.DoesNotContain("readonly", input.Attributes.Select(a => a.Name));
    }

    [Fact]
    public void TwSlider_ReadOnly_AppliesPointerEventsNone()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert - Readonly should apply pointer-events-none to prevent interaction while maintaining color
        var input = cut.Find("input");
        var classes = input.GetAttribute("class");
        Assert.Contains("pointer-events-none", classes);
        Assert.DoesNotContain("opacity-40", classes);
        Assert.DoesNotContain("cursor-not-allowed", classes);
    }

    [Fact]
    public void TwSlider_NotReadOnly_DoesNotApplyDisabledAttribute()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.ReadOnly, false)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("disabled"));
    }

    [Fact]
    public void TwSlider_DoesNotInvokeCallback_WhenReadonly()
    {
        // Arrange
        int? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input(75);

        // Assert
        Assert.Null(valueFromCallback); // Callback should not be invoked when readonly
    }

    [Fact]
    public void TwSlider_AppliesDisabledState()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("disabled"));
    }

    [Fact]
    public void TwSlider_DoesNotInvokeCallback_WhenDisabled()
    {
        // Arrange
        int? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input(75);

        // Assert
        Assert.Null(valueFromCallback); // Callback should not be invoked when disabled
    }

    [Fact]
    public void TwSlider_AppliesDisabledClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        var classes = input.GetAttribute("class");
        Assert.Contains("opacity-40", classes);
        Assert.Contains("cursor-not-allowed", classes);
    }

    [Fact]
    public void TwSlider_WorksWithDecimalValues()
    {
        // Arrange
        decimal? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<decimal>>(parameters => parameters
            .Add(p => p.Value, 5.5m)
            .Add(p => p.Min, 0.0m)
            .Add(p => p.Max, 10.0m)
            .Add(p => p.Step, 0.5m)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input("7.5");

        // Assert
        Assert.NotNull(valueFromCallback);
        Assert.Equal(7.5m, valueFromCallback.Value);
    }

    [Fact]
    public void TwSlider_WorksWithDoubleValues()
    {
        // Arrange
        double? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<double>>(parameters => parameters
            .Add(p => p.Value, 3.14)
            .Add(p => p.Min, 0.0)
            .Add(p => p.Max, 10.0)
            .Add(p => p.Step, 0.01)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<double>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input("6.28");

        // Assert
        Assert.NotNull(valueFromCallback);
        Assert.Equal(6.28, valueFromCallback.Value);
    }

    [Fact]
    public void TwSlider_AppliesColorClass_WhenColorProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Color, Color.Primary)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var input = cut.Find("input");
        var classes = input.GetAttribute("class");
        Assert.NotNull(classes);
        // Color classes are applied through ColorBuilder.GetSliderColor
    }

    [Fact]
    public void TwSlider_HandlesInvalidInput_Gracefully()
    {
        // Arrange
        int? valueFromCallback = null;
        var initialValue = 50;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, initialValue)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input("invalid");

        // Assert
        // Should not throw and should not invoke callback with invalid data
        Assert.Null(valueFromCallback);
    }

    [Fact]
    public void TwSlider_HandlesEmptyInput_Gracefully()
    {
        // Arrange
        int? valueFromCallback = null;
        var initialValue = 50;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, initialValue)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input("");

        // Assert
        // Should not throw and should not invoke callback with empty data
        Assert.Null(valueFromCallback);
    }

    [Fact]
    public void TwSlider_DoesNotInvokeCallback_WhenInputValueIsNull()
    {
        // Arrange
        int? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        // Simulate null value scenario using Input event
        input.Input(new ChangeEventArgs { Value = null });

        // Assert
        Assert.Null(valueFromCallback);
    }

    [Fact]
    public void TwSlider_SupportsNegativeRange()
    {
        // Arrange
        int? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, -25)
            .Add(p => p.Min, -100)
            .Add(p => p.Max, 0)
            .Add(p => p.Step, 5)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input("-50");

        // Assert
        Assert.NotNull(valueFromCallback);
        Assert.Equal(-50, valueFromCallback.Value);
    }

    [Fact]
    public void TwSlider_SupportsLargeNumbers()
    {
        // Arrange
        long? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<long>>(parameters => parameters
            .Add(p => p.Value, 1000000L)
            .Add(p => p.Min, 0L)
            .Add(p => p.Max, 10000000L)
            .Add(p => p.Step, 100000L)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<long>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input("5000000");

        // Assert
        Assert.NotNull(valueFromCallback);
        Assert.Equal(5000000L, valueFromCallback.Value);
    }

    [Fact]
    public void TwSlider_ReadOnly_PreventsValueChange()
    {
        // Arrange
        var initialValue = 50;
        var changedValue = initialValue;
        var callbackInvoked = false;

        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, initialValue)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v =>
            {
                callbackInvoked = true;
                changedValue = v;
            })));

        // Act - Try to change the value
        var input = cut.Find("input");
        input.Input(75);

        // Assert - Value should not change and callback should not be invoked
        Assert.False(callbackInvoked);
        Assert.Equal(initialValue, changedValue);
    }

    [Fact]
    public void TwSlider_Disabled_PreventsValueChange()
    {
        // Arrange
        var initialValue = 50;
        var changedValue = initialValue;
        var callbackInvoked = false;

        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, initialValue)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v =>
            {
                callbackInvoked = true;
                changedValue = v;
            })));

        // Act - Try to change the value
        var input = cut.Find("input");
        input.Input(75);

        // Assert - Value should not change and callback should not be invoked
        Assert.False(callbackInvoked);
        Assert.Equal(initialValue, changedValue);
    }

    [Fact]
    public void TwSlider_ReadOnly_AndDisabled_AppliesDisabledStyling()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert - When both are set, disabled takes precedence
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("disabled"));
        Assert.Contains("opacity-40", input.GetAttribute("class"));
        Assert.Contains("cursor-not-allowed", input.GetAttribute("class"));
        // Should not have pointer-events-none since Disabled takes precedence
        Assert.DoesNotContain("pointer-events-none", input.GetAttribute("class"));
    }

    [Fact]
    public void TwSlider_ReadOnly_WithDecimal_PreventsValueChange()
    {
        // Arrange
        var initialValue = 0.5m;
        var changedValue = initialValue;
        var callbackInvoked = false;

        var cut = TestContext.Render<TwSlider<decimal>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, initialValue)
            .Add(p => p.Min, 0m)
            .Add(p => p.Max, 1m)
            .Add(p => p.Step, 0.01m)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(this, v =>
            {
                callbackInvoked = true;
                changedValue = v;
            })));

        // Act - Try to change the value
        var input = cut.Find("input");
        input.Input("0.75");

        // Assert - Value should not change and callback should not be invoked
        Assert.False(callbackInvoked);
        Assert.Equal(initialValue, changedValue);
    }

    [Fact]
    public void TwSlider_NotReadOnly_AllowsValueChange()
    {
        // Arrange
        int? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.ReadOnly, false)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input(75);

        // Assert - Value should change and callback should be invoked
        Assert.NotNull(valueFromCallback);
        Assert.Equal(75, valueFromCallback.Value);
    }

    [Fact]
    public void TwSlider_NotDisabled_AllowsValueChange()
    {
        // Arrange
        int? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Disabled, false)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Input(75);

        // Assert - Value should change and callback should be invoked
        Assert.NotNull(valueFromCallback);
        Assert.Equal(75, valueFromCallback.Value);
    }

    [Fact]
    public void Percentage_ReturnsZero_WhenMaxIsNotGreaterThanMin()
    {
        // Arrange & Act - the `if (max <= min) return 0;` guard, using equal bounds.
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 5)
            .Add(p => p.Min, 10)
            .Add(p => p.Max, 10)
            .Add(p => p.Step, 1));

        // Assert
        var fill = cut.Find("div.h-full");
        Assert.Contains("width:0%", fill.GetAttribute("style"));
    }

    [Fact]
    public void Percentage_ReturnsZero_WhenValuesAreNotConvertibleToDouble()
    {
        // Arrange & Act - Min/Max/Value are strings that can't be parsed as doubles, so
        // Convert.ToDouble throws FormatException, caught by the percentage getter's
        // `when` filter and falling back to 0 rather than propagating.
        var cut = TestContext.Render<TwSlider<string>>(parameters => parameters
            .Add(p => p.Value, "abc")
            .Add(p => p.Min, "abc")
            .Add(p => p.Max, "xyz")
            .Add(p => p.Step, "1"));

        // Assert
        var fill = cut.Find("div.h-full");
        Assert.Contains("width:0%", fill.GetAttribute("style"));
    }

    [Fact]
    public void OnParametersSet_DoesNotDuplicate_DisabledAttribute_OnRerender()
    {
        // Arrange - pre-populate Attributes with "disabled" already present, covering the
        // `!Attributes.ContainsKey("disabled")` false branch (already-present case).
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.Attributes, new Dictionary<string, object> { ["disabled"] = true }));

        // Act - re-render while still disabled
        cut.Render(parameters => parameters.Add(p => p.Disabled, true));

        // Assert
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("disabled"));
    }

    [Fact]
    public void HandleInput_CatchesOverflowException_WhenValueExceedsTypeRange()
    {
        // Arrange - a numeric string that is well-formed but too large for int,
        // so Convert.ChangeType throws OverflowException rather than FormatException.
        int? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(this, v => valueFromCallback = v)));

        var input = cut.Find("input");

        // Act
        input.Input("99999999999999999999");

        // Assert - value is left unchanged, callback not invoked
        Assert.Null(valueFromCallback);
    }

    [Fact]
    public void HandleInput_CatchesInvalidCastException_ForNonConvertibleType()
    {
        // Arrange - Guid doesn't implement IConvertible, so Convert.ChangeType throws
        // InvalidCastException rather than FormatException/OverflowException.
        Guid? valueFromCallback = null;
        var cut = TestContext.Render<TwSlider<Guid>>(parameters => parameters
            .Add(p => p.Value, Guid.Empty)
            .Add(p => p.Min, Guid.Empty)
            .Add(p => p.Max, Guid.Empty)
            .Add(p => p.Step, Guid.Empty)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<Guid>(this, v => valueFromCallback = v)));

        var input = cut.Find("input");

        // Act
        input.Input(Guid.NewGuid().ToString());

        // Assert - value is left unchanged, callback not invoked
        Assert.Null(valueFromCallback);
    }

    [Theory]
    [InlineData(Color.Accent)]
    [InlineData(Color.Success)]
    [InlineData(Color.Danger)]
    [InlineData(Color.Warning)]
    [InlineData(Color.Info)]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void GetSliderColor_ReturnsThemeColor_ForEachNonPrimaryColor(Color color)
    {
        // Arrange - GetSliderColor's switch expression has a distinct branch for each Color value;
        // only Primary (and the null/default fallback) is exercised elsewhere in this file.
        var sliderTheme = Theme.Components.Require<TwSliderTheme>();
        var expected = color switch
        {
            Color.Accent => sliderTheme.Colors.Accent,
            Color.Success => sliderTheme.Colors.Success,
            Color.Danger => sliderTheme.Colors.Danger,
            Color.Warning => sliderTheme.Colors.Warning,
            Color.Info => sliderTheme.Colors.Info,
            Color.Light => sliderTheme.Colors.Light,
            Color.Dark => sliderTheme.Colors.Dark,
            _ => throw new ArgumentOutOfRangeException(nameof(color))
        };

        // Act
        var cut = TestContext.Render<TwSlider<int>>(parameters => parameters
            .Add(p => p.Color, color)
            .Add(p => p.Value, 50)
            .Add(p => p.Min, 0)
            .Add(p => p.Max, 100)
            .Add(p => p.Step, 1));

        // Assert
        var fill = cut.Find("div.h-full");
        Assert.Contains(expected, fill.GetAttribute("class"));
    }
}
