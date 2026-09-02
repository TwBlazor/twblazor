using TwBlazor.Models;
using TwBlazor.Services;

namespace TwBlazor.Tests.Services;

public class TwDialogInstanceTests
{
    [Fact]
    public void Id_ReturnsReferenceId()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = new TwDialogReference(Guid.NewGuid(), service);
        var instance = new TwDialogInstance(reference);

        // Act & Assert
        Assert.Equal(reference.Id, instance.Id);
    }

    [Fact]
    public void Close_Parameterless_ClosesReferenceWithOkResult()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = new TwDialogReference(Guid.NewGuid(), service);
        var instance = new TwDialogInstance(reference);
        TwDialogResult? closedResult = null;
        service.OnDialogCloseRequested += (_, result) => closedResult = result;

        // Act
        instance.Close();

        // Assert
        Assert.NotNull(closedResult);
        Assert.False(closedResult.Canceled);
    }

    [Fact]
    public void Close_WithResult_PassesResultThrough()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = new TwDialogReference(Guid.NewGuid(), service);
        var instance = new TwDialogInstance(reference);
        TwDialogResult? closedResult = null;
        service.OnDialogCloseRequested += (_, result) => closedResult = result;
        var expected = TwDialogResult.Ok("payload");

        // Act
        instance.Close(expected);

        // Assert
        Assert.Same(expected, closedResult);
    }

    [Fact]
    public void CloseGeneric_WrapsDataInOkResult()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = new TwDialogReference(Guid.NewGuid(), service);
        var instance = new TwDialogInstance(reference);
        TwDialogResult? closedResult = null;
        service.OnDialogCloseRequested += (_, result) => closedResult = result;

        // Act
        instance.Close(123);

        // Assert
        Assert.NotNull(closedResult);
        Assert.False(closedResult.Canceled);
        Assert.Equal(123, closedResult.Data);
    }

    [Fact]
    public void Cancel_ClosesReferenceWithCanceledResult()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = new TwDialogReference(Guid.NewGuid(), service);
        var instance = new TwDialogInstance(reference);
        TwDialogResult? closedResult = null;
        service.OnDialogCloseRequested += (_, result) => closedResult = result;

        // Act
        instance.Cancel();

        // Assert
        Assert.NotNull(closedResult);
        Assert.True(closedResult.Canceled);
    }
}
