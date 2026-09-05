using Bunit;
using TwBlazor.Components;

namespace TwBlazor.Tests.Components.Button;

public class TwButtonGroupTests : TwBlazorTestBase
{
    [Fact]
    public void TwButtonGroup_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>();

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.NotNull(group);
        Assert.Contains("inline-flex", group.GetAttribute("class"));
        Assert.Contains("flex-row", group.GetAttribute("class"));
        // Assert.Contains("gap-2", group.GetAttribute("class")); // TODO - readd this when spacing is added back in.
    }

    [Fact]
    public void TwButtonGroup_Renders_WithChildContent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.ChildContent, RenderFragmentBuilder("<button>Test Button</button>")));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("Test Button", group.InnerHtml);
    }

    [Fact]
    public void TwButtonGroup_Renders_AsVertical()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Vertical, true));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("flex-col", group.GetAttribute("class"));
        Assert.DoesNotContain("flex-row", group.GetAttribute("class"));
    }

    [Fact]
    public void TwButtonGroup_Renders_AsHorizontal()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Vertical, false));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("flex-row", group.GetAttribute("class"));
        Assert.DoesNotContain("flex-col", group.GetAttribute("class"));
    }

    [Fact]
    public void TwButtonGroup_Renders_WithFullWidth()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.FullWidth, true));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("w-full", group.GetAttribute("class"));
    }

    [Fact]
    public void TwButtonGroup_Renders_WithFullWidthAndHorizontal()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.FullWidth, true)
            .Add(p => p.Vertical, false));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("w-full", group.GetAttribute("class"));
        Assert.Contains("[&>*]:flex-1", group.GetAttribute("class"));
    }

    [Fact]
    public void TwButtonGroup_Renders_WithId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Id, "test-button-group"));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Equal("test-button-group", group.GetAttribute("id"));
    }

    [Fact]
    public void TwButtonGroup_Renders_WithCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Class, "custom-class"));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("custom-class", group.GetAttribute("class"));
    }

    [Fact]
    public void TwButtonGroup_Renders_WithStyle()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Style, "background-color: red;"));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Equal("background-color: red;", group.GetAttribute("style"));
    }

    [Fact]
    public void TwButtonGroup_Renders_WithAriaLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.AriaLabel, "Button actions"));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Equal("Button actions", group.GetAttribute("aria-label"));
    }

    [Fact]
    public void TwButtonGroup_Renders_WithLegend()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Legend, "Action Buttons"));

        // Assert
        var legend = cut.Find("div[role='group'] > div");
        Assert.NotNull(legend);
        Assert.Contains("Action Buttons", legend.TextContent);
    }

    [Fact]
    public void TwButtonGroup_Legend_AppliesThemeClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Legend, "Action Buttons"));

        // Assert
        var legend = cut.Find("div[role='group'] > div");
        var legendClass = legend.GetAttribute("class");
        Assert.Contains("text-base", legendClass);
        Assert.Contains("font-medium", legendClass);
        Assert.Contains("text-gray-700", legendClass);
        Assert.Contains("dark:text-gray-300", legendClass);
        Assert.Contains("mb-3", legendClass);
    }

    [Fact]
    public void TwButtonGroup_Legend_AppliesCustomLegendClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Legend, "Action Buttons")
            .Add(p => p.LegendClass, "custom-legend-class"));

        // Assert
        var legend = cut.Find("div[role='group'] > div");
        Assert.Contains("custom-legend-class", legend.GetAttribute("class"));
    }

    [Fact]
    public void TwButtonGroup_DoesNotRender_Legend_WhenEmpty()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>();

        // Assert
        var group = cut.Find("div[role='group']");
        var legends = group.QuerySelectorAll(":scope > div");
        Assert.Empty(legends);
    }

    [Fact]
    public void TwButtonGroup_DoesNotRender_Legend_WhenNull()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Legend, null));

        // Assert
        var group = cut.Find("div[role='group']");
        var legends = group.QuerySelectorAll(":scope > div");
        Assert.Empty(legends);
    }

    [Fact]
    public void TwButtonGroup_Renders_WithAttributes()
    {
        // Arrange
        var attributes = new Dictionary<string, object>
        {
            { "data-test", "button-group" }
        };

        // Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.Attributes, attributes));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Equal("button-group", group.GetAttribute("data-test"));
    }

    [Fact]
    public void TwButtonGroup_Renders_WithMultipleButtons()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwButtonGroup>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TwButton>(0);
                builder.AddAttribute(1, "Label", "Button 1");
                builder.CloseComponent();

                builder.OpenComponent<TwButton>(2);
                builder.AddAttribute(3, "Label", "Button 2");
                builder.CloseComponent();

                builder.OpenComponent<TwButton>(4);
                builder.AddAttribute(5, "Label", "Button 3");
                builder.CloseComponent();
            }));

        // Assert
        var buttons = cut.FindComponents<TwButton>();
        Assert.Equal(3, buttons.Count);
        Assert.Equal("Button 1", buttons[0].Instance.Label);
        Assert.Equal("Button 2", buttons[1].Instance.Label);
        Assert.Equal("Button 3", buttons[2].Instance.Label);
    }
}
