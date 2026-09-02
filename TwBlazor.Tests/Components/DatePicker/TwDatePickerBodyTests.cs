using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components.DatePicker;

namespace TwBlazor.Tests.Components.DatePicker;

public class TwDatePickerBodyTests : TwBlazorTestBase
{
    [Fact]
    public void TwDatePickerBody_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>();

        // Assert
        var outerDiv = cut.Find("div.datepicker-body");
        Assert.NotNull(outerDiv);
        Assert.Contains("flex", outerDiv.ClassList);

        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.NotNull(innerDiv);
        Assert.Contains("w-full", innerDiv.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_GeneratesId_WhenNotProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>();

        // Assert
        var outerDiv = cut.Find("div.datepicker-body");
        var id = outerDiv.GetAttribute("id");
        Assert.NotNull(id);
        Assert.StartsWith("datepickerbody-", id);
    }

    [Fact]
    public void TwDatePickerBody_UsesProvidedId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.Id, "custom-picker-body"));

        // Assert
        var outerDiv = cut.Find("div.datepicker-body");
        Assert.Equal("custom-picker-body", outerDiv.GetAttribute("id"));
    }

    [Fact]
    public void TwDatePickerBody_GeneratesUniqueIds_ForMultipleInstances()
    {
        // Arrange & Act
        var cut1 = TestContext.Render<TwDatePickerBody>();
        var cut2 = TestContext.Render<TwDatePickerBody>();

        // Assert
        var id1 = cut1.Find("div.datepicker-body").GetAttribute("id");
        var id2 = cut2.Find("div.datepicker-body").GetAttribute("id");
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void RendersChildContent_AndCombinesClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(p => p
            .Add(x => x.Class, "days")
            .AddChildContent("<div id='inner'>content</div>")
        );

        var div = cut.Find("#inner");
        var wrapper = cut.Find("div.datepicker-body > div");

        // Assert
        Assert.NotNull(div);
        Assert.Contains("days", wrapper.ClassList);
        Assert.Contains("w-full", wrapper.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_RendersChildContent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .AddChildContent("<span>Test Content</span>"));

        // Assert
        var span = cut.Find("span");
        Assert.NotNull(span);
        Assert.Equal("Test Content", span.TextContent);
    }

    [Fact]
    public void TwDatePickerBody_RendersWithoutChildContent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>();

        // Assert
        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.NotNull(innerDiv);
        Assert.Empty(innerDiv.Children);
    }

    [Fact]
    public void TwDatePickerBody_AppliesCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.Class, "custom-datepicker-class"));

        // Assert
        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.Contains("custom-datepicker-class", innerDiv.ClassList);
        Assert.Contains("w-full", innerDiv.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_AppliesOnlyDefaultClass_WhenNoCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>();

        // Assert
        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.Contains("w-full", innerDiv.ClassList);
        Assert.Single(innerDiv.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_AppliesNullClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.Class, null));

        // Assert
        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.Contains("w-full", innerDiv.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_AppliesEmptyClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.Class, string.Empty));

        // Assert
        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.Contains("w-full", innerDiv.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_HasCorrectStructure()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>();

        // Assert
        var outerDiv = cut.Find("div.datepicker-body");
        Assert.Contains("flex", outerDiv.ClassList);

        var innerDiv = outerDiv.QuerySelector("div");
        Assert.NotNull(innerDiv);
        Assert.Contains("w-full", innerDiv.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_RendersComplexChildContent()
    {
        // Arrange
        var childContent = @"
            <div class='calendar-grid'>
                <div class='day'>1</div>
                <div class='day'>2</div>
                <div class='day'>3</div>
            </div>";

        // Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .AddChildContent(childContent));

        // Assert
        var grid = cut.Find(".calendar-grid");
        Assert.NotNull(grid);
        var days = cut.FindAll(".day");
        Assert.Equal(3, days.Count);
    }

    [Fact]
    public void TwDatePickerBody_CombinesMultipleClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.Class, "days months years"));

        // Assert
        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.Contains("days", innerDiv.ClassList);
        Assert.Contains("months", innerDiv.ClassList);
        Assert.Contains("years", innerDiv.ClassList);
        Assert.Contains("w-full", innerDiv.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_InheritsFromTwBlazorComponentBase()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>();

        // Assert
        Assert.IsType<TwBlazorComponentBase>(cut.Instance, exactMatch: false);
    }

    [Fact]
    public void TwDatePickerBody_SupportsAriaLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.AriaLabel, "Date picker body"));

        // Assert - Component accepts the property without error
        var outerDiv = cut.Find("div.datepicker-body");
        Assert.NotNull(outerDiv);
    }

    [Fact]
    public void TwDatePickerBody_RendersWithAllProperties()
    {
        // Arrange
        var customId = "my-picker-body";
        var customClass = "custom-body-class";
        var childContent = "<span>Body Content</span>";

        // Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.Id, customId)
            .Add(p => p.Class, customClass)
            .AddChildContent(childContent));

        // Assert
        var outerDiv = cut.Find("div.datepicker-body");
        Assert.Equal(customId, outerDiv.GetAttribute("id"));

        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.Contains(customClass, innerDiv.ClassList);
        Assert.Contains("w-full", innerDiv.ClassList);

        var span = cut.Find("span");
        Assert.Equal("Body Content", span.TextContent);
    }

    [Fact]
    public void TwDatePickerBody_OuterDiv_HasCorrectClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>();

        // Assert
        var outerDiv = cut.Find("div.datepicker-body");
        Assert.Contains("datepicker-body", outerDiv.ClassList);
        Assert.Contains("flex", outerDiv.ClassList);
        Assert.Equal(2, outerDiv.ClassList.Length);
    }

    [Fact]
    public void TwDatePickerBody_InnerDiv_AlwaysHasWFullClass()
    {
        // Arrange - Test with various scenarios
        var scenarios = new[]
        {
            null,
            string.Empty,
            "  ",
            "custom-class",
            "multiple custom classes"
        };

        foreach (var classValue in scenarios)
        {
            // Act
            var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
                .Add(p => p.Class, classValue));

            // Assert
            var innerDiv = cut.Find("div.datepicker-body > div");
            Assert.Contains("w-full", innerDiv.ClassList);
        }
    }

    [Fact]
    public void TwDatePickerBody_RendersNestedComponents()
    {
        // Arrange
        RenderFragment nestedContent = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "nested");
            builder.AddContent(2, "Nested Component");
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.ChildContent, nestedContent));

        // Assert
        var nested = cut.Find(".nested");
        Assert.NotNull(nested);
        Assert.Equal("Nested Component", nested.TextContent);
    }

    [Theory]
    [InlineData("days")]
    [InlineData("months")]
    [InlineData("years")]
    [InlineData("decade")]
    public void TwDatePickerBody_AppliesViewSpecificClasses(string viewClass)
    {
        // Arrange & Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .Add(p => p.Class, viewClass));

        // Assert
        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.Contains(viewClass, innerDiv.ClassList);
        Assert.Contains("w-full", innerDiv.ClassList);
    }

    [Fact]
    public void TwDatePickerBody_PreservesWhitespace_InChildContent()
    {
        // Arrange
        var contentWithSpaces = "  Content with spaces  ";

        // Act
        var cut = TestContext.Render<TwDatePickerBody>(parameters => parameters
            .AddChildContent(contentWithSpaces));

        // Assert
        var innerDiv = cut.Find("div.datepicker-body > div");
        Assert.Contains("spaces", innerDiv.TextContent);
    }
}
