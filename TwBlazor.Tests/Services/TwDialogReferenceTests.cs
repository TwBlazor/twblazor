using TwBlazor.Models;
using TwBlazor.Services;

namespace TwBlazor.Tests.Services;

public class TwDialogReferenceTests
{
    [Fact]
    public void Constructor_SetsId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var service = new TwDialogService();

        // Act
        var reference = new TwDialogReference(id, service);

        // Assert
        Assert.Equal(id, reference.Id);
    }

    [Fact]
    public void NewReference_HasNullTitleAndOptions()
    {
        // Arrange & Act
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());

        // Assert
        Assert.Null(reference.Title);
        Assert.Null(reference.Options);
        Assert.Null(reference.RenderFragment);
        Assert.Null(reference.Dialog);
    }

    [Fact]
    public void InjectTitle_SetsTitle()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());

        // Act
        reference.InjectTitle("My Title");

        // Assert
        Assert.Equal("My Title", reference.Title);
    }

    [Fact]
    public void InjectOptions_SetsOptions()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());
        var options = new TwDialogOptions { NoHeader = true };

        // Act
        reference.InjectOptions(options);

        // Assert
        Assert.Same(options, reference.Options);
    }

    [Fact]
    public void InjectDialog_SetsDialogInstance()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());
        var instance = new object();

        // Act
        reference.InjectDialog(instance);

        // Assert
        Assert.Same(instance, reference.Dialog);
    }

    [Fact]
    public void InjectRenderFragment_SetsRenderFragment()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());
        void Fragment(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { }

        // Act
        reference.InjectRenderFragment(Fragment);

        // Assert
        Assert.NotNull(reference.RenderFragment);
    }

    [Fact]
    public void Close_Parameterless_DelegatesToServiceWithOkResult()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = new TwDialogReference(Guid.NewGuid(), service);
        ITwDialogReference? closedReference = null;
        TwDialogResult? closedResult = null;
        service.OnDialogCloseRequested += (dialog, result) =>
        {
            closedReference = dialog;
            closedResult = result;
        };

        // Act
        reference.Close();

        // Assert
        Assert.Same(reference, closedReference);
        Assert.NotNull(closedResult);
        Assert.False(closedResult.Canceled);
    }

    [Fact]
    public void Close_WithResult_DelegatesToServiceWithProvidedResult()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = new TwDialogReference(Guid.NewGuid(), service);
        TwDialogResult? closedResult = null;
        service.OnDialogCloseRequested += (_, result) => closedResult = result;
        var expected = TwDialogResult.Cancel();

        // Act
        reference.Close(expected);

        // Assert
        Assert.Same(expected, closedResult);
    }

    [Fact]
    public async Task Dismiss_CompletesResultTask()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());
        var result = TwDialogResult.Ok("data");

        // Act
        var dismissed = reference.Dismiss(result);

        // Assert
        Assert.True(dismissed);
        Assert.True(reference.Result.IsCompletedSuccessfully);
        Assert.Same(result, await reference.Result);
    }

    [Fact]
    public void Dismiss_ReturnsFalse_WhenAlreadyDismissed()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());
        reference.Dismiss(TwDialogResult.Ok());

        // Act
        var secondDismiss = reference.Dismiss(TwDialogResult.Cancel());

        // Assert
        Assert.False(secondDismiss);
    }

    [Fact]
    public async Task GetReturnValueAsync_ReturnsTypedData()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());
        reference.Dismiss(TwDialogResult.Ok(42));

        // Act
        var value = await reference.GetReturnValueAsync<int>();

        // Assert
        Assert.Equal(42, value);
    }

    [Fact]
    public async Task GetReturnValueAsync_ReturnsDefault_WhenResultIsNull()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());
        reference.Dismiss(null);

        // Act
        var value = await reference.GetReturnValueAsync<int>();

        // Assert
        Assert.Equal(0, value);
    }

    [Fact]
    public async Task GetReturnValueAsync_ReturnsDefault_WhenCastFails()
    {
        // Arrange
        var reference = new TwDialogReference(Guid.NewGuid(), new TwDialogService());
        reference.Dismiss(TwDialogResult.Ok("not-an-int"));

        // Act
        var value = await reference.GetReturnValueAsync<int>();

        // Assert
        Assert.Equal(0, value);
    }
}
