using Microsoft.JSInterop;
using TwBlazor.Utilities;

namespace TwBlazor.Tests.Utilities;

public class DeviceDetectorTests
{
    private sealed class StubJSRuntime(bool result) : IJSRuntime
    {
        public string? LastIdentifier { get; private set; }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            LastIdentifier = identifier;
            return ValueTask.FromResult((TValue)(object)result);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            LastIdentifier = identifier;
            return ValueTask.FromResult((TValue)(object)result);
        }
    }

    private sealed class ThrowingJSRuntime(Exception exception) : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => throw exception;

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => throw exception;
    }

    [Fact]
    public async Task PrefersNativePickerAsync_InvokesTwDeviceIdentifier_AndReturnsResult()
    {
        // Arrange
        var jsRuntime = new StubJSRuntime(true);

        // Act
        var result = await DeviceDetector.PrefersNativePickerAsync(jsRuntime, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result);
        Assert.Equal("twDevice.prefersNativePicker", jsRuntime.LastIdentifier);
    }

    [Fact]
    public async Task PrefersNativePickerAsync_ReturnsFalse_WhenJsReturnsFalse()
    {
        // Arrange
        var jsRuntime = new StubJSRuntime(false);

        // Act
        var result = await DeviceDetector.PrefersNativePickerAsync(jsRuntime, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(SuppressedExceptionTypes))]
    public async Task PrefersNativePickerAsync_SuppressesJsInteropFailures_AndReturnsFalse(Type exceptionType)
    {
        // Arrange — these are the failure modes possible when JS interop is unavailable
        // (e.g. prerendering, disposal races, or a missing/unloaded twblazor.js in unit tests).
        var exception = (Exception)Activator.CreateInstance(exceptionType, "failure")!;
        var jsRuntime = new ThrowingJSRuntime(exception);

        // Act
        var result = await DeviceDetector.PrefersNativePickerAsync(jsRuntime, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
    }

    public static TheoryData<Type> SuppressedExceptionTypes =>
    [
        typeof(JSException),
        typeof(InvalidOperationException),
        typeof(TaskCanceledException),
    ];
}
