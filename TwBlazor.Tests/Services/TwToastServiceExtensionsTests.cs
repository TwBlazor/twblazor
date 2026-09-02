using TwBlazor.Enums;
using TwBlazor.Services;

namespace TwBlazor.Tests.Services;

public class TwToastServiceExtensionsTests
{
    [Fact]
    public void ShowSuccess_CreatesSuccessToast()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.ShowSuccess("Success", "Operation completed");

        // Assert
        var toasts = service.GetToasts();
        var toast = Assert.Single(toasts);
        Assert.Equal("Success", toast.Title);
        Assert.Equal("Operation completed", toast.Message);
        Assert.Equal(Color.Success, toast.Color);
        Assert.Equal(Icon.Check_Circle, toast.Icon);
        Assert.Equal(5000, toast.Duration);
    }

    [Fact]
    public void ShowSuccess_WithoutMessage_CreatesEmptyMessage()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.ShowSuccess("Success");

        // Assert
        var toasts = service.GetToasts();
        var toast = Assert.Single(toasts);
        Assert.Equal(string.Empty, toast.Message);
    }

    [Fact]
    public void ShowSuccess_WithCustomDuration()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.ShowSuccess("Success", "Message", 3000);

        // Assert
        var toasts = service.GetToasts();
        Assert.Equal(3000, toasts[0].Duration);
    }

    [Fact]
    public void ShowError_CreatesErrorToast()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.ShowError("Error", "Something went wrong");

        // Assert
        var toasts = service.GetToasts();
        var toast = Assert.Single(toasts);
        Assert.Equal("Error", toast.Title);
        Assert.Equal("Something went wrong", toast.Message);
        Assert.Equal(Color.Danger, toast.Color);
        Assert.Equal(Icon.X_Circle, toast.Icon);
    }

    [Fact]
    public void ShowWarning_CreatesWarningToast()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.ShowWarning("Warning", "Please be careful");

        // Assert
        var toasts = service.GetToasts();
        var toast = Assert.Single(toasts);
        Assert.Equal("Warning", toast.Title);
        Assert.Equal("Please be careful", toast.Message);
        Assert.Equal(Color.Warning, toast.Color);
        Assert.Equal(Icon.Exclamation_Triangle, toast.Icon);
    }

    [Fact]
    public void ShowInfo_CreatesInfoToast()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.ShowInfo("Info", "Here's some information");

        // Assert
        var toasts = service.GetToasts();
        var toast = Assert.Single(toasts);
        Assert.Equal("Info", toast.Title);
        Assert.Equal("Here's some information", toast.Message);
        Assert.Equal(Color.Primary, toast.Color);
        Assert.Equal(Icon.Info_Circle, toast.Icon);
    }

    [Fact]
    public void Show_CreatesCustomToast()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.Show("Custom", "Custom message", Color.Accent, Icon.Heart, 2000);

        // Assert
        var toasts = service.GetToasts();
        var toast = Assert.Single(toasts);
        Assert.Equal("Custom", toast.Title);
        Assert.Equal("Custom message", toast.Message);
        Assert.Equal(Color.Accent, toast.Color);
        Assert.Equal(Icon.Heart, toast.Icon);
        Assert.Equal(2000, toast.Duration);
    }

    [Fact]
    public void Show_WithNullColor_CreatesToastWithoutColor()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.Show("Title", "Message");

        // Assert
        var toasts = service.GetToasts();
        var toast = Assert.Single(toasts);
        Assert.Null(toast.Color);
        Assert.Null(toast.Icon);
    }

    [Fact]
    public void AllMethods_HandleNullMessages()
    {
        // Arrange
        using var service = new TwToastService();

        // Act
        service.ShowSuccess("Success", null);
        service.ShowError("Error", null);
        service.ShowWarning("Warning", null);
        service.ShowInfo("Info", null);
        service.Show("Custom", null);

        // Assert
        var toasts = service.GetToasts();
        Assert.Equal(5, toasts.Count);
        Assert.All(toasts, toast => Assert.Equal(string.Empty, toast.Message));
    }
}
