using Bunit;
using TwBlazor.Components;
using TwBlazor.Enums;

namespace TwBlazor.Tests.Components.Chip;

public class TwChipGroupTests : TwBlazorTestBase
{
    [Fact]
    public void TwChipGroup_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>();

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.NotNull(group);
        Assert.Contains("flex", group.GetAttribute("class"));
        Assert.Contains("flex-wrap", group.GetAttribute("class"));
        // Assert.Contains("gap-2", group.GetAttribute("class")); // TODO - readd this when spacing is added back in.
        Assert.Contains("justify-start", group.GetAttribute("class"));
    }

    [Fact]
    public void TwChipGroup_Renders_WithChildContent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.ChildContent, RenderFragmentBuilder("Test Content")));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("Test Content", group.TextContent);
    }

    [Theory]
    [InlineData("start", "justify-start")]
    [InlineData("center", "justify-center")]
    [InlineData("end", "justify-end")]
    public void TwChipGroup_Renders_WithAlignment(string alignment, string expectedClass)
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Alignment, alignment));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains(expectedClass, group.GetAttribute("class"));
    }

    [Fact]
    public void TwChipGroup_Renders_WithDefaultAlignment()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>();

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("justify-start", group.GetAttribute("class"));
    }

    [Fact]
    public void TwChipGroup_Renders_WithId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Id, "test-chipset"));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Equal("test-chipset", group.GetAttribute("id"));
    }

    [Fact]
    public void TwChipGroup_Renders_WithCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Class, "custom-class"));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("custom-class", group.GetAttribute("class"));
    }

    [Fact]
    public void TwChipGroup_Renders_WithLegend()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Legend, "Select Tags"));

        // Assert
        var legend = cut.Find("div[role='group'] > div");
        Assert.NotNull(legend);
        Assert.Contains("Select Tags", legend.TextContent);
    }

    [Fact]
    public void TwChipGroup_Legend_AppliesThemeClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Legend, "Select Tags"));

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
    public void TwChipGroup_Legend_AppliesCustomLegendClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Legend, "Select Tags")
            .Add(p => p.LegendClass, "custom-legend-class"));

        // Assert
        var legend = cut.Find("div[role='group'] > div");
        Assert.Contains("custom-legend-class", legend.GetAttribute("class"));
    }

    [Fact]
    public void TwChipGroup_DoesNotRender_Legend_WhenEmpty()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>();

        // Assert
        var group = cut.Find("div[role='group']");
        var legends = group.QuerySelectorAll(":scope > div");
        Assert.Empty(legends);
    }

    [Fact]
    public void TwChipGroup_DoesNotRender_Legend_WhenNull()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Legend, null));

        // Assert
        var group = cut.Find("div[role='group']");
        var legends = group.QuerySelectorAll(":scope > div");
        Assert.Empty(legends);
    }

    [Fact]
    public void TwChipGroup_Renders_WithStyle()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Style, "background-color: blue;"));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Equal("background-color: blue;", group.GetAttribute("style"));
    }

    [Fact]
    public void TwChipGroup_Renders_WithAriaLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.AriaLabel, "Chip collection"));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Equal("Chip collection", group.GetAttribute("aria-label"));
    }

    [Fact]
    public void TwChipGroup_Renders_WithAttributes()
    {
        // Arrange
        var attributes = new Dictionary<string, object>
        {
            { "data-test", "chipset" }
        };

        // Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Attributes, attributes));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Equal("chipset", group.GetAttribute("data-test"));
    }

    [Fact]
    public void TwChipGroup_Renders_WithMultipleChips()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<TwChip>(0);
                builder.AddAttribute(1, "Label", "Chip 1");
                builder.AddAttribute(2, "Color", Color.Primary);
                builder.CloseComponent();

                builder.OpenComponent<TwChip>(3);
                builder.AddAttribute(4, "Label", "Chip 2");
                builder.AddAttribute(5, "Color", Color.Success);
                builder.CloseComponent();

                builder.OpenComponent<TwChip>(6);
                builder.AddAttribute(7, "Label", "Chip 3");
                builder.AddAttribute(8, "Color", Color.Danger);
                builder.CloseComponent();
            }));

        // Assert
        var chips = cut.FindComponents<TwChip>();
        Assert.Equal(3, chips.Count);
        Assert.Equal("Chip 1", chips[0].Instance.Label);
        Assert.Equal("Chip 2", chips[1].Instance.Label);
        Assert.Equal("Chip 3", chips[2].Instance.Label);
    }

    [Theory]
    [InlineData("center")]
    [InlineData("end")]
    public void TwChipGroup_Renders_WithNonDefaultAlignment(string alignment)
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>(parameters => parameters
            .Add(p => p.Alignment, alignment));

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.DoesNotContain("justify-start", group.GetAttribute("class"));
    }

    [Fact]
    public void TwChipGroup_WrapsContent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChipGroup>();

        // Assert
        var group = cut.Find("div[role='group']");
        Assert.Contains("flex-wrap", group.GetAttribute("class"));
    }
}
