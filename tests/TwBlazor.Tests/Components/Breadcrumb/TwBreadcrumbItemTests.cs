using Bunit;
using TwBlazor.Components;
using TwBlazor.Models;
using Icons = TwBlazor.Enums.Icon;

namespace TwBlazor.Tests.Components.Breadcrumb;

public class TwBreadcrumbItemTests : TwBlazorTestBase
{
    [Fact]
    public void TwBreadcrumbItem_Renders_LinkWithLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Label, "Home")
            .Add(x => x.Href, "/"));

        // Assert
        var link = cut.Find("a");
        Assert.Equal("Home", link.TextContent.Trim());
        Assert.Equal("/", link.GetAttribute("href"));
    }

    [Fact]
    public void TwBreadcrumbItem_Renders_InsideLiElement()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Label, "Home")
            .Add(x => x.Href, "/"));

        // Assert
        var li = cut.Find("li");
        Assert.NotNull(li);
    }

    [Fact]
    public void TwBreadcrumbItem_Renders_AriaCurrent_WhenSet()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Label, "Current Page")
            .Add(x => x.Href, "/current")
            .Add(x => x.AriaCurrent, true));

        // Assert
        var span = cut.Find("[aria-current='page']");
        Assert.NotNull(span);
        Assert.Contains("Current Page", span.TextContent);
    }

    [Fact]
    public void TwBreadcrumbItem_DoesNotRender_Link_WhenAriaCurrent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Label, "Current Page")
            .Add(x => x.Href, "/current")
            .Add(x => x.AriaCurrent, true));

        // Assert
        var links = cut.FindAll("a");
        Assert.Empty(links);
    }

    [Fact]
    public void TwBreadcrumbItem_Renders_WithCustomClass_OnAriaCurrentSpan()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Label, "Active")
            .Add(x => x.AriaCurrent, true)
            .Add(x => x.Class, "font-bold"));

        // Assert
        var span = cut.Find("[aria-current='page']");
        Assert.Contains("font-bold", span.GetAttribute("class"));
    }

    [Fact]
    public void TwBreadcrumbItem_Renders_Icon_WhenSet()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Label, "Nav")
            .Add(x => x.Href, "/nav")
            .Add(x => x.Icon, Icons.Segmented_Nav));

        // Assert — TwIcon renders an <i> element
        Assert.NotNull(cut.Markup);
        Assert.Contains("<i ", cut.Markup, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TwBreadcrumbItem_Renders_FromBreadcrumbModel()
    {
        // Arrange
        var model = new BreadcrumbItem { Label = "Settings", Href = "/settings" };

        // Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Breadcrumb, model));

        // Assert
        var link = cut.Find("a");
        Assert.Equal("Settings", link.TextContent.Trim());
        Assert.Equal("/settings", link.GetAttribute("href"));
    }

    [Fact]
    public void TwBreadcrumbItem_Renders_AriaCurrent_FromBreadcrumbModel()
    {
        // Arrange
        var model = new BreadcrumbItem { Label = "Active", Href = "/active", AriaCurrent = true };

        // Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Breadcrumb, model));

        // Assert
        var span = cut.Find("[aria-current='page']");
        Assert.NotNull(span);
        Assert.Contains("Active", span.TextContent);
    }

    [Fact]
    public void TwBreadcrumbItem_Renders_Icon_FromBreadcrumbModel()
    {
        // Arrange
        var model = new BreadcrumbItem { Label = "Nav", Href = "/nav", Icon = Icons.Segmented_Nav };

        // Act
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Breadcrumb, model));

        // Assert
        Assert.Contains("<i ", cut.Markup, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TwBreadcrumbItem_NoSeparator_WhenRenderedWithoutParent()
    {
        // Arrange & Act — standalone, no cascading TwBreadcrumb
        var cut = TestContext.Render<TwBreadcrumbItem>(p => p
            .Add(x => x.Label, "Home")
            .Add(x => x.Href, "/"));

        // Assert — isFirst returns true when parent is null, so no separator
        var separators = cut.FindAll("span").Where(s => s.TextContent.Trim() == "/").ToList();
        Assert.Empty(separators);
    }

    [Fact]
    public void TwBreadcrumbItem_NoSeparator_ForFirstItem_InParent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.ChildContent, b =>
            {
                b.OpenComponent<TwBreadcrumbItem>(0);
                b.AddAttribute(1, "Label", "Home");
                b.AddAttribute(2, "Href", "/");
                b.CloseComponent();

                b.OpenComponent<TwBreadcrumbItem>(3);
                b.AddAttribute(4, "Label", "Next");
                b.AddAttribute(5, "Href", "/next");
                b.CloseComponent();
            }));

        // Assert — only the second item gets a separator
        var separators = cut.FindAll("span").Where(s => s.TextContent.Trim() == "/").ToList();
        Assert.Single(separators);
    }

    [Fact]
    public void TwBreadcrumbItem_Separator_RenderedBefore_NonFirstItems()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.ChildContent, b =>
            {
                b.OpenComponent<TwBreadcrumbItem>(0);
                b.AddAttribute(1, "Label", "A");
                b.AddAttribute(2, "Href", "/a");
                b.CloseComponent();

                b.OpenComponent<TwBreadcrumbItem>(3);
                b.AddAttribute(4, "Label", "B");
                b.AddAttribute(5, "Href", "/b");
                b.CloseComponent();

                b.OpenComponent<TwBreadcrumbItem>(6);
                b.AddAttribute(7, "Label", "C");
                b.AddAttribute(8, "Href", "/c");
                b.CloseComponent();
            }));

        // Assert — 3 items = 2 separators (before B and C)
        var separators = cut.FindAll("span").Where(s => s.TextContent.Trim() == "/").ToList();
        Assert.Equal(2, separators.Count);
    }

    [Fact]
    public void TwBreadcrumbItem_RendersCorrectHref_WhenInParent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.ChildContent, b =>
            {
                b.OpenComponent<TwBreadcrumbItem>(0);
                b.AddAttribute(1, "Label", "Home");
                b.AddAttribute(2, "Href", "/");
                b.CloseComponent();

                b.OpenComponent<TwBreadcrumbItem>(3);
                b.AddAttribute(4, "Label", "About");
                b.AddAttribute(5, "Href", "/about");
                b.CloseComponent();
            }));

        // Assert
        var links = cut.FindAll("a");
        Assert.Contains(links, l => l.GetAttribute("href") == "/");
        Assert.Contains(links, l => l.GetAttribute("href") == "/about");
    }

    [Fact]
    public void TwBreadcrumbItem_AriaCurrentItem_NotAnAnchor_WhenInParent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.ChildContent, b =>
            {
                b.OpenComponent<TwBreadcrumbItem>(0);
                b.AddAttribute(1, "Label", "Home");
                b.AddAttribute(2, "Href", "/");
                b.CloseComponent();

                b.OpenComponent<TwBreadcrumbItem>(3);
                b.AddAttribute(4, "Label", "Here");
                b.AddAttribute(5, "Href", "/here");
                b.AddAttribute(6, "AriaCurrent", true);
                b.CloseComponent();
            }));

        // Assert — "Here" should not be an anchor
        var links = cut.FindAll("a");
        Assert.DoesNotContain(links, l => l.TextContent.Trim() == "Here");

        var span = cut.Find("[aria-current='page']");
        Assert.Contains("Here", span.TextContent);
    }
}