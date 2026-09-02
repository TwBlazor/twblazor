using Bunit;
using TwBlazor.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;

namespace TwBlazor.Tests.Components.Textfield;

public class TwTextfieldTests : TwBlazorTestBase
{
    private TwInputTheme inputTheme => Theme.Components.Require<TwInputTheme>();

    [Fact]
    public void TwTextfield_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>();

        // Assert
        var input = cut.Find("input");
        Assert.NotNull(input);
        Assert.Equal("text", input.GetAttribute("type"));
        Assert.Empty(input.GetAttribute("placeholder") ?? string.Empty);
    }

    [Fact]
    public void TwTextfield_Renders_WithLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Label, "Username"));

        // Assert
        var label = cut.Find("label");
        Assert.NotNull(label);
        Assert.Equal("Username", label.TextContent);
        Assert.Contains("block mb-2 text-xs font-normal tracking-wide", label.GetAttribute("class"));
    }

    [Fact]
    public void TwTextfield_DoesNotRender_LabelWhenEmpty()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Label, string.Empty));

        // Assert
        Assert.Throws<ElementNotFoundException>(() => cut.Find("label"));
    }

    [Fact]
    public void TwTextfield_Renders_WithPlaceholder()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Placeholder, "Enter your email"));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("Enter your email", input.GetAttribute("placeholder"));
    }

    [Fact]
    public void TwTextfield_Renders_WithCustomInputType()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.InputType, "password"));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("password", input.GetAttribute("type"));
    }

    [Fact]
    public void TwTextfield_Renders_WithEmailType()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.InputType, "email"));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("email", input.GetAttribute("type"));
    }

    [Fact]
    public void TwTextfield_Renders_WithNumberType()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<int>>(parameters => parameters
            .Add(p => p.InputType, "number")
            .Add(p => p.Value, 42));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("number", input.GetAttribute("type"));
    }

    [Fact]
    public void TwTextfield_Renders_WithId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Id, "custom-id"));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("custom-id", input.GetAttribute("id"));
    }

    [Fact]
    public void TwTextfield_Renders_WithLabelAndId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Id, "username-input")
            .Add(p => p.Label, "Username"));

        // Assert
        var label = cut.Find("label");
        var input = cut.Find("input");
        Assert.Equal("username-input", label.GetAttribute("for"));
        Assert.Equal("username-input", input.GetAttribute("id"));
    }

    [Fact]
    public void TwTextfield_Renders_WithCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Class, "custom-class"));

        // Assert
        var input = cut.Find("input");
        Assert.Contains("custom-class", input.GetAttribute("class"));
        Assert.Contains(inputTheme.TextfieldBase, input.GetAttribute("class")); // Default class should still be present
    }

    [Fact]
    public void TwTextfield_Renders_WithCustomLabelClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Label, "Email")
            .Add(p => p.LabelClass, "text-blue-600"));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("text-blue-600", label.GetAttribute("class"));
        Assert.Contains(inputTheme.LabelBase, label.GetAttribute("class")); // Default class should still be present
    }

    [Fact]
    public void TwTextfield_Renders_WithInitialValue()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Value, "Initial Value"));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("Initial Value", input.GetAttribute("value"));
    }

    [Fact]
    public void TwTextfield_TriggersValueChanged_OnInput()
    {
        // Arrange
        var newValue = string.Empty;
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Value, "")
            .Add(p => p.ValueChanged, value => newValue = value));

        // Act
        var input = cut.Find("input");
        input.Change("Test Input");

        // Assert
        Assert.Equal("Test Input", newValue);
    }

    [Fact]
    public void TwTextfield_TriggersValueChanged_WithIntType()
    {
        // Arrange
        var newValue = 0;
        var cut = TestContext.Render<TwTextfield<int>>(parameters => parameters
            .Add(p => p.InputType, "number")
            .Add(p => p.Value, 0)
            .Add(p => p.ValueChanged, value => newValue = value));

        // Act
        var input = cut.Find("input");
        input.Change(123);

        // Assert
        Assert.Equal(123, newValue);
    }

    [Fact]
    public void TwTextfield_UpdatesValue_OnChange()
    {
        // Arrange
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Value, "Old Value")
            .Add(p => p.ValueChanged, _ => { }));

        // Act
        var input = cut.Find("input");
        input.Change("New Value");

        // Assert
        Assert.Equal("New Value", input.GetAttribute("value"));
    }

    [Fact]
    public void TwTextfield_Renders_WithCustomBindEvent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.BindEvent, "oninput")
            .Add(p => p.Value, "")
            .Add(p => p.ValueChanged, _ => { }));

        // Assert - This tests that the component accepts the parameter without error
        var input = cut.Find("input");
        Assert.NotNull(input);
    }

    [Fact]
    public void TwTextfield_Renders_WithAttributes()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Attributes, new Dictionary<string, object>
            {
                { "data-test", "test-value" },
                { "disabled", true }
            }));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("test-value", input.GetAttribute("data-test"));
        Assert.True(input.HasAttribute("disabled"));
    }

    [Fact]
    public void TwTextfield_Renders_WithMultipleParameters()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Id, "full-example")
            .Add(p => p.Label, "Full Name")
            .Add(p => p.Placeholder, "Enter your full name")
            .Add(p => p.Value, "John Doe")
            .Add(p => p.Class, "rounded-lg")
            .Add(p => p.LabelClass, "font-bold"));

        // Assert
        var label = cut.Find("label");
        var input = cut.Find("input");

        Assert.Equal("Full Name", label.TextContent);
        Assert.Contains("font-bold", label.GetAttribute("class"));
        Assert.Equal("full-example", input.GetAttribute("id"));
        Assert.Equal("Enter your full name", input.GetAttribute("placeholder"));
        Assert.Equal("John Doe", input.GetAttribute("value"));
        Assert.Contains("rounded-lg", input.GetAttribute("class"));
    }

    [Fact]
    public void TwTextfield_Renders_WithDecimalType()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<decimal>>(parameters => parameters
            .Add(p => p.InputType, "number")
            .Add(p => p.Value, 99.99m));

        // Assert
        var input = cut.Find("input");
        Assert.NotNull(input);
    }

    [Fact]
    public void TwTextfield_HasDefaultClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>();

        // Assert
        var input = cut.Find("input");
        var classes = input.GetAttribute("class");

        Assert.Contains(inputTheme.TextfieldBase, classes);
    }

    [Fact]
    public void TwTextfield_DoesNotTriggerValueChanged_WhenNotProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Value, "Initial"));

        var input = cut.Find("input");

        // Act - Should not throw exception
        var exception = Record.Exception(() => input.Change("Changed"));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void TwTextfield_GeneratesId_WhenNotProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>();

        // Assert
        var input = cut.Find("input");
        var id = input.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("textfield-", id);
        Assert.DoesNotContain("`", id); // Should not contain generic type indicator
    }

    [Fact]
    public void TwTextfield_GeneratesUniqueIds_ForMultipleInstances()
    {
        // Arrange & Act
        var cut1 = TestContext.Render<TwTextfield<string>>();
        var cut2 = TestContext.Render<TwTextfield<string>>();

        // Assert
        var id1 = cut1.Find("input").GetAttribute("id");
        var id2 = cut2.Find("input").GetAttribute("id");
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void TwTextfield_AssociatesLabelWithGeneratedId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Label, "Email"));

        // Assert
        var label = cut.Find("label");
        var input = cut.Find("input");
        var inputId = input.GetAttribute("id");
        var labelFor = label.GetAttribute("for");

        Assert.NotNull(inputId);
        Assert.Equal(inputId, labelFor);
        Assert.StartsWith("textfield-", inputId);
    }

    [Fact]
    public void TwTextfield_Renders_WithDisabled()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Disabled, true));

        // Assert
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("disabled"));
        Assert.Contains("opacity-40 cursor-not-allowed", input.GetAttribute("class"));
    }

    [Fact]
    public void TwTextfield_Renders_WithoutDisabled_WhenFalse()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Disabled, false));

        // Assert
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("disabled"));
    }

    [Fact]
    public void TwTextfield_Renders_WithReadonly()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.ReadOnly, true));

        // Assert
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("readonly"));
    }

    [Fact]
    public void TwTextfield_Renders_WithoutReadonly_WhenFalse()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.ReadOnly, false));

        // Assert
        var input = cut.Find("input");
        Assert.False(input.HasAttribute("readonly"));
    }

    [Fact]
    public void TwTextfield_Renders_WithBothDisabledAndReadonly()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.ReadOnly, true));

        // Assert
        var input = cut.Find("input");
        Assert.True(input.HasAttribute("disabled"));
        Assert.True(input.HasAttribute("readonly"));
    }

    [Theory]
    [InlineData("tel")]
    [InlineData("url")]
    public void TwTextfield_SetsAutocompleteAndInputMode_ForTelAndUrlTypes(string inputType)
    {
        // Arrange & Act - GetAutoCompleteForInputType and GetInputModeForInputType both have a
        // dedicated "tel"/"url" branch (in addition to the already-covered "email" and "number" ones).
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.InputType, inputType));

        // Assert
        var input = cut.Find("input");
        Assert.Equal(inputType, input.GetAttribute("autocomplete"));
        Assert.Equal(inputType, input.GetAttribute("inputmode"));
    }

    [Fact]
    public void TwTextfield_SetsInputMode_ForSearchType_WithoutAutocomplete()
    {
        // Arrange & Act - "search" has a branch in GetInputModeForInputType but not in
        // GetAutoCompleteForInputType (which falls through to its default null case for it).
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.InputType, "search"));

        // Assert
        var input = cut.Find("input");
        Assert.Equal("search", input.GetAttribute("inputmode"));
        Assert.False(input.HasAttribute("autocomplete"));
    }

    [Fact]
    public void TwTextfield_UsesGlobalDefaultVariant_WhenNotSet()
    {
        // Arrange - no Variant set on the component, so it must follow TwInputTheme.DefaultInputVariant
        // (inherited via TwBlazorInputComponentBase.effectiveVariant).
        inputTheme.DefaultInputVariant = InputVariant.Outlined;

        // Act
        var cut = TestContext.Render<TwTextfield<string>>();

        // Assert
        var classes = cut.Find("input").GetAttribute("class");
        Assert.Contains(InputVariantBuilder.GetClasses(InputVariant.Outlined, inputTheme), classes);
    }

    [Fact]
    public void TwTextfield_ExplicitVariant_OverridesGlobalDefault()
    {
        // Arrange - the global default is Outlined, but this instance explicitly asks for Filled.
        inputTheme.DefaultInputVariant = InputVariant.Outlined;

        // Act
        var cut = TestContext.Render<TwTextfield<string>>(parameters => parameters
            .Add(p => p.Variant, InputVariant.Filled));

        // Assert
        var classes = cut.Find("input").GetAttribute("class");
        Assert.Contains(InputVariantBuilder.GetClasses(InputVariant.Filled, inputTheme), classes);
    }
}
