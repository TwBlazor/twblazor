using TwBlazor.Enums;
using TwBlazor.Models;

namespace TwBlazor.Tests.Models;

public class TwDialogOptionsTests
{
    [Fact]
    public void DefaultOptions_HasAllNullProperties()
    {
        // Arrange & Act
        var options = new TwDialogOptions();

        // Assert
        Assert.Null(options.Position);
        Assert.Null(options.MaxWidth);
        Assert.Null(options.BackdropClick);
        Assert.Null(options.CloseOnEscapeKey);
        Assert.Null(options.NoHeader);
        Assert.Null(options.CloseButton);
        Assert.Null(options.FullScreen);
        Assert.Null(options.FullWidth);
        Assert.Null(options.Rounded);
        Assert.Null(options.Shadow);
        Assert.Null(options.Class);
        Assert.Null(options.BackdropClass);
    }

    [Fact]
    public void Options_CanBeInitialized_WithAllProperties()
    {
        // Arrange & Act
        var options = new TwDialogOptions
        {
            Position = DialogPosition.TopCenter,
            MaxWidth = DialogMaxWidth.Large,
            BackdropClick = false,
            CloseOnEscapeKey = false,
            NoHeader = true,
            CloseButton = false,
            FullScreen = true,
            FullWidth = true,
            Rounded = Rounded.Full,
            Shadow = Shadow.Lg,
            Class = "custom-class",
            BackdropClass = "custom-backdrop"
        };

        // Assert
        Assert.Equal(DialogPosition.TopCenter, options.Position);
        Assert.Equal(DialogMaxWidth.Large, options.MaxWidth);
        Assert.False(options.BackdropClick);
        Assert.False(options.CloseOnEscapeKey);
        Assert.True(options.NoHeader);
        Assert.False(options.CloseButton);
        Assert.True(options.FullScreen);
        Assert.True(options.FullWidth);
        Assert.Equal(Rounded.Full, options.Rounded);
        Assert.Equal(Shadow.Lg, options.Shadow);
        Assert.Equal("custom-class", options.Class);
        Assert.Equal("custom-backdrop", options.BackdropClass);
    }

    [Fact]
    public void Options_SupportsRecordEquality()
    {
        // Arrange
        var options1 = new TwDialogOptions { NoHeader = true };
        var options2 = new TwDialogOptions { NoHeader = true };

        // Assert
        Assert.Equal(options1, options2);
    }

    [Fact]
    public void Options_WithExpression_CreatesModifiedCopy()
    {
        // Arrange
        var options = new TwDialogOptions { NoHeader = false };

        // Act
        var modified = options with { NoHeader = true };

        // Assert
        Assert.False(options.NoHeader);
        Assert.True(modified.NoHeader);
    }
}
