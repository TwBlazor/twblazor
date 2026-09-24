using TwBlazor.Docs.Compiler;
using TwBlazor.Docs.Services;

namespace TwBlazor.Docs.Tests.Build;

public class SitemapGeneratorTests
{
    [Fact]
    public void GenerateSitemap_WritesAnAbsoluteLocForEveryPage()
    {
        // Arrange
        PageEntry[] entries = [new("/", null), new("/card", null)];

        // Act
        var xml = SitemapGenerator.GenerateSitemap(entries, "https://example.com/");

        // Assert
        Assert.Contains("<loc>https://example.com/</loc>", xml);
        Assert.Contains("<loc>https://example.com/card</loc>", xml);
    }

    [Fact]
    public void GenerateSitemap_WritesLastmodOnlyForPagesWithAKnownDate()
    {
        // Arrange
        PageEntry[] entries = [new("/card", new DateOnly(2026, 9, 4)), new("/alert", null)];

        // Act
        var xml = SitemapGenerator.GenerateSitemap(entries);

        // Assert
        Assert.Single(System.Text.RegularExpressions.Regex.Matches(xml, "<lastmod>"));
        Assert.Contains("<lastmod>2026-09-04</lastmod>", xml);
    }

    [Fact]
    public void GenerateSitemap_OmitsChangefreqAndPriority()
    {
        // Arrange & Act
        var xml = SitemapGenerator.GenerateSitemap([new PageEntry("/card", null)]);

        // Assert - search engines ignore both, so they'd only be noise.
        Assert.DoesNotContain("changefreq", xml);
        Assert.DoesNotContain("priority", xml);
    }

    [Fact]
    public void GenerateSitemap_EscapesXmlSpecialCharactersInTheRoute()
    {
        // Arrange & Act
        var xml = SitemapGenerator.GenerateSitemap([new PageEntry("/a&b", null)]);

        // Assert
        Assert.Contains("<loc>https://twblazor.com/a&amp;b</loc>", xml);
    }

    [Fact]
    public void GenerateSitemap_ProducesWellFormedXml()
    {
        // Arrange & Act
        var xml = SitemapGenerator.GenerateSitemap([new PageEntry("/", new DateOnly(2026, 1, 2)), new PageEntry("/card", null)]);

        // Assert
        var document = System.Xml.Linq.XDocument.Parse(xml);
        Assert.Equal("urlset", document.Root!.Name.LocalName);
    }

    [Fact]
    public void GeneratePageLastModifiedClass_ListsDatedPagesInRouteOrderAndSkipsUndatedOnes()
    {
        // Arrange
        PageEntry[] entries = [new("/card", new DateOnly(2026, 9, 4)), new("/alert", new DateOnly(2026, 1, 12)), new("/chip", null)];

        // Act
        var code = SitemapGenerator.GeneratePageLastModifiedClass(entries);

        // Assert
        Assert.Contains("[\"/alert\"] = new DateOnly(2026, 1, 12),", code);
        Assert.Contains("[\"/card\"] = new DateOnly(2026, 9, 4),", code);
        Assert.DoesNotContain("/chip", code);
        Assert.True(code.IndexOf("/alert", StringComparison.Ordinal) < code.IndexOf("/card", StringComparison.Ordinal));
    }

    [Fact]
    public void DefaultBaseUrl_MatchesTheOriginTheCanonicalUrlsAreBuiltFrom()
    {
        // Assert - a mismatch would put sitemap URLs on a different origin than the pages' canonical tags.
        Assert.Equal(SitemapGenerator.DefaultBaseUrl, SiteMetadata.BaseUrl);
    }
}
