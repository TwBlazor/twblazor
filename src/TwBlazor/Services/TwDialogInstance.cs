// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// Design and API shape inspired by MudBlazor's dialog instance cascading value
// (https://github.com/MudBlazor/MudBlazor/tree/dev/src/MudBlazor/Services/Dialog), MIT License.

using TwBlazor.Models;

namespace TwBlazor.Services;

/// <summary>
/// Provides dialog content components with access to close or cancel the dialog they are hosted in.
/// </summary>
/// <remarks>
/// Receive this via a cascading parameter in a component shown through <see cref="ITwDialogService"/>:
/// <c>[CascadingParameter] private TwDialogInstance? DialogInstance { get; set; }</c>
/// </remarks>
public sealed class TwDialogInstance(ITwDialogReference reference)
{
    /// <summary>
    /// Gets the unique identifier of the dialog.
    /// </summary>
    public Guid Id => reference.Id;

    /// <summary>
    /// Closes the dialog with a successful, empty result.
    /// </summary>
    public void Close() => reference.Close();

    /// <summary>
    /// Closes the dialog with the specified result.
    /// </summary>
    /// <param name="result">The result to return to the caller awaiting <see cref="ITwDialogReference.Result"/>.</param>
    public void Close(TwDialogResult? result) => reference.Close(result);

    /// <summary>
    /// Closes the dialog with a successful result carrying the specified data.
    /// </summary>
    /// <typeparam name="T">The type of the returned data.</typeparam>
    /// <param name="data">The data to return.</param>
    public void Close<T>(T data) => reference.Close(TwDialogResult.Ok(data));

    /// <summary>
    /// Closes the dialog as canceled.
    /// </summary>
    public void Cancel() => reference.Close(TwDialogResult.Cancel());
}
