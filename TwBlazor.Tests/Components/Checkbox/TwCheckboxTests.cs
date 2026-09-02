using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components;
using TwBlazor.Enums;

namespace TwBlazor.Tests.Components.Checkbox;

public class TwCheckboxTests : TwBlazorTestBase
{
    [Fact]
    public void TwCheckbox_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input[type='checkbox']");
        Assert.NotNull(input);
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    public void TwCheckbox_GeneratesId_WhenNotProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        var id = input.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("checkbox-", id);
        Assert.DoesNotContain("`", id); // Should not contain generic type indicator
    }

    [Fact]
    public void TwCheckbox_UsesProvidedId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Id, "custom-checkbox-id")
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("custom-checkbox-id", input.GetAttribute("id"));
    }

    [Fact]
    public void TwCheckbox_GeneratesUniqueIds_ForMultipleInstances()
    {
        // Arrange & Act
        var cut1 = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false));
        var cut2 = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false));

        // Assert
        var id1 = cut1.Find("input").GetAttribute("id");
        var id2 = cut2.Find("input").GetAttribute("id");
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void TwCheckbox_Renders_AsChecked_WhenValueIsTrue()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, true));

        // Assert
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("checked"));
    }

    [Fact]
    public void TwCheckbox_Renders_AsUnchecked_WhenValueIsFalse()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    public void TwCheckbox_RendersLabel_WhenLabelProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Label, "Accept Terms")
            .Add(p => p.Value, false));

        // Assert
        var label = cut.Find("label");
        Assert.NotNull(label);
        Assert.Contains("Accept Terms", label.TextContent);
    }

    [Fact]
    public void TwCheckbox_LabelFor_MatchesCheckboxId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Id, "terms-checkbox")
            .Add(p => p.Label, "I Agree")
            .Add(p => p.Value, false));

        // Assert
        var label = cut.Find("label");
        var input = cut.Find("input");
        Assert.Equal("terms-checkbox", label.GetAttribute("for"));
        Assert.Equal("terms-checkbox", input.GetAttribute("id"));
    }

    [Fact]
    public void TwCheckbox_InvokesValueChanged_WhenChecked()
    {
        // Arrange
        bool? valueFromCallback = null;
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<bool>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Change(true);

        // Assert
        Assert.NotNull(valueFromCallback);
        Assert.True(valueFromCallback.Value);
    }

    [Fact]
    public void TwCheckbox_InvokesValueChanged_WhenUnchecked()
    {
        // Arrange
        bool? valueFromCallback = null;
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, true)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<bool>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Change(false);

        // Assert
        Assert.NotNull(valueFromCallback);
        Assert.False(valueFromCallback.Value);
    }

    [Fact]
    public void TwCheckbox_SetsName_WhenProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Name, "agreement")
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("agreement", input.GetAttribute("name"));
    }

    [Fact]
    public void TwCheckbox_AppliesCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Class, "custom-checkbox-class")
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.Contains("custom-checkbox-class", input.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckbox_HasDefaultClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        var classes = input.GetAttribute("class");
        Assert.Contains("peer", classes);
        Assert.Contains("h-5", classes);
        Assert.Contains("w-5", classes);
        Assert.Contains("cursor-pointer", classes);
        Assert.Contains("appearance-none", classes);
        Assert.Contains("rounded", classes);
        Assert.Contains("border", classes);
        Assert.Contains("border-gray-300", classes);
    }

    [Fact]
    public void TwCheckbox_AppliesCustomLabelClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Label, "Test Label")
            .Add(p => p.LabelClass, "text-blue-600")
            .Add(p => p.Value, false));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("text-blue-600", label.GetAttribute("class"));
        Assert.Contains("flex", label.GetAttribute("class"));
        Assert.Contains("items-center", label.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckbox_AppliesAriaReadonly_WhenReadonly()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, false));

        // Assert - Readonly uses aria-readonly attribute for accessibility
        var input = cut.Find("input");
        Assert.Equal("true", input.GetAttribute("aria-readonly"));
    }

    [Fact]
    public void TwCheckbox_DoesNotApplyAriaReadonly_WhenNotReadonly()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.ReadOnly, false)
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("aria-readonly"));
    }

    [Fact]
    public void TwCheckbox_DoesNotInvokeCallback_WhenReadonly()
    {
        // Arrange
        bool? valueFromCallback = null;
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, false)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<bool>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Change(true);

        // Assert
        Assert.Null(valueFromCallback); // Callback should not be invoked when readonly
    }

    [Fact]
    public void TwCheckbox_AppliesDisabledState()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("disabled"));
    }

    [Fact]
    public void TwCheckbox_DoesNotInvokeCallback_WhenDisabled()
    {
        // Arrange
        bool? valueFromCallback = null;
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, false)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<bool>(this, v => valueFromCallback = v)));

        // Act
        var input = cut.Find("input");
        input.Change(true);

        // Assert
        Assert.Null(valueFromCallback); // Callback should not be invoked when disabled
    }

    [Fact]
    public void TwCheckbox_AppliesDisabledClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        var classes = input.GetAttribute("class");
        Assert.Contains("opacity-40", classes);
        Assert.Contains("cursor-not-allowed", classes);
    }

    [Fact]
    public void TwCheckbox_LabelHasPointerEventsNone_WhenDisabled()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Label, "Disabled Checkbox")
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, false));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("pointer-events-none", label.GetAttribute("class"));
        Assert.Contains("opacity-40", label.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckbox_LabelHasPointerEventsNone_WhenReadonly()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Label, "Readonly Checkbox")
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, false));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("pointer-events-none", label.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckbox_AppliesPurpleColor_WhenColorIsPrimary()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Color, Color.Primary)
            .Add(p => p.Value, false));

        // Assert
        var markup = cut.Markup;
        Assert.Contains("checked:bg-purple-600", markup);
    }

    [Fact]
    public void TwCheckbox_AppliesGreenColor_WhenColorIsGreen()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Color, Color.Success)
            .Add(p => p.Value, false));

        // Assert
        var markup = cut.Markup;
        Assert.Contains("checked:bg-green-600", markup);
    }

    [Fact]
    public void TwCheckbox_AppliesRedColor_WhenColorIsRed()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Color, Color.Danger)
            .Add(p => p.Value, false));

        // Assert
        var markup = cut.Markup;
        Assert.Contains("checked:bg-red-600", markup);
    }

    [Fact]
    public void TwCheckbox_AppliesFuchsiaColor_WhenColorIsAccent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Color, Color.Accent)
            .Add(p => p.Value, false));

        // Assert
        var markup = cut.Markup;
        Assert.Contains("checked:bg-fuchsia-600", markup);
    }

    [Theory]
    [InlineData(Color.Primary, "purple")]
    [InlineData(Color.Success, "green")]
    [InlineData(Color.Danger, "red")]
    [InlineData(Color.Accent, "fuchsia")]
    [InlineData(Color.Warning, "yellow")]
    [InlineData(Color.Info, "blue")]
    public void TwCheckbox_AppliesCorrectColor_ForEachColorEnum(Color color, string colorName)
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Color, color)
            .Add(p => p.Value, false));

        // Assert
        var markup = cut.Markup;
        Assert.Contains($"checked:bg-{colorName}-600", markup);
    }

    [Fact]
    public void TwCheckbox_InheritsAriaProperties_FromBaseClass()
    {
        // Arrange & Act - AriaLabel and AriaLabelledBy are inherited from TwBlazorComponentBase
        // but may not be applied directly to the input element in the current implementation
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.AriaLabel, "Accept terms and conditions")
            .Add(p => p.Value, false));

        // Assert - Component accepts the property without error
        var input = cut.Find("input");
        Assert.NotNull(input);
    }

    [Fact]
    public void TwCheckbox_AcceptsAriaLabelledBy_Property()
    {
        // Arrange & Act - AriaLabelledBy is inherited from TwBlazorComponentBase
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.AriaLabelledBy, "terms-label")
            .Add(p => p.Value, false));

        // Assert - Component accepts the property without error
        var input = cut.Find("input");
        Assert.NotNull(input);
    }

    [Fact]
    public void TwCheckbox_RendersWithAllProperties()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Id, "full-checkbox")
            .Add(p => p.Name, "agreement")
            .Add(p => p.Label, "I Accept")
            .Add(p => p.LabelClass, "font-bold")
            .Add(p => p.Class, "custom-check")
            .Add(p => p.Color, Color.Success)
            .Add(p => p.Value, true));

        // Assert
        var input = cut.Find("input");
        var label = cut.Find("label");

        Assert.Equal("full-checkbox", input.GetAttribute("id"));
        Assert.Equal("agreement", input.GetAttribute("name"));
        Assert.Contains("custom-check", input.GetAttribute("class"));
        Assert.True(input.HasAttribute("checked"));

        Assert.Contains("I Accept", label.TextContent);
        Assert.Contains("font-bold", label.GetAttribute("class"));
        Assert.Equal("full-checkbox", label.GetAttribute("for"));
    }

    [Fact]
    public void TwCheckbox_LabelHasCursorPointer_WhenNotDisabledOrReadonly()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Label, "Normal Checkbox")
            .Add(p => p.Value, false));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("cursor-pointer", label.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckbox_LabelContainsSelectNone()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Label, "Test")
            .Add(p => p.Value, false));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("select-none", label.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckbox_BothReadonlyAndDisabled_CanBeSetTogether()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("disabled"));
        Assert.Equal("true", input.GetAttribute("aria-readonly"));
    }

    [Fact]
    public void TwCheckbox_UsesGroupName_WhenNameNotProvided()
    {
        // Arrange & Act - TwCheckboxGroup cascades its Name as GroupName; an unnamed
        // child TwCheckbox should fall back to that cascaded value.
        var cut = TestContext.Render<TwCheckboxGroup<bool>>(parameters => parameters
            .Add(p => p.Name, "cascaded-group")
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TwCheckbox<bool>>(0);
                builder.AddAttribute(1, "Value", false);
                builder.CloseComponent();
            }));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("cascaded-group", input.GetAttribute("name"));
    }

    [Fact]
    public void TwCheckbox_Renders_Unchecked_WhenValueIsNotBool()
    {
        // Act - isChecked's `Value is bool boolValue` pattern match should fail gracefully
        // for a non-bool TValue rather than throwing.
        var cut = TestContext.Render<TwCheckbox<string>>(parameters => parameters
            .Add(p => p.Value, "not-a-bool"));

        // Assert
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    public void TwCheckbox_DoesNotInvokeCallback_WhenEventValueIsNotBool()
    {
        // Arrange - exercises the `e.Value is bool newValue` false branch of HandleChange.
        var callbackInvoked = false;
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<bool>(this, _ => callbackInvoked = true)));

        // Act
        var input = cut.Find("input");
        input.Change("not-a-bool");

        // Assert
        Assert.False(callbackInvoked);
    }

    [Fact]
    public void TwCheckbox_AppliesAriaInvalid_WhenInvalid()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Invalid, true)
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("true", input.GetAttribute("aria-invalid"));
    }

    [Fact]
    public void TwCheckbox_DoesNotApplyAriaInvalid_WhenNotInvalid()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Invalid, false)
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("aria-invalid"));
    }

    [Fact]
    public void TwCheckbox_SetsAriaDescribedBy_ToErrorId_WhenInvalidWithErrorMessage()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Id, "terms-checkbox")
            .Add(p => p.Invalid, true)
            .Add(p => p.ErrorMessage, "Required")
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("terms-checkbox-error", input.GetAttribute("aria-describedby"));

        var error = cut.Find("p[role='alert']");
        Assert.Equal("terms-checkbox-error", error.GetAttribute("id"));
        Assert.Contains("Required", error.TextContent);
    }

    [Fact]
    public void TwCheckbox_AriaDescribedBy_IsNull_WhenNotInvalid()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("aria-describedby"));
    }

    [Fact]
    public void TwCheckbox_SetsAriaLabel_WhenProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.AriaLabel, "Accept terms and conditions")
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("Accept terms and conditions", input.GetAttribute("aria-label"));
    }

    [Fact]
    public void TwCheckbox_SetsAriaLabelledBy_WhenProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckbox<bool>>(parameters => parameters
            .Add(p => p.AriaLabelledBy, "terms-label")
            .Add(p => p.Value, false));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("terms-label", input.GetAttribute("aria-labelledby"));
    }
}
