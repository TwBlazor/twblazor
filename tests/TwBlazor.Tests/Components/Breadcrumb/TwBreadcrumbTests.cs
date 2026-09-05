using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TwBlazor.Components;
using TwBlazor.Models;

namespace TwBlazor.Tests.Components.Breadcrumb;

public class TwBreadcrumbTests : TwBlazorTestBase
{
    [Fact]
    public void TwBreadcrumb_Renders_NavElement()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>();

        // Assert
        var nav = cut.Find("nav");
        Assert.NotNull(nav);
    }

    [Fact]
    public void TwBreadcrumb_Renders_OrderedList()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>();

        // Assert
        var ol = cut.Find("ol");
        Assert.NotNull(ol);
    }

    [Fact]
    public void TwBreadcrumb_Renders_WithAriaLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.AriaLabel, "Site navigation"));

        // Assert
        var nav = cut.Find("nav");
        Assert.Equal("Site navigation", nav.GetAttribute("aria-label"));
    }

    [Fact]
    public void TwBreadcrumb_Renders_WithCustomId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Id, "my-breadcrumb"));

        // Assert
        var nav = cut.Find("nav");
        Assert.Equal("my-breadcrumb", nav.GetAttribute("id"));
    }

    [Fact]
    public void TwBreadcrumb_Renders_WithCustomClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Class, "custom-class"));

        // Assert
        var nav = cut.Find("nav");
        Assert.Contains("custom-class", nav.GetAttribute("class"));
    }

    [Fact]
    public void TwBreadcrumb_Renders_BreadcrumbsFromList()
    {
        // Arrange
        List<BreadcrumbItem> items =
        [
            new() { Label = "Home", Href = "/" },
            new() { Label = "Docs", Href = "/docs" },
        ];

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Breadcrumbs, items));

        // Assert
        Assert.Contains("Home", cut.Markup);
        Assert.Contains("Docs", cut.Markup);
    }

    [Fact]
    public void TwBreadcrumb_Renders_CorrectHrefs_FromList()
    {
        // Arrange
        List<BreadcrumbItem> items =
        [
            new() { Label = "Home", Href = "/" },
            new() { Label = "Components", Href = "/components" },
        ];

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Breadcrumbs, items));

        // Assert
        var links = cut.FindAll("a");
        Assert.Contains(links, l => l.GetAttribute("href") == "/");
        Assert.Contains(links, l => l.GetAttribute("href") == "/components");
    }

    [Fact]
    public void TwBreadcrumb_Renders_Separator_BetweenListItems()
    {
        // Arrange
        List<BreadcrumbItem> items =
        [
            new() { Label = "Home", Href = "/" },
            new() { Label = "Docs", Href = "/docs" },
            new() { Label = "Page", Href = "/docs/page" },
        ];

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Breadcrumbs, items));

        // Assert - 3 items means 2 separators (between items only, not a trailing one after the
        // last item), and each lives inside its <li> rather than as an invalid sibling of it.
        var separators = cut.FindAll("li span").Where(s => s.TextContent.Trim() == "/").ToList();
        Assert.Equal(2, separators.Count);
        Assert.All(separators, s => Assert.Equal("true", s.GetAttribute("aria-hidden")));
    }

    [Fact]
    public void TwBreadcrumb_Renders_AriaCurrent_OnCurrentItem_FromList()
    {
        // Arrange
        List<BreadcrumbItem> items =
        [
            new() { Label = "Home", Href = "/" },
            new() { Label = "Current", Href = "/current", AriaCurrent = true },
        ];

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Breadcrumbs, items));

        // Assert
        var currentSpan = cut.Find("[aria-current='page']");
        Assert.NotNull(currentSpan);
        Assert.Contains("Current", currentSpan.TextContent);
    }

    [Fact]
    public void TwBreadcrumb_DoesNotRender_Link_ForCurrentItem_FromList()
    {
        // Arrange
        List<BreadcrumbItem> items =
        [
            new() { Label = "Home", Href = "/" },
            new() { Label = "Here", Href = "/here", AriaCurrent = true },
        ];

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Breadcrumbs, items));

        // Assert — the current item should be a span, not an anchor
        var links = cut.FindAll("a");
        Assert.DoesNotContain(links, l => l.TextContent.Trim() == "Here");
    }

    [Fact]
    public void TwBreadcrumb_Renders_InlineChildItems()
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
                b.AddAttribute(4, "Label", "Docs");
                b.AddAttribute(5, "Href", "/docs");
                b.CloseComponent();
            }));

        // Assert
        Assert.Contains("Home", cut.Markup);
        Assert.Contains("Docs", cut.Markup);
    }

    [Fact]
    public void TwBreadcrumb_Renders_Separator_BetweenInlineItems()
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
                b.AddAttribute(4, "Label", "Docs");
                b.AddAttribute(5, "Href", "/docs");
                b.CloseComponent();

                b.OpenComponent<TwBreadcrumbItem>(6);
                b.AddAttribute(7, "Label", "Page");
                b.AddAttribute(8, "Href", "/docs/page");
                b.CloseComponent();
            }));

        // Assert — first item has no separator; 2nd and 3rd do
        var separators = cut.FindAll("span").Where(s => s.TextContent.Trim() == "/").ToList();
        Assert.Equal(2, separators.Count);
    }

    [Fact]
    public void TwBreadcrumb_FirstInlineItem_HasNo_Separator()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.ChildContent, b =>
            {
                b.OpenComponent<TwBreadcrumbItem>(0);
                b.AddAttribute(1, "Label", "Home");
                b.AddAttribute(2, "Href", "/");
                b.CloseComponent();
            }));

        // Assert — single item, no separator
        var separators = cut.FindAll("span").Where(s => s.TextContent.Trim() == "/").ToList();
        Assert.Empty(separators);
    }

    [Fact]
    public void TwBreadcrumb_Renders_AriaCurrent_OnInlineItem()
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
                b.AddAttribute(4, "Label", "Current");
                b.AddAttribute(5, "Href", "/current");
                b.AddAttribute(6, "AriaCurrent", true);
                b.CloseComponent();
            }));

        // Assert
        var currentSpan = cut.Find("[aria-current='page']");
        Assert.NotNull(currentSpan);
        Assert.Contains("Current", currentSpan.TextContent);
    }

    [Fact]
    public void TwBreadcrumb_Registers_InlineItems_InOrder()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.ChildContent, b =>
            {
                b.OpenComponent<TwBreadcrumbItem>(0);
                b.AddAttribute(1, "Label", "First");
                b.AddAttribute(2, "Href", "/first");
                b.CloseComponent();

                b.OpenComponent<TwBreadcrumbItem>(3);
                b.AddAttribute(4, "Label", "Second");
                b.AddAttribute(5, "Href", "/second");
                b.CloseComponent();
            }));

        // Assert — text appears in order
        var markup = cut.Markup;
        Assert.True(markup.IndexOf("First", StringComparison.Ordinal) < markup.IndexOf("Second", StringComparison.Ordinal));
    }

    [Fact]
    public void TwBreadcrumb_Renders_EmptyList_WithNoItems()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwBreadcrumb>();

        // Assert
        var links = cut.FindAll("a");
        Assert.Empty(links);
    }

    [Fact]
    public void TwBreadcrumb_Auto_BuildsBreadcrumbsFromCurrentUri()
    {
        // Arrange - Auto=true builds Breadcrumbs from the current NavigationManager URI
        // segments instead of relying on manually supplied items.
        var navMan = TestContext.Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("products/shoes");

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Auto, true));

        // Assert
        Assert.Equal(2, cut.Instance.Breadcrumbs.Count);
        Assert.Equal("products", cut.Instance.Breadcrumbs[0].Label);
        Assert.Equal("/products", cut.Instance.Breadcrumbs[0].Href);
        Assert.Equal("shoes", cut.Instance.Breadcrumbs[1].Label);
        Assert.Equal("/products/shoes", cut.Instance.Breadcrumbs[1].Href);
    }

    [Fact]
    public void TwBreadcrumb_Auto_AtSiteRoot_ProducesNoBreadcrumbs()
    {
        // Arrange - at the root path, ToBaseRelativePath returns "", and naively splitting that on "/"
        // yields a single empty-string segment. Unfiltered, that used to produce one breadcrumb with an
        // empty Label marked as the current page (aria-current="page") - an empty, nameless entry.
        var navMan = TestContext.Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("");

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Auto, true));

        // Assert
        Assert.Empty(cut.Instance.Breadcrumbs);
    }

    [Fact]
    public void TwBreadcrumb_Auto_SingleSegmentPath_ProducesOneBreadcrumb()
    {
        // Arrange
        var navMan = TestContext.Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("docs");

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Auto, true));

        // Assert
        Assert.Single(cut.Instance.Breadcrumbs);
        Assert.Equal("docs", cut.Instance.Breadcrumbs[0].Label);
        Assert.Equal("/docs", cut.Instance.Breadcrumbs[0].Href);
        Assert.True(cut.Instance.Breadcrumbs[0].AriaCurrent);
    }

    [Fact]
    public void TwBreadcrumb_NotAuto_DoesNotBuildBreadcrumbsFromUri()
    {
        // Arrange
        var navMan = TestContext.Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("products/shoes");

        // Act
        var cut = TestContext.Render<TwBreadcrumb>(p => p
            .Add(x => x.Auto, false));

        // Assert
        Assert.Empty(cut.Instance.Breadcrumbs);
    }

    [Fact]
    public void AddItem_DoesNotDuplicate_WhenSameItemAddedTwice()
    {
        // Arrange
        var cut = TestContext.Render<TwBreadcrumb>();
        var item = new TwBreadcrumbItem();

        // Act - must run via the component's dispatcher since AddItem calls StateHasChanged
        cut.InvokeAsync(() => cut.Instance.AddItem(item));
        cut.InvokeAsync(() => cut.Instance.AddItem(item)); // duplicate reference - should be ignored

        // Assert
        Assert.Single(cut.Instance.inlineBreadcrumbs);
    }
}