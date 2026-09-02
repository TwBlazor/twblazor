using Bunit;
using Microsoft.AspNetCore.Components.Web;
using TwBlazor.Components;
using TwBlazor.Enums;
using Icons = TwBlazor.Enums.Icon;

namespace TwBlazor.Tests.Components.Chip;

public class TwChipTests : TwBlazorTestBase
{
    [Fact]
    public void TwChip_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>();

        // Assert
        var chip = cut.Find("span");
        Assert.NotNull(chip);
    }

    [Fact]
    public void TwChip_Renders_WithLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Test Chip"));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("Test Chip", chip.TextContent);
    }

    [Fact]
    public void TwChip_Renders_WithChildContent()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.ChildContent, RenderFragmentBuilder("Custom Content")));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("Custom Content", chip.TextContent);
    }

    [Fact]
    public void TwChip_ChildContent_TakesPrecedenceOverLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "This should not appear")
            .Add(p => p.ChildContent, RenderFragmentBuilder("This should appear")));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("This should appear", chip.TextContent);
        Assert.DoesNotContain("This should not appear", chip.TextContent);
    }

    [Fact]
    public void TwChip_Renders_WithId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Id, "test-chip"));

        // Assert
        var chip = cut.Find("span");
        Assert.Equal("test-chip", chip.GetAttribute("id"));
    }

    [Fact]
    public void TwChip_Renders_WithClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Class, "custom-class"));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("custom-class", chip.GetAttribute("class"));
    }

    [Theory]
    [InlineData(ChipSize.Small)]
    [InlineData(ChipSize.Medium)]
    [InlineData(ChipSize.Large)]
    public void TwChip_Renders_WithSize(ChipSize size)
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Size, size));

        // Assert
        var chip = cut.Find("span");
        Assert.NotNull(chip);
    }

    [Theory]
    [InlineData(Color.Danger)]
    [InlineData(Color.Primary)]
    [InlineData(Color.Success)]
    [InlineData(Color.Accent)]
    public void TwChip_Renders_WithColor(Color color)
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Color, color));

        // Assert
        var chip = cut.Find("span");
        Assert.NotNull(chip);
    }

    [Theory]
    [InlineData(ButtonVariant.Filled)]
    [InlineData(ButtonVariant.Outlined)]
    [InlineData(ButtonVariant.Text)]
    [InlineData(ButtonVariant.Elevated)]
    public void TwChip_Renders_WithVariant(ButtonVariant variant)
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Variant, variant));

        // Assert
        var chip = cut.Find("span");
        Assert.NotNull(chip);
    }

    [Fact]
    public void TwChip_Renders_AsDisabled()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Disabled, true));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("cursor-not-allowed", chip.GetAttribute("class"));
    }

    [Fact]
    public void TwChip_Renders_WithStartIcon()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Icon Chip")
            .Add(p => p.StartIcon, Icons.Star));

        // Assert
        var chip = cut.Find("span");
        var icon = chip.QuerySelector("i");
        Assert.NotNull(icon);
    }

    [Fact]
    public void TwChip_Renders_WithAvatar()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Avatar Chip")
            .Add(p => p.Avatar, "AB"));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("AB", chip.TextContent);
    }

    [Fact]
    public void TwChip_Avatar_TakesPrecedenceOverStartIcon()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Avatar Chip")
            .Add(p => p.Avatar, "AB")
            .Add(p => p.StartIcon, Icons.Star));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("AB", chip.TextContent);
        // Should not render icon when avatar is present
        var icons = chip.QuerySelectorAll("i");
        // Avatar chip shouldn't have the start icon (only close icon if closable)
        Assert.Empty(icons);
    }

    [Theory]
    [InlineData(ChipSize.Small)]
    [InlineData(ChipSize.Medium)]
    [InlineData(ChipSize.Large)]
    public void TwChip_Renders_WithAvatar_ForEachSize(ChipSize size)
    {
        // Arrange & Act - GetAvatarClasses() switches on Size; the default-parameter
        // tests above only ever exercised the Medium arm.
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Avatar Chip")
            .Add(p => p.Avatar, "AB")
            .Add(p => p.Size, size));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("AB", chip.TextContent);
    }

    [Theory]
    [InlineData(ChipSize.Small)]
    [InlineData(ChipSize.Medium)]
    [InlineData(ChipSize.Large)]
    public void TwChip_Renders_WithStartIcon_ForEachSize(ChipSize size)
    {
        // Arrange & Act - GetIconSize() switches on Size; the default-parameter
        // tests above only ever exercised the Medium arm.
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Icon Chip")
            .Add(p => p.StartIcon, Icons.Star)
            .Add(p => p.Size, size));

        // Assert
        var chip = cut.Find("span");
        var icon = chip.QuerySelector("i");
        Assert.NotNull(icon);
    }

    [Fact]
    public void TwChip_Renders_WithAvatar_ForUndefinedSize()
    {
        // Arrange & Act - out-of-range ChipSize hits GetAvatarClasses()'s switch default arm.
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Avatar Chip")
            .Add(p => p.Avatar, "AB")
            .Add(p => p.Size, (ChipSize)999));

        // Assert
        var chip = cut.Find("span");
        Assert.Contains("AB", chip.TextContent);
    }

    [Fact]
    public void TwChip_Renders_WithStartIcon_ForUndefinedSize()
    {
        // Arrange & Act - out-of-range ChipSize hits GetIconSize()'s switch default arm.
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Icon Chip")
            .Add(p => p.StartIcon, Icons.Star)
            .Add(p => p.Size, (ChipSize)999));

        // Assert
        var chip = cut.Find("span");
        var icon = chip.QuerySelector("i");
        Assert.NotNull(icon);
    }

    [Fact]
    public void TwChip_Renders_WithCloseButton_WhenClosable()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Closable Chip")
            .Add(p => p.Closable, true));

        // Assert
        var chip = cut.Find("span");
        var closeButton = chip.QuerySelector("button");
        Assert.NotNull(closeButton);
    }

    [Fact]
    public void TwChip_DoesNotRender_CloseButton_WhenNotClosable()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Non-closable Chip")
            .Add(p => p.Closable, false));

        // Assert
        var chip = cut.Find("span");
        var closeButton = chip.QuerySelector("button");
        Assert.Null(closeButton);
    }

    [Fact]
    public void TwChip_InvokesOnClose_WhenCloseButtonClicked()
    {
        // Arrange
        var closeCalled = false;
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Closable Chip")
            .Add(p => p.Closable, true)
            .Add(p => p.OnClose, () => closeCalled = true));

        // Act
        var closeButton = cut.Find("button");
        closeButton.Click();

        // Assert
        Assert.True(closeCalled);
    }

    [Fact]
    public void TwChip_InvokesOnClick_WhenClicked()
    {
        // Arrange
        var clickCalled = false;
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Clickable Chip")
            .Add(p => p.OnClick, () => clickCalled = true));

        // Act
        var chip = cut.Find("button");
        chip.Click();

        // Assert
        Assert.True(clickCalled);
    }

    [Fact]
    public void TwChip_DoesNotInvokeOnClick_WhenDisabled()
    {
        // Arrange
        var clickCalled = false;
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Disabled Chip")
            .Add(p => p.Disabled, true)
            .Add(p => p.OnClick, () => clickCalled = true));

        // Act
        var chip = cut.Find("button");
        chip.Click();

        // Assert
        Assert.False(clickCalled);
    }

    [Fact]
    public void TwChip_InvokesOnClick_WhenEnterPressed()
    {
        // Arrange - Closable so the chip still renders as a span/role="button" with a custom
        // keydown handler (a plain non-closable clickable chip now renders as a real <button>,
        // which gets Enter/Space activation for free from the browser rather than from our code).
        var clickCalled = false;
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Clickable Chip")
            .Add(p => p.Closable, true)
            .Add(p => p.OnClick, () => clickCalled = true));

        // Act
        var chip = cut.Find("span");
        chip.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        // Assert
        Assert.True(clickCalled);
    }

    [Fact]
    public void TwChip_InvokesOnClick_WhenSpacePressed()
    {
        // Arrange - see TwChip_InvokesOnClick_WhenEnterPressed for why Closable is set here.
        var clickCalled = false;
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Clickable Chip")
            .Add(p => p.Closable, true)
            .Add(p => p.OnClick, () => clickCalled = true));

        // Act
        var chip = cut.Find("span");
        chip.KeyDown(new KeyboardEventArgs { Key = " " });

        // Assert
        Assert.True(clickCalled);
    }

    [Fact]
    public void TwChip_DoesNotInvokeOnClick_WhenOtherKeyPressed()
    {
        // Arrange - see TwChip_InvokesOnClick_WhenEnterPressed for why Closable is set here.
        var clickCalled = false;
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Clickable Chip")
            .Add(p => p.Closable, true)
            .Add(p => p.OnClick, () => clickCalled = true));

        // Act
        var chip = cut.Find("span");
        chip.KeyDown(new KeyboardEventArgs { Key = "A" });

        // Assert
        Assert.False(clickCalled);
    }

    [Fact]
    public void TwChip_KeyDown_DoesNothing_WhenNotClickable()
    {
        // Arrange - no OnClick delegate and no Href means isClickable is false.
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Static Chip"));

        // Act & Assert - should not throw
        var chip = cut.Find("span");
        chip.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        // Assert the chip element still exists and is rendered correctly
        Assert.NotNull(chip);
        Assert.Contains("Static Chip", chip.TextContent);
    }

    [Fact]
    public void TwChip_DoesNotInvokeOnClose_WhenDisabled()
    {
        // Arrange
        var closeCalled = false;
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Disabled Closable Chip")
            .Add(p => p.Disabled, true)
            .Add(p => p.Closable, true)
            .Add(p => p.OnClose, () => closeCalled = true));

        // Act
        var closeButton = cut.Find("button");
        closeButton.Click();

        // Assert
        Assert.False(closeCalled);
    }

    [Fact]
    public void TwChip_Renders_AsLink_WhenHrefIsSet()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Link Chip")
            .Add(p => p.Href, "/test"));

        // Assert
        var link = cut.Find("a");
        Assert.NotNull(link);
        Assert.Equal("/test", link.GetAttribute("href"));
    }

    [Fact]
    public void TwChip_Link_AddsNoopenerRel_WhenTargetIsBlank()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "External Link Chip")
            .Add(p => p.Href, "https://example.com")
            .Add(p => p.Target, "_blank"));

        // Assert
        var link = cut.Find("a");
        Assert.Equal("noopener", link.GetAttribute("rel"));
        Assert.Equal("_blank", link.GetAttribute("target"));
    }

    [Fact]
    public void TwChip_Link_UsesCustomRel_WhenProvided()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "External Link Chip")
            .Add(p => p.Href, "https://example.com")
            .Add(p => p.Target, "_blank")
            .Add(p => p.Rel, "noopener noreferrer"));

        // Assert
        var link = cut.Find("a");
        Assert.Equal("noopener noreferrer", link.GetAttribute("rel"));
    }

    [Fact]
    public void TwChip_Link_DoesNotRenderCloseButton()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Link Chip")
            .Add(p => p.Href, "/test")
            .Add(p => p.Closable, true));

        // Assert
        var link = cut.Find("a");
        var closeButton = link.QuerySelector("button");
        Assert.Null(closeButton);
    }

    [Fact]
    public void TwChip_Link_DoesNotInvokeOnClick()
    {
        // Arrange
        var clickCalled = false;
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Link Chip")
            .Add(p => p.Href, "/test")
            .Add(p => p.OnClick, () => clickCalled = true));

        // Act
        var link = cut.Find("a");

        // Assert
        Assert.False(link.HasAttribute("onclick"));
        Assert.False(clickCalled);
    }

    [Fact]
    public void TwChip_Renders_WithCustomCloseIcon()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Closable Chip")
            .Add(p => p.Closable, true)
            .Add(p => p.CloseIcon, Icons.X_Circle));

        // Assert
        var chip = cut.Find("span");
        var closeButton = chip.QuerySelector("button");
        Assert.NotNull(closeButton);
    }

    [Fact]
    public void TwChip_Renders_WithAriaLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwChip>(parameters => parameters
            .Add(p => p.Label, "Chip")
            .Add(p => p.AriaLabel, "Accessible Chip"));

        // Assert
        var chip = cut.Find("span");
        Assert.Equal("Accessible Chip", chip.GetAttribute("aria-label"));
    }
}
