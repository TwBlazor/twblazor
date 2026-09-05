// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.JSInterop;

namespace TwBlazor.Utilities;

/// <summary>
/// Detects whether the current client platform should use its own native input UI (e.g. the iOS/Android
/// date, time, and color pickers) instead of a TwBlazor picker's custom popover.
/// </summary>
public static class DeviceDetector
{
    /// <summary>
    /// Asks the browser, via JS interop, whether the current device is a mobile platform (iOS or Android)
    /// that provides its own native picker UI.
    /// </summary>
    /// <param name="jsRuntime">The JS runtime used to query the client platform.</param>
    /// <param name="cancellationToken">A token used to cancel the interop call.</param>
    /// <returns>
    /// <see langword="true"/> if the client is running iOS or Android; otherwise <see langword="false"/>.
    /// Also returns <see langword="false"/> if the JS runtime is unavailable (e.g. during prerendering or in
    /// unit tests), so callers safely fall back to the custom picker UI.
    /// </returns>
    public static async Task<bool> PrefersNativePickerAsync(IJSRuntime jsRuntime, CancellationToken cancellationToken = default)
    {
        try
        {
            return await jsRuntime.InvokeAsync<bool>("twDevice.prefersNativePicker", cancellationToken);
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (JSException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}
