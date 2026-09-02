// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// Design and API shape inspired by MudBlazor's IDialogService
// (https://github.com/MudBlazor/MudBlazor/tree/dev/src/MudBlazor/Services/Dialog), MIT License.

using Microsoft.AspNetCore.Components;
using TwBlazor.Models;

namespace TwBlazor.Services;

/// <summary>
/// Shows and closes dialogs rendered through <see cref="Components.TwDialog"/> from C# code.
/// </summary>
/// <remarks>
/// This service requires a <see cref="Components.TwDialogProvider"/> in the active render tree, typically placed
/// once in your layout.
/// </remarks>
public interface ITwDialogService
{
    /// <summary>
    /// Occurs when a new dialog instance is created and needs to be rendered.
    /// </summary>
    event Func<ITwDialogReference, Task>? DialogInstanceAddedAsync;

    /// <summary>
    /// Occurs when a request is made to close a dialog.
    /// </summary>
    event Action<ITwDialogReference, TwDialogResult?>? OnDialogCloseRequested;

    /// <summary>
    /// Displays a dialog.
    /// </summary>
    /// <typeparam name="TComponent">The dialog content to display.</typeparam>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync<TComponent>() where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog with a custom title.
    /// </summary>
    /// <typeparam name="TComponent">The dialog content to display.</typeparam>
    /// <param name="title">The text at the top of the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync<TComponent>(string? title) where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog with a custom title and options.
    /// </summary>
    /// <typeparam name="TComponent">The dialog content to display.</typeparam>
    /// <param name="title">The text at the top of the dialog.</param>
    /// <param name="options">The custom display options for the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync<TComponent>(string? title, TwDialogOptions options) where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog with options.
    /// </summary>
    /// <typeparam name="TComponent">The dialog content to display.</typeparam>
    /// <param name="options">The custom display options for the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync<TComponent>(TwDialogOptions options) where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog with parameters.
    /// </summary>
    /// <typeparam name="TComponent">The dialog content to display.</typeparam>
    /// <param name="parameters">The custom parameters to set on the dialog content.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync<TComponent>(TwDialogParameters parameters) where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog with a custom title and parameters.
    /// </summary>
    /// <typeparam name="TComponent">The dialog content to display.</typeparam>
    /// <param name="title">The text at the top of the dialog.</param>
    /// <param name="parameters">The custom parameters to set on the dialog content.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync<TComponent>(string? title, TwDialogParameters parameters) where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog with a custom title, parameters, and options.
    /// </summary>
    /// <typeparam name="TComponent">The dialog content to display.</typeparam>
    /// <param name="title">The text at the top of the dialog.</param>
    /// <param name="parameters">The custom parameters to set on the dialog content.</param>
    /// <param name="options">The custom display options for the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync<TComponent>(string? title, TwDialogParameters parameters, TwDialogOptions? options) where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog with parameters and options.
    /// </summary>
    /// <typeparam name="TComponent">The dialog content to display.</typeparam>
    /// <param name="parameters">The custom parameters to set on the dialog content.</param>
    /// <param name="options">The custom display options for the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync<TComponent>(TwDialogParameters parameters, TwDialogOptions options) where TComponent : IComponent;

    /// <summary>
    /// Displays a dialog for the specified component type.
    /// </summary>
    /// <param name="component">The dialog content to display.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync(Type component);

    /// <summary>
    /// Displays a dialog for the specified component type with a custom title.
    /// </summary>
    /// <param name="component">The dialog content to display.</param>
    /// <param name="title">The text at the top of the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync(Type component, string? title);

    /// <summary>
    /// Displays a dialog for the specified component type with a custom title and options.
    /// </summary>
    /// <param name="component">The dialog content to display.</param>
    /// <param name="title">The text at the top of the dialog.</param>
    /// <param name="options">The custom display options for the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync(Type component, string? title, TwDialogOptions options);

    /// <summary>
    /// Displays a dialog for the specified component type with a custom title and parameters.
    /// </summary>
    /// <param name="component">The dialog content to display.</param>
    /// <param name="title">The text at the top of the dialog.</param>
    /// <param name="parameters">The custom parameters to set on the dialog content.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync(Type component, string? title, TwDialogParameters parameters);

    /// <summary>
    /// Displays a dialog for the specified component type with a custom title, parameters, and options.
    /// </summary>
    /// <param name="component">The dialog content to display.</param>
    /// <param name="title">The text at the top of the dialog.</param>
    /// <param name="parameters">The custom parameters to set on the dialog content.</param>
    /// <param name="options">The custom display options for the dialog.</param>
    /// <returns>A reference to the dialog.</returns>
    Task<ITwDialogReference> ShowAsync(Type component, string? title, TwDialogParameters parameters, TwDialogOptions options);

    /// <summary>
    /// Creates a dialog reference without showing it.
    /// </summary>
    /// <returns>The created dialog reference.</returns>
    ITwDialogReference CreateReference();

    /// <summary>
    /// Closes the specified dialog with a successful, empty result.
    /// </summary>
    /// <param name="dialog">The reference of the dialog to close.</param>
    void Close(ITwDialogReference dialog);

    /// <summary>
    /// Closes the specified dialog with the given result.
    /// </summary>
    /// <param name="dialog">The reference of the dialog to close.</param>
    /// <param name="result">The result to include.</param>
    void Close(ITwDialogReference dialog, TwDialogResult? result);
}
