using Bunit;
using TwBlazor.Models;
using TwBlazor.Services;

namespace TwBlazor.Tests.Services;

public class TwDialogServiceTests
{
    private static TwDialogService CreateServiceWithProvider(out List<ITwDialogReference> shown)
    {
        var service = new TwDialogService();
        List<ITwDialogReference> captured = [];
        service.DialogInstanceAddedAsync += reference =>
        {
            captured.Add(reference);
            reference.RenderCompleteTaskCompletionSource.TrySetResult(true);
            return Task.CompletedTask;
        };
        shown = captured;
        return service;
    }

    [Fact]
    public async Task ShowAsync_ThrowsInvalidOperationException_WhenNoProviderSubscribed()
    {
        // Arrange
        var service = new TwDialogService();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.ShowAsync<DialogTestContent>());
        Assert.Equal(TwDialogService.missingProviderMessage, exception.Message);
    }

    [Fact]
    public async Task ShowAsync_Generic_Parameterless_ReturnsReferenceWithEmptyTitleAndDefaultOptions()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);

        // Act
        var reference = await service.ShowAsync<DialogTestContent>();

        // Assert
        Assert.Equal(string.Empty, reference.Title);
        Assert.NotNull(reference.Options);
        Assert.Null(reference.Options.NoHeader);
        Assert.NotNull(reference.RenderFragment);
    }

    [Fact]
    public async Task ShowAsync_WithTitle_SetsTitleOnReference()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);

        // Act
        var reference = await service.ShowAsync<DialogTestContent>("My Dialog");

        // Assert
        Assert.Equal("My Dialog", reference.Title);
    }

    [Fact]
    public async Task ShowAsync_WithTitleAndOptions_SetsBoth()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        var options = new TwDialogOptions { NoHeader = true };

        // Act
        var reference = await service.ShowAsync<DialogTestContent>("Title", options);

        // Assert
        Assert.Equal("Title", reference.Title);
        Assert.Same(options, reference.Options);
    }

    [Fact]
    public async Task ShowAsync_WithOptions_UsesEmptyTitle()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        var options = new TwDialogOptions { FullScreen = true };

        // Act
        var reference = await service.ShowAsync<DialogTestContent>(options);

        // Assert
        Assert.Equal(string.Empty, reference.Title);
        Assert.Same(options, reference.Options);
    }

    [Fact]
    public async Task ShowAsync_WithParameters_PassesParametersToContent()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        var parameters = new TwDialogParameters { ["Message"] = "Hello dialog" };

        // Act
        var reference = await service.ShowAsync<DialogTestContent>(parameters);

        // Assert - render the produced fragment and check the parameter reached the content component
        using var testContext = new Bunit.BunitContext();
        var cut = testContext.Render(reference.RenderFragment!);
        Assert.Contains("Hello dialog", cut.Markup);
    }

    [Fact]
    public async Task ShowAsync_WithTitleAndParameters_SetsTitleAndAppliesParameters()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        var parameters = new TwDialogParameters { ["Message"] = "Hi" };

        // Act
        var reference = await service.ShowAsync<DialogTestContent>("Confirm", parameters);

        // Assert
        Assert.Equal("Confirm", reference.Title);
        using var testContext = new Bunit.BunitContext();
        var cut = testContext.Render(reference.RenderFragment!);
        Assert.Contains("Hi", cut.Markup);
    }

    [Fact]
    public async Task ShowAsync_WithTitleParametersAndOptions_SetsAll()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        var parameters = new TwDialogParameters { ["Message"] = "Body" };
        var options = new TwDialogOptions { CloseButton = false };

        // Act
        var reference = await service.ShowAsync<DialogTestContent>("Title", parameters, options);

        // Assert
        Assert.Equal("Title", reference.Title);
        Assert.Same(options, reference.Options);
    }

    [Fact]
    public async Task ShowAsync_WithTitleParametersAndNullOptions_FallsBackToDefaultOptions()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        TwDialogParameters parameters = [];

        // Act
        var reference = await service.ShowAsync<DialogTestContent>("Title", parameters, null);

        // Assert
        Assert.NotNull(reference.Options);
    }

    [Fact]
    public async Task ShowAsync_WithParametersAndOptions_SetsBothWithEmptyTitle()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        TwDialogParameters parameters = [];
        var options = new TwDialogOptions { NoHeader = true };

        // Act
        var reference = await service.ShowAsync<DialogTestContent>(parameters, options);

        // Assert
        Assert.Equal(string.Empty, reference.Title);
        Assert.Same(options, reference.Options);
    }

    [Fact]
    public async Task ShowAsync_ByType_Parameterless_Works()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);

        // Act
#pragma warning disable CA2263 // Intentionally exercising the runtime Type-based overload, not its generic counterpart
        var reference = await service.ShowAsync(typeof(DialogTestContent));
#pragma warning restore CA2263

        // Assert
        Assert.Equal(string.Empty, reference.Title);
    }

    [Fact]
    public async Task ShowAsync_ByType_WithTitle_Works()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);

        // Act
#pragma warning disable CA2263 // Intentionally exercising the runtime Type-based overload, not its generic counterpart
        var reference = await service.ShowAsync(typeof(DialogTestContent), "Typed Title");
#pragma warning restore CA2263

        // Assert
        Assert.Equal("Typed Title", reference.Title);
    }

    [Fact]
    public async Task ShowAsync_ByType_WithTitleAndOptions_Works()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        var options = new TwDialogOptions { FullWidth = true };

        // Act
#pragma warning disable CA2263 // Intentionally exercising the runtime Type-based overload, not its generic counterpart
        var reference = await service.ShowAsync(typeof(DialogTestContent), "Title", options);
#pragma warning restore CA2263

        // Assert
        Assert.Same(options, reference.Options);
    }

    [Fact]
    public async Task ShowAsync_ByType_WithTitleAndParameters_Works()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        var parameters = new TwDialogParameters { ["Message"] = "Typed body" };

        // Act
#pragma warning disable CA2263 // Intentionally exercising the runtime Type-based overload, not its generic counterpart
        var reference = await service.ShowAsync(typeof(DialogTestContent), "Title", parameters);
#pragma warning restore CA2263

        // Assert
        using var testContext = new Bunit.BunitContext();
        var cut = testContext.Render(reference.RenderFragment!);
        Assert.Contains("Typed body", cut.Markup);
    }

    [Fact]
    public async Task ShowAsync_ByType_ThrowsArgumentException_WhenTypeIsNotAComponent()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.ShowAsync(typeof(string)));
    }

    [Fact]
    public void CreateReference_ReturnsUniqueReferences()
    {
        // Arrange
        var service = new TwDialogService();

        // Act
        var reference1 = service.CreateReference();
        var reference2 = service.CreateReference();

        // Assert
        Assert.NotEqual(reference1.Id, reference2.Id);
    }

    [Fact]
    public void Close_Parameterless_RaisesEventWithOkResult()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = service.CreateReference();
        TwDialogResult? capturedResult = null;
        service.OnDialogCloseRequested += (_, result) => capturedResult = result;

        // Act
        service.Close(reference);

        // Assert
        Assert.NotNull(capturedResult);
        Assert.False(capturedResult.Canceled);
    }

    [Fact]
    public void Close_WithResult_RaisesEventWithProvidedResult()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = service.CreateReference();
        TwDialogResult? capturedResult = null;
        service.OnDialogCloseRequested += (_, result) => capturedResult = result;
        var expected = TwDialogResult.Cancel();

        // Act
        service.Close(reference, expected);

        // Assert
        Assert.Same(expected, capturedResult);
    }

    [Fact]
    public void Close_DoesNotThrow_WhenNoSubscribers()
    {
        // Arrange
        var service = new TwDialogService();
        var reference = service.CreateReference();

        // Act
        var exception = Record.Exception(() => service.Close(reference));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task ShowAsync_InvokesDialogInstanceAddedAsync_ExactlyOnce()
    {
        // Arrange
        var service = CreateServiceWithProvider(out var shown);

        // Act
        await service.ShowAsync<DialogTestContent>();

        // Assert
        Assert.Single(shown);
    }

    [Fact]
    public async Task ShowAsync_ContentComponent_ReceivesDialogInstanceCascadingValue()
    {
        // Arrange
        var service = CreateServiceWithProvider(out _);
        var reference = await service.ShowAsync<DialogTestContent>(new TwDialogParameters { ["Message"] = "Bound" });

        using var testContext = new BunitContext();
        var cut = testContext.Render(reference.RenderFragment!);

        TwDialogResult? capturedResult = null;
        service.OnDialogCloseRequested += (_, result) => capturedResult = result;

        // Act - click the close button rendered by DialogTestContent, which calls DialogInstance.Close(...)
        cut.Find(".dialog-test-close").Click();

        // Assert
        Assert.NotNull(capturedResult);
        Assert.Equal("closed-from-content", capturedResult.Data);
    }
}
