using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Docs.Layout;

namespace TwBlazor.Docs.Tests.Layout;

public class PageContainerTests : DocsTestBase
{
    private IRenderedComponent<PageContainer> Render(string path = "/no-such-page", string? seoTitle = null) =>
        TestContext.Render<PageContainer>(parameters => parameters
            .Add(p => p.Title, "TwCard")
            .Add(p => p.Path, path)
            .Add(p => p.Description, "TwCard is a Blazor card component.")
            .Add(p => p.SeoTitle, seoTitle)
            .Add(p => p.ChildContent, (RenderFragment)(builder => builder.AddMarkupContent(0, "<p id=\"body\">Body</p>"))));

    [Fact]
    public void Render_ShowsTheDescriptionAsTheIntroUnderTheHeading()
    {
        // Arrange & Act
        var cut = Render();

        // Assert - answer engines quote body text, so the intro must be in the page, not only in a meta tag.
        Assert.Equal("TwCard", cut.Find("h1").TextContent);
        Assert.Equal("TwCard is a Blazor card component.", cut.Find("h1 + p").TextContent);
    }

    [Fact]
    public void Render_TitlesThePageWithTheNamePipeSiteDashSummaryShape()
    {
        // Arrange
        var head = TestContext.Render<Microsoft.AspNetCore.Components.Web.HeadOutlet>();

        // Act
        Render();
        head.WaitForState(() => head.Markup.Contains("<title>"));

        // Assert
        Assert.Equal("TwCard | twblazor - Blazor Card Component", head.Find("title").TextContent);
    }

    [Fact]
    public void Render_UsesTheExplicitSeoTitle_WhenOneIsGiven()
    {
        // Arrange
        var head = TestContext.Render<Microsoft.AspNetCore.Components.Web.HeadOutlet>();

        // Act
        Render(seoTitle: "Get Started | twblazor - Install Tailwind CSS Components for Blazor");
        head.WaitForState(() => head.Markup.Contains("<title>"));

        // Assert
        Assert.Equal("Get Started | twblazor - Install Tailwind CSS Components for Blazor", head.Find("title").TextContent);
    }

    [Fact]
    public void Render_ShowsNoLastUpdatedLine_WhenNoDateIsKnown()
    {
        // Arrange & Act
        var cut = Render("/no-such-page");

        // Assert
        Assert.DoesNotContain("Last updated", cut.Markup);
        Assert.Empty(cut.FindAll("time"));
    }

    [Fact]
    public void Render_RendersTheChildContentAfterTheIntro()
    {
        // Arrange & Act
        var cut = Render();

        // Assert
        Assert.Equal("Body", cut.Find("#body").TextContent);
    }

    [Fact]
    public void Render_ShowsNoThemeCard_WhenNoThemeTypeIsGiven()
    {
        // Arrange & Act
        var cut = Render();

        // Assert
        Assert.DoesNotContain("Theme Configuration", cut.Markup);
    }
}
