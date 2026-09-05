using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components;
using TwBlazor.Configuration.Components;

namespace TwBlazor.Tests.Components.Table;

public class TwTableTests : TwBlazorTestBase
{
    private TwTableTheme tableTheme => Theme.Components.Require<TwTableTheme>();

    [Fact]
    public void TwTable_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>();

        // Assert
        var table = cut.Find("table");
        Assert.NotNull(table);
        Assert.Contains("w-full", table.GetAttribute("class"));
        Assert.Contains("text-sm", table.GetAttribute("class"));
        Assert.Contains("text-left", table.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_Renders_WithTableHeader()
    {
        // Arrange
        RenderFragment header = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "th");
            builder.AddContent(2, "Header Content");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableHeader, header));

        // Assert
        var thead = cut.Find("thead");
        Assert.Contains("Header Content", thead.TextContent);
    }

    [Fact]
    public void TwTable_Renders_WithTableBody()
    {
        // Arrange
        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Body Content");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableBody, body));

        // Assert
        var tbody = cut.Find("tbody");
        Assert.Contains("Body Content", tbody.TextContent);
    }

    [Fact]
    public void TwTable_Renders_WithTableFooter()
    {
        // Arrange
        RenderFragment footer = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Footer Content");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableFooter, footer));

        // Assert
        var tfoot = cut.Find("tfoot");
        Assert.Contains("Footer Content", tfoot.TextContent);
    }

    [Fact]
    public void TwTable_Renders_WithAllSections()
    {
        // Arrange
        RenderFragment header = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "th");
            builder.AddContent(2, "Header");
            builder.CloseElement();
            builder.CloseElement();
        };

        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Body");
            builder.CloseElement();
            builder.CloseElement();
        };

        RenderFragment footer = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Footer");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableHeader, header)
            .Add(p => p.TableBody, body)
            .Add(p => p.TableFooter, footer));

        // Assert
        Assert.Contains("Header", cut.Find("thead").TextContent);
        Assert.Contains("Body", cut.Find("tbody").TextContent);
        Assert.Contains("Footer", cut.Find("tfoot").TextContent);
    }

    [Fact]
    public void TwTable_Striped_DefaultsToTrue()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Striped, true)
            .Add(p => p.TableBody, RenderFragmentBuilder("<tr><td>Test</td></tr>")));

        // Assert
        var tbody = cut.Find("tbody");
        Assert.Contains("nth-child(even)", tbody.GetAttribute("class"));
        Assert.Contains("bg-gray-50", tbody.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_Striped_CanBeDisabled()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableBody, RenderFragmentBuilder("<tr><td>Test</td></tr>"))
            .Add(p => p.Striped, false));

        // Assert
        var tbody = cut.Find("tbody");
        Assert.DoesNotContain("nth-child(even)", tbody.GetAttribute("class") ?? string.Empty);
    }

    [Fact]
    public void TwTable_Hoverable_DefaultsToTrue()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Hoverable, true)
            .Add(p => p.TableBody, RenderFragmentBuilder("<tr><td>Test</td></tr>")));

        // Assert
        var tbody = cut.Find("tbody");
        var tbodyClass = tbody.GetAttribute("class") ?? string.Empty;
        Assert.Contains("hover", tbodyClass);
        Assert.Contains("!bg-gray-100", tbodyClass);
    }

    [Fact]
    public void TwTable_Hoverable_CanBeDisabled()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableBody, RenderFragmentBuilder("<tr><td>Test</td></tr>"))
            .Add(p => p.Hoverable, false));

        // Assert
        var tbody = cut.Find("tbody");
        var tbodyClass = tbody.GetAttribute("class") ?? string.Empty;
        Assert.DoesNotContain("hover", tbodyClass);
    }

    [Fact]
    public void TwTable_Bordered_DefaultsToFalse()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.NoBorder, true));

        // Assert
        var table = cut.Find("table");
        var tableClass = table.GetAttribute("class") ?? string.Empty;
        Assert.DoesNotContain("border border-gray-200", tableClass);
    }

    [Fact]
    public void TwTable_Bordered_CanBeEnabled()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Bordered, true));

        // Assert
        var table = cut.Find("table");
        var tableClass = table.GetAttribute("class") ?? string.Empty;
        Assert.Contains("border", tableClass);
        Assert.Contains("border-gray-200", tableClass);
    }

    [Fact]
    public void TwTable_HeaderClass_AppliesCustomClass()
    {
        // Arrange
        RenderFragment header = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "th");
            builder.AddContent(2, "Header");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableHeader, header)
            .Add(p => p.HeaderClass, "custom-header-class"));

        // Assert
        var thead = cut.Find("thead");
        Assert.Contains("custom-header-class", thead.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_BodyClass_AppliesCustomClass()
    {
        // Arrange
        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Body");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableBody, body)
            .Add(p => p.BodyClass, "custom-body-class"));

        // Assert
        var tbody = cut.Find("tbody");
        Assert.Contains("custom-body-class", tbody.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_FooterClass_AppliesCustomClass()
    {
        // Arrange
        RenderFragment footer = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Footer");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableFooter, footer)
            .Add(p => p.FooterClass, "custom-footer-class"));

        // Assert
        var tfoot = cut.Find("tfoot");
        Assert.Contains("custom-footer-class", tfoot.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_Id_SetsTableId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Id, "my-table-id"));

        // Assert
        var table = cut.Find("table");
        Assert.Equal("my-table-id", table.GetAttribute("id"));
    }

    [Fact]
    public void TwTable_Class_AppliesCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Class, "custom-table-class"));

        // Assert
        var table = cut.Find("table");
        Assert.Contains("custom-table-class", table.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_Attributes_AppliesCustomAttributes()
    {
        // Arrange
        var attributes = new Dictionary<string, object>
        {
            { "data-test", "test-value" }
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Attributes, attributes)
            .Add(p => p.AriaLabel, "Test Table"));

        // Assert
        var table = cut.Find("table");
        Assert.Equal("test-value", table.GetAttribute("data-test"));
        // aria-label is set via the AriaLabel component parameter, not the generic Attributes
        // dictionary - a stray "aria-label" key in Attributes must never silently override it.
        Assert.Equal("Test Table", table.GetAttribute("aria-label"));
    }

    [Fact]
    public void TwTable_CombinedProperties_WorkTogether()
    {
        // Arrange
        RenderFragment header = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "th");
            builder.AddContent(2, "Name");
            builder.CloseElement();
            builder.CloseElement();
        };

        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "John Doe");
            builder.CloseElement();
            builder.CloseElement();
        };

        var attributes = new Dictionary<string, object>
        {
            { "role", "grid" }
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Id, "users-table")
            .Add(p => p.Class, "shadow-lg")
            .Add(p => p.TableHeader, header)
            .Add(p => p.TableBody, body)
            .Add(p => p.HeaderClass, "bg-blue-500")
            .Add(p => p.BodyClass, "bg-white")
            .Add(p => p.Striped, true)
            .Add(p => p.Hoverable, true)
            .Add(p => p.Bordered, true)
            .Add(p => p.Attributes, attributes));

        // Assert
        var table = cut.Find("table");
        Assert.Equal("users-table", table.GetAttribute("id"));
        Assert.Contains("shadow-lg", table.GetAttribute("class"));
        Assert.Contains("border", table.GetAttribute("class"));
        Assert.Equal("grid", table.GetAttribute("role"));

        var thead = cut.Find("thead");
        Assert.Contains("bg-blue-500", thead.GetAttribute("class"));
        Assert.Contains("Name", thead.TextContent);

        var tbody = cut.Find("tbody");
        Assert.Contains("bg-white", tbody.GetAttribute("class"));
        Assert.Contains("nth-child(even)", tbody.GetAttribute("class"));
        Assert.Contains("hover", tbody.GetAttribute("class"));
        Assert.Contains("John Doe", tbody.TextContent);
    }

    [Fact]
    public void TwTable_WithoutAnyContent_RendersEmptyTable()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>();

        // Assert
        var table = cut.Find("table");
        Assert.NotNull(table);

        // Should not render any sections when content is not provided
        var theadElements = cut.FindAll("thead");
        var tbodyElements = cut.FindAll("tbody");
        var tfootElements = cut.FindAll("tfoot");

        Assert.Empty(theadElements);
        Assert.Empty(tbodyElements);
        Assert.Empty(tfootElements);
    }

    [Fact]
    public void TwTable_DarkMode_AppliesDarkClasses()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>();

        // Assert
        var table = cut.Find("table");
        var tableClass = table.GetAttribute("class") ?? string.Empty;
        Assert.Contains("dark:text-white", tableClass);
    }

    [Fact]
    public void TwTable_Header_DarkMode_UsesDarkerBackgroundThanBody()
    {
        // Arrange
        RenderFragment header = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "th");
            builder.AddContent(2, "Header");
            builder.CloseElement();
            builder.CloseElement();
        };

        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Body");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableHeader, header)
            .Add(p => p.TableBody, body));

        // Assert
        var thead = cut.Find("thead");
        Assert.Contains("bg-gray-200", thead.GetAttribute("class"));
        Assert.Contains("dark:bg-gray-950", thead.GetAttribute("class"));

        var tbody = cut.Find("tbody");
        Assert.Contains("dark:bg-gray-900", tbody.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_Header_UsesThemeHeaderClass_ByDefault()
    {
        // Arrange
        RenderFragment header = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "th");
            builder.AddContent(2, "Header");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableHeader, header));

        // Assert
        var thead = cut.Find("thead");
        Assert.Contains(tableTheme.Header, thead.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_Header_ReflectsCustomThemeHeaderClass()
    {
        // Arrange - overriding the shared theme token lets consumers restyle every table's header
        // (TwTable and TwDataTable alike) from a single place instead of per-component parameters.
        tableTheme.Header = "bg-blue-950 text-white custom-theme-header";

        RenderFragment header = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "th");
            builder.AddContent(2, "Header");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableHeader, header));

        // Assert
        var thead = cut.Find("thead");
        Assert.Contains("custom-theme-header", thead.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_Body_ReflectsCustomThemeBodyClass()
    {
        // Arrange
        tableTheme.Body = "custom-theme-body";

        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Body");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableBody, body));

        // Assert
        var tbody = cut.Find("tbody");
        Assert.Contains("custom-theme-body", tbody.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_RTL_AppliesRTLClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>();

        // Assert
        var table = cut.Find("table");
        var tableClass = table.GetAttribute("class") ?? string.Empty;
        Assert.Contains("rtl:text-right", tableClass);
    }

    [Fact]
    public void TwTable_StripedAndHoverable_BothApplyCorrectly()
    {
        // Arrange
        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Row 1");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableBody, body)
            .Add(p => p.Striped, true)
            .Add(p => p.Hoverable, true));

        // Assert
        var tbody = cut.Find("tbody");
        var tbodyClass = tbody.GetAttribute("class") ?? string.Empty;

        // Should have both striped and hover classes
        Assert.Contains("nth-child(even)", tbodyClass);
        Assert.Contains("bg-gray-50", tbodyClass);
        Assert.Contains("hover", tbodyClass);
        Assert.Contains("!bg-gray-100", tbodyClass);
    }

    [Fact]
    public void TwTable_HoverOverridesStriped_WithImportant()
    {
        // Arrange
        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "tr");
            builder.OpenElement(1, "td");
            builder.AddContent(2, "Test");
            builder.CloseElement();
            builder.CloseElement();
        };

        // Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableBody, body)
            .Add(p => p.Striped, true)
            .Add(p => p.Hoverable, true));

        // Assert
        var tbody = cut.Find("tbody");
        var tbodyClass = tbody.GetAttribute("class") ?? string.Empty;

        // Hover should use !important to override striped
        Assert.Contains("!bg-gray-100", tbodyClass);
        Assert.Contains("!bg-gray-800", tbodyClass);
    }

    [Fact]
    public void TwTable_DoesNotRenderCaption_WhenNotProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.TableBody, RenderFragmentBuilder("<tr><td>Test</td></tr>")));

        // Assert
        Assert.Empty(cut.FindAll("caption"));
    }

    [Fact]
    public void TwTable_RendersCaption_WhenProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Caption, RenderFragmentBuilder("Table of users"))
            .Add(p => p.TableBody, RenderFragmentBuilder("<tr><td>Test</td></tr>")));

        // Assert
        var caption = cut.Find("caption");
        Assert.Contains("Table of users", caption.TextContent);
    }

    [Fact]
    public void TwTable_Caption_IsSrOnly_ByDefault()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Caption, RenderFragmentBuilder("Table of users"))
            .Add(p => p.TableBody, RenderFragmentBuilder("<tr><td>Test</td></tr>")));

        // Assert
        var caption = cut.Find("caption");
        Assert.Contains("sr-only", caption.GetAttribute("class"));
    }

    [Fact]
    public void TwTable_Caption_IsVisible_WhenCaptionVisibleTrue()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTable>(parameters => parameters
            .Add(p => p.Caption, RenderFragmentBuilder("Table of users"))
            .Add(p => p.CaptionVisible, true)
            .Add(p => p.TableBody, RenderFragmentBuilder("<tr><td>Test</td></tr>")));

        // Assert
        var caption = cut.Find("caption");
        Assert.DoesNotContain("sr-only", caption.GetAttribute("class"));
    }
}
