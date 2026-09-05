using Bunit;
using TwBlazor.Components;
using TwBlazor.Configuration.Components;

namespace TwBlazor.Tests.Components.InputLabel;

public class TwInputLabelTests : TwBlazorTestBase
{
    private TwInputTheme inputTheme => Theme.Components.Require<TwInputTheme>();

    [Fact]
    public void TwInputLabel_Renders_WithDefaultValues()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>();

        // Assert
        var label = cut.Find("label");
        Assert.NotNull(label);
        Assert.Equal(string.Empty, label.GetAttribute("for"));
        Assert.Equal(string.Empty, label.TextContent);
    }

    [Fact]
    public void TwInputLabel_Renders_WithFor()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.For, "username"));

        // Assert
        var label = cut.Find("label");
        Assert.Equal("username", label.GetAttribute("for"));
    }

    [Fact]
    public void TwInputLabel_Renders_WithLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.Label, "Username"));

        // Assert
        var label = cut.Find("label");
        Assert.Equal("Username", label.TextContent);
    }

    [Fact]
    public void TwInputLabel_Renders_WithForAndLabel()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.For, "email")
            .Add(p => p.Label, "Email Address"));

        // Assert
        var label = cut.Find("label");
        Assert.Equal("email", label.GetAttribute("for"));
        Assert.Equal("Email Address", label.TextContent);
    }

    [Fact]
    public void TwInputLabel_Renders_WithId()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.For, "password")
            .Add(p => p.Label, "Password")
            .Add(p => p.Id, "password-label"));

        // Assert
        var label = cut.Find("label");
        Assert.Equal("password-label", label.GetAttribute("id"));
    }

    [Fact]
    public void TwInputLabel_Renders_WithClass()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.For, "name")
            .Add(p => p.Label, "Full Name")
            .Add(p => p.Class, "text-sm font-medium"));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("text-sm", label.GetAttribute("class"));
        Assert.Contains("font-medium", label.GetAttribute("class"));
    }

    [Fact]
    public void TwInputLabel_Renders_WithUnmatchedAttributes()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.For, "description")
            .Add(p => p.Label, "Description")
            .AddUnmatched("data-testid", "description-label")
            .AddUnmatched("title", "Enter description"));

        // Assert
        var label = cut.Find("label");
        Assert.Equal("description-label", label.GetAttribute("data-testid"));
        Assert.Equal("Enter description", label.GetAttribute("title"));
    }

    [Fact]
    public void TwInputLabel_Renders_WithMultipleParameters()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.For, "phone")
            .Add(p => p.Label, "Phone Number")
            .Add(p => p.Id, "phone-label")
            .Add(p => p.Class, "required asterisk"));

        // Assert
        var label = cut.Find("label");
        Assert.Equal("phone", label.GetAttribute("for"));
        Assert.Equal("Phone Number", label.TextContent);
        Assert.Equal("phone-label", label.GetAttribute("id"));
        Assert.Contains("required", label.GetAttribute("class"));
        Assert.Contains("asterisk", label.GetAttribute("class"));
    }

    [Fact]
    public void TwInputLabel_Renders_WithEmptyStrings()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.For, string.Empty)
            .Add(p => p.Label, string.Empty));

        // Assert
        var label = cut.Find("label");
        Assert.NotNull(label);
        Assert.Equal(string.Empty, label.GetAttribute("for"));
        Assert.Equal(string.Empty, label.TextContent);
    }

    [Fact]
    public void TwInputLabel_Renders_WithSpecialCharacters()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.For, "user-email-address")
            .Add(p => p.Label, "User's Email & Contact"));

        // Assert
        var label = cut.Find("label");
        Assert.Equal("user-email-address", label.GetAttribute("for"));
        Assert.Equal("User's Email & Contact", label.TextContent);
    }

    #region Theme Tests

    [Fact]
    public void TwInputLabel_UsesDefaultClasses_WhenNoOptionsSet()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.Label, "Test Label"));

        // Assert - Should use default theme classes
        var label = cut.Find("label");
        Assert.NotNull(label.GetAttribute("class"));
    }

    [Fact]
    public void TwInputLabel_UsesTheme_LabelBaseClasses()
    {
        // Arrange
        inputTheme.LabelBase = "custom-base-label-class";

        // Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.Label, "Test"));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("custom-base-label-class", label.GetAttribute("class"));
    }

    [Fact]
    public void TwInputLabel_UsesTheme_LabelBaseClasses_WithUppercase()
    {
        // Arrange
        inputTheme.LabelBase = "custom-base text-sm uppercase";

        // Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.Label, "Test"));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("custom-base", label.GetAttribute("class"));
        Assert.Contains("text-sm", label.GetAttribute("class"));
        Assert.Contains("uppercase", label.GetAttribute("class"));
    }

    [Fact]
    public void TwInputLabel_CombinesThemeBaseClassesAndCustomClasses()
    {
        // Arrange
        inputTheme.LabelBase = "theme-base uppercase";

        // Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.Label, "Test")
            .Add(p => p.Class, "custom-class"));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("theme-base", label.GetAttribute("class"));
        Assert.Contains("uppercase", label.GetAttribute("class"));
        Assert.Contains("custom-class", label.GetAttribute("class"));
    }

    [Fact]
    public void TwInputLabel_AddsAdditionalClassesToBaseClasses()
    {
        // Arrange
        inputTheme.LabelBase = "block mb-2 text-xs font-bold custom-tracking";

        // Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.Label, "Test"));

        // Assert
        var label = cut.Find("label");
        Assert.Contains("block", label.GetAttribute("class"));
        Assert.Contains("mb-2", label.GetAttribute("class"));
        Assert.Contains("text-xs", label.GetAttribute("class"));
        Assert.Contains("font-bold", label.GetAttribute("class"));
        Assert.Contains("custom-tracking", label.GetAttribute("class"));
    }

    [Fact]
    public void TwInputLabel_UsesOverrideClass_InsteadOfTheme()
    {
        // Arrange - set a theme value that should be ignored when OverrideClass is set
        inputTheme.LabelBase = "theme-base";

        // Act
        var cut = TestContext.Render<TwInputLabel>(parameters => parameters
            .Add(p => p.Label, "Test")
            .Add(p => p.OverrideClass, "override-base")
            .Add(p => p.Class, "extra-class"));

        // Assert
        var label = cut.Find("label");
        var classes = label.GetAttribute("class");
        Assert.Contains("override-base", classes);
        Assert.Contains("extra-class", classes);
        Assert.DoesNotContain("theme-base", classes);
    }

    #endregion
}
