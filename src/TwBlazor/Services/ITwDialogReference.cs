// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// Design and API shape inspired by MudBlazor's IDialogReference
// (https://github.com/MudBlazor/MudBlazor/tree/dev/src/MudBlazor/Services/Dialog), MIT License.

using Microsoft.AspNetCore.Components;
using TwBlazor.Models;

namespace TwBlazor.Services;

/// <summary>
/// A handle to an active dialog instance shown via <see cref="ITwDialogService"/>.
/// </summary>
public interface ITwDialogReference
{
    /// <summary>
    /// Gets the unique identifier of this dialog.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the title displayed in the dialog header.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// Gets the options used to display this dialog.
    /// </summary>
    TwDialogOptions? Options { get; }

    /// <summary>
    /// Gets or sets the rendered content of the dialog.
    /// </summary>
    RenderFragment? RenderFragment { get; set; }

    /// <summary>
    /// Gets a task that completes with the result once the dialog closes.
    /// </summary>
    Task<TwDialogResult?> Result { get; }

    /// <summary>
    /// Gets a task source that completes once the dialog has rendered for the first time.
    /// </summary>
    TaskCompletionSource<bool> RenderCompleteTaskCompletionSource { get; }

    /// <summary>
    /// Gets the dialog content component instance, once rendered.
    /// </summary>
    object? Dialog { get; }

    /// <summary>
    /// Closes the dialog with a successful, empty result.
    /// </summary>
    void Close();

    /// <summary>
    /// Closes the dialog with the specified result.
    /// </summary>
    /// <param name="result">The result to complete <see cref="Result"/> with.</param>
    void Close(TwDialogResult? result);

    /// <summary>
    /// Marks the dialog as dismissed, completing <see cref="Result"/>.
    /// </summary>
    /// <param name="result">The result to complete <see cref="Result"/> with.</param>
    /// <returns><c>true</c> if this call set the result; <c>false</c> if it was already set.</returns>
    bool Dismiss(TwDialogResult? result);

    /// <summary>
    /// Replaces the rendered content of the dialog.
    /// </summary>
    /// <param name="renderFragment">The new content to render.</param>
    void InjectRenderFragment(RenderFragment renderFragment);

    /// <summary>
    /// Assigns the dialog content component instance.
    /// </summary>
    /// <param name="instance">The rendered dialog content component.</param>
    void InjectDialog(object instance);

    /// <summary>
    /// Assigns the options used to display this dialog.
    /// </summary>
    /// <param name="options">The options to use.</param>
    void InjectOptions(TwDialogOptions options);

    /// <summary>
    /// Assigns the title displayed in the dialog header.
    /// </summary>
    /// <param name="title">The title to display.</param>
    void InjectTitle(string? title);

    /// <summary>
    /// Gets the result data cast to <typeparamref name="T"/>, once the dialog closes.
    /// </summary>
    /// <typeparam name="T">The expected type of the returned data.</typeparam>
    Task<T?> GetReturnValueAsync<T>();
}
