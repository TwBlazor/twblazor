using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components;
using TwBlazor.Models;

namespace TwBlazor.Tests.Components.Checkbox;

public class TwCheckboxGroupTests : TwBlazorTestBase
{
    [Fact]
    public void TwCheckboxGroup_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>();

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.NotNull(fieldset);
        Assert.Contains("border-none", fieldset.GetAttribute("class"));
        Assert.Contains("flex-col", fieldset.GetAttribute("class"));
        // Assert.Contains("gap-2", fieldset.GetAttribute("class")); // TODO - readd this when spacing is added back in.
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithChildContent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.ChildContent, RenderFragmentBuilder("Test Content")));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("Test Content", fieldset.TextContent);
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithLegend()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Legend, "Select options"));

        // Assert
        var legend = cut.Find("legend");
        Assert.NotNull(legend);
        Assert.Contains("Select options", legend.TextContent);
        Assert.Contains("text-base", legend.GetAttribute("class"));
        Assert.Contains("font-medium", legend.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckboxGroup_DoesNotRender_Legend_WhenEmpty()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>();

        // Assert
        var legends = cut.FindAll("legend");
        Assert.Empty(legends);
    }

    [Fact]
    public void TwCheckboxGroup_Legend_AppliesThemeClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Legend, "Select options"));

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
    public void TwCheckboxGroup_Legend_AppliesCustomLegendClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Legend, "Select options")
            .Add(p => p.LegendClass, "custom-legend-class"));

        // Assert
        var legend = cut.Find("legend");
        Assert.Contains("custom-legend-class", legend.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckboxGroup_DoesNotRender_Legend_WhenNull()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Legend, null));

        // Assert
        var legends = cut.FindAll("legend");
        Assert.Empty(legends);
    }

    [Fact]
    public void TwCheckboxGroup_Renders_AsHorizontal()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Horizontal, true));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("flex-row", fieldset.GetAttribute("class"));
        Assert.Contains("flex-wrap", fieldset.GetAttribute("class"));
        Assert.DoesNotContain("flex-col", fieldset.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_AsVertical()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Horizontal, false));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("flex-col", fieldset.GetAttribute("class"));
        Assert.DoesNotContain("flex-row", fieldset.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_AsDisabled()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Disabled, true));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("opacity-40", fieldset.GetAttribute("class"));
        Assert.Contains("pointer-events-none", fieldset.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Id, "test-checkbox-group"));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Equal("test-checkbox-group", fieldset.GetAttribute("id"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Class, "custom-class"));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("custom-class", fieldset.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithCustomLegendClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Legend, "Test Legend")
            .Add(p => p.LegendClass, "custom-legend-class"));

        // Assert
        var legend = cut.Find("legend");
        Assert.Contains("custom-legend-class", legend.GetAttribute("class"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithStyle()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Style, "margin-top: 20px;"));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Equal("margin-top: 20px;", fieldset.GetAttribute("style"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithAriaLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.AriaLabel, "Checkbox options"));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Equal("Checkbox options", fieldset.GetAttribute("aria-label"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithAttributes()
    {
        // Arrange
        var attributes = new Dictionary<string, object>
        {
            { "data-test", "checkbox-group" }
        };

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Attributes, attributes));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Equal("checkbox-group", fieldset.GetAttribute("data-test"));
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithMultipleCheckboxes()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TwCheckbox<bool>>(0);
                builder.AddAttribute(1, "Label", "Option 1");
                builder.AddAttribute(2, "Value", false);
                builder.CloseComponent();

                builder.OpenComponent<TwCheckbox<bool>>(3);
                builder.AddAttribute(4, "Label", "Option 2");
                builder.AddAttribute(5, "Value", true);
                builder.CloseComponent();

                builder.OpenComponent<TwCheckbox<bool>>(6);
                builder.AddAttribute(7, "Label", "Option 3");
                builder.AddAttribute(8, "Value", false);
                builder.CloseComponent();
            }));

        // Assert
        var checkboxes = cut.FindComponents<TwCheckbox<bool>>();
        Assert.Equal(3, checkboxes.Count);
        Assert.Equal("Option 1", checkboxes[0].Instance.Label);
        Assert.Equal("Option 2", checkboxes[1].Instance.Label);
        Assert.Equal("Option 3", checkboxes[2].Instance.Label);
    }

    // Automatic mode tests (with Items)

    [Fact]
    public void TwCheckboxGroup_Renders_WithItems()
    {
        // Arrange
        List<CheckboxGroupItem<string>> items =
        [
            new() { Label = "Option 1", Value = "opt1", IsSelected = false },
            new() { Label = "Option 2", Value = "opt2", IsSelected = true },
            new() { Label = "Option 3", Value = "opt3", IsSelected = false }
        ];

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        var checkboxes = cut.FindComponents<TwCheckbox<bool>>();
        Assert.Equal(3, checkboxes.Count);
    }

    [Fact]
    public void TwCheckboxGroup_BindsSelectedValues()
    {
        // Arrange
        List<CheckboxGroupItem<string>> items =
        [
            new() { Label = "Option 1", Value = "opt1", IsSelected = true },
            new() { Label = "Option 2", Value = "opt2", IsSelected = false },
            new() { Label = "Option 3", Value = "opt3", IsSelected = true }
        ];
        List<string> selectedValues = ["opt1", "opt3"];

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.SelectedValues, selectedValues));

        // Assert
        var checkboxes = cut.FindComponents<TwCheckbox<bool>>();
        Assert.Equal(3, checkboxes.Count);
        Assert.True(checkboxes[0].Instance.Value);
        Assert.False(checkboxes[1].Instance.Value);
        Assert.True(checkboxes[2].Instance.Value);
    }

    [Fact]
    public void TwCheckboxGroup_UpdatesSelectedValues_OnChange()
    {
        // Arrange
        List<CheckboxGroupItem<string>> items =
        [
            new() { Label = "Option 1", Value = "opt1", IsSelected = false },
            new() { Label = "Option 2", Value = "opt2", IsSelected = false }
        ];
        List<string> selectedValues = [];
        IEnumerable<string>? newSelectedValues = null;

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.SelectedValues, selectedValues)
            .Add(p => p.SelectedValuesChanged, EventCallback.Factory.Create<IEnumerable<string>>(
                this, values => newSelectedValues = values)));

        var checkboxes = cut.FindAll("input[type='checkbox']");
        checkboxes[0].Change(true);

        // Assert
        Assert.NotNull(newSelectedValues);
        var newSelectedValue = Assert.Single(newSelectedValues);
        Assert.Equal("opt1", newSelectedValue);
    }

    [Fact]
    public void TwCheckboxGroup_Renders_WithChildContent_WhenNoItems()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.ChildContent, RenderFragmentBuilder("<div>Custom Content</div>")));

        // Assert
        var fieldset = cut.Find("fieldset");
        Assert.Contains("Custom Content", fieldset.InnerHtml);
    }

    [Fact]
    public void TwCheckboxGroup_WorksWithEnums()
    {
        // Arrange
        List<CheckboxGroupItem<DayOfWeek>> items =
        [
            new() { Label = "Monday", Value = DayOfWeek.Monday, IsSelected = true },
            new() { Label = "Tuesday", Value = DayOfWeek.Tuesday, IsSelected = false }
        ];

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<DayOfWeek>>(parameters => parameters
            .Add(p => p.Items, items));

        // Assert
        var checkboxes = cut.FindComponents<TwCheckbox<bool>>();
        Assert.Equal(2, checkboxes.Count);
    }

    [Fact]
    public void TwCheckboxGroup_WorksWithComplexObjects()
    {
        // Arrange
        var obj1 = new TestObject { Id = 1, Name = "Test 1" };
        var obj2 = new TestObject { Id = 2, Name = "Test 2" };

        List<CheckboxGroupItem<TestObject>> items =
        [
            new() { Label = "Object 1", Value = obj1, IsSelected = false },
            new() { Label = "Object 2", Value = obj2, IsSelected = true }
        ];

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<TestObject>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.SelectedValues, [obj2]));

        // Assert
        var checkboxes = cut.FindComponents<TwCheckbox<bool>>();
        Assert.Equal(2, checkboxes.Count);
        Assert.False(checkboxes[0].Instance.Value);
        Assert.True(checkboxes[1].Instance.Value);
    }

    [Fact]
    public void TwCheckboxGroup_WithItems_Renders_AsDisabled()
    {
        // Arrange
        List<CheckboxGroupItem<string>> items =
        [
            new() { Label = "Option 1", Value = "opt1" }
        ];

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
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
        public override int GetHashCode() => Id;
    }

    [Fact]
    public void TwCheckboxGroup_WithDefaultValueItem_IsNotSelected_WhenNotInSelectedValues()
    {
        // Arrange - item with a null/default value that is absent from SelectedValues
        // should stay unselected, same as any other value that isn't in the set.
        List<CheckboxGroupItem<string>> items =
        [
            new() { Label = "Empty", Value = default!, IsSelected = false },
            new() { Label = "Option 1", Value = "opt1", IsSelected = false }
        ];
        List<string> selectedValues = ["opt1"];

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.SelectedValues, selectedValues));

        // Assert
        var checkboxes = cut.FindComponents<TwCheckbox<bool>>();
        Assert.False(checkboxes[0].Instance.Value);
        Assert.True(checkboxes[1].Instance.Value);
    }

    [Fact]
    public void TwCheckboxGroup_WithDefaultValueItem_IsSelected_WhenInSelectedValues()
    {
        // Arrange - regression test for a bug where OnParametersSet unconditionally
        // treated any item whose Value equalled default(TValue) as unselectable, even when
        // that value was present in SelectedValues. DayOfWeek.Sunday is 0 (the enum's
        // default), so a "select weekend" action that included Sunday would set
        // SelectedValues correctly but the checkbox itself would never tick.
        List<CheckboxGroupItem<DayOfWeek>> items =
        [
            new() { Label = "Saturday", Value = DayOfWeek.Saturday, IsSelected = false },
            new() { Label = "Sunday", Value = DayOfWeek.Sunday, IsSelected = false }
        ];
        List<DayOfWeek> selectedValues = [DayOfWeek.Saturday, DayOfWeek.Sunday];

        // Act
        var cut = TestContext.Render<TwCheckboxGroup<DayOfWeek>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.SelectedValues, selectedValues));

        // Assert
        var checkboxes = cut.FindComponents<TwCheckbox<bool>>();
        Assert.True(checkboxes[0].Instance.Value); // Saturday
        Assert.True(checkboxes[1].Instance.Value); // Sunday == default(DayOfWeek) == 0
    }

    [Fact]
    public void TwCheckboxGroup_HandleItemChanged_SelectsDefaultValueItem()
    {
        // Arrange - same regression as above, but through the user-interaction path
        // (HandleItemChanged) rather than the SelectedValues-sync path.
        List<CheckboxGroupItem<DayOfWeek>> items =
        [
            new() { Label = "Sunday", Value = DayOfWeek.Sunday, IsSelected = false }
        ];
        IEnumerable<DayOfWeek>? newSelectedValues = null;

        var cut = TestContext.Render<TwCheckboxGroup<DayOfWeek>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.SelectedValuesChanged, EventCallback.Factory.Create<IEnumerable<DayOfWeek>>(
                this, values => newSelectedValues = values)));

        // Act
        cut.FindAll("input[type='checkbox']")[0].Change(true);

        // Assert
        Assert.NotNull(newSelectedValues);
        Assert.Contains(DayOfWeek.Sunday, newSelectedValues);
    }
}
