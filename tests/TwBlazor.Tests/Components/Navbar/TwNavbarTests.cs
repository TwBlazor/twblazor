using Bunit;
using TwBlazor.Components;
using TwBlazor.Configuration.Components;

namespace TwBlazor.Tests.Components.Navbar;

public class TwNavbarTests : TwBlazorTestBase
{
    private TwSidebarTheme sidebarTheme => Theme.Components.Require<TwSidebarTheme>();

    #region Rendering

    [Fact]
    public void ShouldRender_NavElement_WithDefaultAriaLabel()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>();

        // Assert
        var nav = cut.Find("nav");
        Assert.Equal("Top navigation", nav.GetAttribute("aria-label"));
    }

    [Fact]
    public void ShouldRender_CustomAriaLabel_WhenProvided()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.AriaLabel, "my navbar")
        );

        // Assert
        Assert.Equal("my navbar", cut.Find("nav").GetAttribute("aria-label"));
    }

    [Fact]
    public void ShouldRender_ChildContent_InsideNav()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.ChildContent, b => b.AddMarkupContent(0, "<span class=\"inner\">Hello</span>"))
        );

        // Assert
        Assert.NotNull(cut.Find("span.inner"));
        Assert.Contains("Hello", cut.Markup);
    }

    [Fact]
    public void ShouldRender_WithNoChildContent_WhenNotProvided()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>();

        // Assert — nav renders with no child elements
        var nav = cut.Find("nav");
        Assert.Empty(nav.Children);
    }

    #endregion

    #region Base Theme Classes

    [Fact]
    public void ShouldApply_NavbarThemeBaseClasses()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>();

        // Assert
        var cls = cut.Find("nav").GetAttribute("class");
        Assert.Contains(sidebarTheme.Navbar.Split(' ')[0], cls);
    }

    #endregion

    #region Fixed = false (sticky)

    [Fact]
    public void ShouldApply_StickyClasses_WhenFixed_False()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.Fixed, false)
        );

        // Assert
        var cls = cut.Find("nav").GetAttribute("class");
        Assert.Contains("sticky", cls);
        Assert.Contains("top-0", cls);
    }

    [Fact]
    public void ShouldNotApply_FixedPositionClasses_WhenFixed_False()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.Fixed, false)
        );

        // Assert
        var cls = cut.Find("nav").GetAttribute("class");
        Assert.DoesNotContain("left-0", cls);
        Assert.DoesNotContain("right-0", cls);
    }

    #endregion

    #region Fixed = true

    [Fact]
    public void ShouldApply_FixedClasses_WhenFixed_True()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.Fixed, true)
        );

        // Assert
        var cls = cut.Find("nav").GetAttribute("class");
        Assert.Contains("fixed", cls);
        Assert.Contains("top-0", cls);
        Assert.Contains("left-0", cls);
        Assert.Contains("right-0", cls);
    }

    [Fact]
    public void ShouldNotApply_StickyClass_WhenFixed_True()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.Fixed, true)
        );

        // Assert
        Assert.DoesNotContain("sticky", cut.Find("nav").GetAttribute("class"));
    }

    #endregion

    #region Class parameter

    [Fact]
    public void ShouldApply_ExtraClass_WhenClassProvided()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.Class, "my-custom-class")
        );

        // Assert
        Assert.Contains("my-custom-class", cut.Find("nav").GetAttribute("class"));
    }

    [Fact]
    public void ShouldApply_ExtraClass_AlongsideFixedClasses()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.Fixed, true)
            .Add(x => x.Class, "extra-fixed")
        );

        // Assert
        var cls = cut.Find("nav").GetAttribute("class");
        Assert.Contains("fixed", cls);
        Assert.Contains("extra-fixed", cls);
    }

    [Fact]
    public void ShouldApply_ExtraClass_AlongsideStickyClasses()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>(p => p
            .Add(x => x.Fixed, false)
            .Add(x => x.Class, "extra-sticky")
        );

        // Assert
        var cls = cut.Find("nav").GetAttribute("class");
        Assert.Contains("sticky", cls);
        Assert.Contains("extra-sticky", cls);
    }

    #endregion

    #region Default Fixed value

    [Fact]
    public void ShouldDefault_ToSticky_WhenFixed_NotProvided()
    {
        // Act
        var cut = TestContext.Render<TwNavbar>();

        // Assert
        var cls = cut.Find("nav").GetAttribute("class");
        Assert.Contains("sticky", cls);
        Assert.DoesNotContain("left-0", cls);
    }

    #endregion
}
