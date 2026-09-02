using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components;
using TwBlazor.Models;

namespace TwBlazor.Tests.Components.RadioButton;

public class TwRadioGroupTests : TwBlazorTestBase
{
    [Fact]
    public void TwRadioGroup_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>();

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.NotNull(fieldset);
        Assert.Contains("border-none", fieldset.GetAttribute("class"));
        Assert.Contains("flex-col", fieldset.GetAttribute("class"));
        // Assert.Contains("gap-2", fieldset.GetAttribute("class")); // TODO - readd this when spacing is added back in.
    }

    [Fact]
    public void TwRadioGroup_Renders_WithChildContent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.ChildContent, RenderFragmentBuilder("Test Content")));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("Test Content", fieldset.TextContent);
    }

    [Fact]
    public void TwRadioGroup_Renders_WithLegend()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Legend, "Select an option"));

        // Assert
        var legend = cut.Find("legend");
        Assert.NotNull(legend);
        Assert.Contains("Select an option", legend.TextContent);
        Assert.Contains("text-base", legend.GetAttribute("class"));
        Assert.Contains("font-medium", legend.GetAttribute("class"));
    }

    [Fact]
    public void TwRadioGroup_DoesNotRender_Legend_WhenEmpty()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>();

        // Assert
        var legends = cut.FindAll("legend");
        Assert.Empty(legends);
    }

    [Fact]
    public void TwRadioGroup_Legend_AppliesThemeClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Legend, "Select an option"));

        // Assert
        var legend = cut.Find("legend");
        var legendClass = legend.GetAttribute("class");
        Assert.Contains("text-base", legendClass);
        Assert.Contains("font-medium", legendClass);
        Assert.Contains("text-gray-700", legendClass);
        Assert.Contains("dark:text-gray-300", legendClass);
        Assert.Contains("mb-3", legendClass);
    }

    [Fact]
    public void TwRadioGroup_Legend_AppliesCustomLegendClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Legend, "Select an option")
            .Add(p => p.LegendClass, "custom-legend-class"));

        // Assert
        var legend = cut.Find("legend");
        Assert.Contains("custom-legend-class", legend.GetAttribute("class"));
    }

    [Fact]
    public void TwRadioGroup_DoesNotRender_Legend_WhenNull()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Legend, null));

        // Assert
        var legends = cut.FindAll("legend");
        Assert.Empty(legends);
    }

    [Fact]
    public void TwRadioGroup_Renders_AsHorizontal()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Horizontal, true));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("flex-row", fieldset.GetAttribute("class"));
        Assert.Contains("flex-wrap", fieldset.GetAttribute("class"));
        Assert.DoesNotContain("flex-col", fieldset.GetAttribute("class"));
    }

    [Fact]
    public void TwRadioGroup_Renders_AsVertical()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Horizontal, false));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("flex-col", fieldset.GetAttribute("class"));
        Assert.DoesNotContain("flex-row", fieldset.GetAttribute("class"));
    }

    [Fact]
    public void TwRadioGroup_Renders_AsDisabled()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Disabled, true));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("opacity-40", fieldset.GetAttribute("class"));
        Assert.Contains("pointer-events-none", fieldset.GetAttribute("class"));
    }

    [Fact]
    public void TwRadioGroup_Renders_WithId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Id, "test-radio-group"));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Equal("test-radio-group", fieldset.GetAttribute("id"));
    }

    [Fact]
    public void TwRadioGroup_Renders_WithCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Class, "custom-class"));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("custom-class", fieldset.GetAttribute("class"));
    }

    [Fact]
    public void TwRadioGroup_Renders_WithCustomLegendClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Legend, "Test Legend")
            .Add(p => p.LegendClass, "custom-legend-class"));

        // Assert
        var legend = cut.Find("legend");
        Assert.Contains("custom-legend-class", legend.GetAttribute("class"));
    }

    [Fact]
    public void TwRadioGroup_Renders_WithStyle()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Style, "margin-top: 20px;"));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Equal("margin-top: 20px;", fieldset.GetAttribute("style"));
    }

    [Fact]
    public void TwRadioGroup_Renders_WithAriaLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.AriaLabel, "Radio options"));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Equal("Radio options", fieldset.GetAttribute("aria-label"));
    }

    [Fact]
    public void TwRadioGroup_Renders_WithAttributes()
    {
        // Arrange
        var attributes = new Dictionary<string, object>
        {
            { "data-test", "radio-group" }
        };

        // Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Attributes, attributes));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Equal("radio-group", fieldset.GetAttribute("data-test"));
    }

    [Fact]
    public void TwRadioGroup_Renders_WithMultipleRadioButtons()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TwRadioButton<string>>(0);
                builder.AddAttribute(1, "Label", "Option 1");
                builder.AddAttribute(2, "Value", "opt1");
                builder.AddAttribute(3, "Name", "options");
                builder.CloseComponent();

                builder.OpenComponent<TwRadioButton<string>>(4);
                builder.AddAttribute(5, "Label", "Option 2");
                builder.AddAttribute(6, "Value", "opt2");
                builder.AddAttribute(7, "Name", "options");
                builder.CloseComponent();

                builder.OpenComponent<TwRadioButton<string>>(8);
                builder.AddAttribute(9, "Label", "Option 3");
                builder.AddAttribute(10, "Value", "opt3");
                builder.AddAttribute(11, "Name", "options");
                builder.CloseComponent();
            }));

        // Assert
        var radioButtons = cut.FindComponents<TwRadioButton<string>>();
        Assert.Equal(3, radioButtons.Count);
        Assert.Equal("Option 1", radioButtons[0].Instance.Label);
        Assert.Equal("Option 2", radioButtons[1].Instance.Label);
        Assert.Equal("Option 3", radioButtons[2].Instance.Label);
    }

    // Automatic mode tests (with Items)

    [Fact]
    public void TwRadioGroup_Renders_WithItems()
    {
        // Arrange
        List<RadioGroupItem<string>> items =
        [
            new() { Label = "Option 1", Value = "opt1" },
            new() { Label = "Option 2", Value = "opt2" },
            new() { Label = "Option 3", Value = "opt3" }
        ];

        // Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Name, "test-group"));

        // Assert
        var radioButtons = cut.FindComponents<TwRadioButton<string>>();
        Assert.Equal(3, radioButtons.Count);
    }

    [Fact]
    public void TwRadioGroup_BindsValue()
    {
        // Arrange
        List<RadioGroupItem<string>> items =
        [
            new() { Label = "Option 1", Value = "opt1" },
            new() { Label = "Option 2", Value = "opt2" },
            new() { Label = "Option 3", Value = "opt3" }
        ];
        var selectedValue = "opt2";

        // Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Name, "test-group")
            .Add(p => p.Value, selectedValue));

        // Assert
        var radioButtons = cut.FindComponents<TwRadioButton<string>>();
        Assert.Equal(3, radioButtons.Count);
        Assert.Equal(selectedValue, radioButtons[0].Instance.SelectedValue);
        Assert.Equal(selectedValue, radioButtons[1].Instance.SelectedValue);
        Assert.Equal(selectedValue, radioButtons[2].Instance.SelectedValue);
    }

    [Fact]
    public void TwRadioGroup_UpdatesValue_OnChange()
    {
        // Arrange
        List<RadioGroupItem<string>> items =
        [
            new() { Label = "Option 1", Value = "opt1" },
            new() { Label = "Option 2", Value = "opt2" }
        ];
        var selectedValue = "opt1";
        string? newValue = null;

        // Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Name, "test-group")
            .Add(p => p.Value, selectedValue)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(
                this, value => newValue = value)));

        var radioInputs = cut.FindAll("input[type='radio']");
        radioInputs[1].Change(true);

        // Assert
        Assert.NotNull(newValue);
        Assert.Equal("opt2", newValue);
    }

    [Fact]
    public void TwRadioGroup_Renders_WithChildContent_WhenNoItems()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.ChildContent, RenderFragmentBuilder("<div>Custom Content</div>")));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("Custom Content", fieldset.InnerHtml);
    }

    [Fact]
    public void TwRadioGroup_WorksWithEnums()
    {
        // Arrange
        List<RadioGroupItem<DayOfWeek>> items =
        [
            new() { Label = "Monday", Value = DayOfWeek.Monday },
            new() { Label = "Tuesday", Value = DayOfWeek.Tuesday }
        ];

        // Act
        var cut = TestContext.Render<TwRadioGroup<DayOfWeek>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Name, "days")
            .Add(p => p.Value, DayOfWeek.Monday));

        // Assert
        var radioButtons = cut.FindComponents<TwRadioButton<DayOfWeek>>();
        Assert.Equal(2, radioButtons.Count);
        Assert.Equal(DayOfWeek.Monday, radioButtons[0].Instance.SelectedValue);
    }

    [Fact]
    public void TwRadioGroup_WorksWithIntegers()
    {
        // Arrange
        List<RadioGroupItem<int>> items =
        [
            new() { Label = "One", Value = 1 },
            new() { Label = "Two", Value = 2 },
            new() { Label = "Three", Value = 3 }
        ];

        // Act
        var cut = TestContext.Render<TwRadioGroup<int>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Name, "numbers")
            .Add(p => p.Value, 2));

        // Assert
        var radioButtons = cut.FindComponents<TwRadioButton<int>>();
        Assert.Equal(3, radioButtons.Count);
        Assert.Equal(2, radioButtons[0].Instance.SelectedValue);
    }

    [Fact]
    public void TwRadioGroup_WorksWithComplexObjects()
    {
        // Arrange
        var obj1 = new TestObject { Id = 1, Name = "Test 1" };
        var obj2 = new TestObject { Id = 2, Name = "Test 2" };

        List<RadioGroupItem<TestObject>> items =
        [
            new() { Label = "Object 1", Value = obj1 },
            new() { Label = "Object 2", Value = obj2 }
        ];

        // Act
        var cut = TestContext.Render<TwRadioGroup<TestObject>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Name, "objects")
            .Add(p => p.Value, obj2));

        // Assert
        var radioButtons = cut.FindComponents<TwRadioButton<TestObject>>();
        Assert.Equal(2, radioButtons.Count);
        Assert.Equal(obj2, radioButtons[0].Instance.SelectedValue);
    }

    [Fact]
    public void TwRadioGroup_WithItems_Renders_AsDisabled()
    {
        // Arrange
        List<RadioGroupItem<string>> items =
        [
            new() { Label = "Option 1", Value = "opt1" }
        ];

        // Act
        var cut = TestContext.Render<TwRadioGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.Name, "test-group")
            .Add(p => p.Disabled, true));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("opacity-40", fieldset.GetAttribute("class"));
    }

    private class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public override bool Equals(object? obj) => obj is TestObject t && t.Id == Id;
        public override int GetHashCode() => Id.GetHashCode();
    }
}
