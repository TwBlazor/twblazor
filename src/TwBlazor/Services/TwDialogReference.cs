// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// Design and API shape inspired by MudBlazor's DialogReference
// (https://github.com/MudBlazor/MudBlazor/tree/dev/src/MudBlazor/Services/Dialog), MIT License.

using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using TwBlazor.Models;

namespace TwBlazor.Services;

/// <summary>
/// Default implementation of <see cref="ITwDialogReference"/>.
/// </summary>
public class TwDialogReference : ITwDialogReference
{
    private readonly TaskCompletionSource<TwDialogResult?> _resultCompletion = new();
    private readonly ITwDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TwDialogReference"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the dialog.</param>
    /// <param name="dialogService">The service used to close the dialog.</param>
    public TwDialogReference(Guid id, ITwDialogService dialogService)
    {
        Id = id;
        _dialogService = dialogService;
    }

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public string? Title { get; private set; }

    /// <inheritdoc />
    public TwDialogOptions? Options { get; private set; }

    /// <inheritdoc />
    public RenderFragment? RenderFragment { get; set; }

    /// <inheritdoc />
    public Task<TwDialogResult?> Result => _resultCompletion.Task;

    /// <inheritdoc />
    public TaskCompletionSource<bool> RenderCompleteTaskCompletionSource { get; } = new();

    /// <inheritdoc />
    public object? Dialog { get; private set; }

    /// <inheritdoc />
    public void Close() => _dialogService.Close(this);

    /// <inheritdoc />
    public void Close(TwDialogResult? result) => _dialogService.Close(this, result);

    /// <inheritdoc />
    public virtual bool Dismiss(TwDialogResult? result) => _resultCompletion.TrySetResult(result);

    /// <inheritdoc />
    public void InjectRenderFragment(RenderFragment renderFragment) => RenderFragment = renderFragment;

    /// <inheritdoc />
    public void InjectDialog(object instance) => Dialog = instance;

    /// <inheritdoc />
    public void InjectOptions(TwDialogOptions options) => Options = options;

    /// <inheritdoc />
    public void InjectTitle(string? title) => Title = title;

    /// <inheritdoc />
    public async Task<T?> GetReturnValueAsync<T>()
    {
        var result = await Result;
        if (result?.Data is T data)
        {
            return data;
        }

        Debug.WriteLine($"Could not cast dialog result to {typeof(T)}, returning default.");
        return default;
    }
}
