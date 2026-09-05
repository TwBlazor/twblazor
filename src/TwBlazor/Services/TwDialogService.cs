// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// Design and API shape inspired by MudBlazor's DialogService
// (https://github.com/MudBlazor/MudBlazor/tree/dev/src/MudBlazor/Services/Dialog), MIT License.

using Microsoft.AspNetCore.Components;
using TwBlazor.Models;

namespace TwBlazor.Services;

/// <summary>
/// Default implementation of <see cref="ITwDialogService"/>.
/// </summary>
/// <remarks>
/// Register a <see cref="Components.TwDialogProvider"/> once in your layout so dialogs created here can render.
/// </remarks>
public class TwDialogService : ITwDialogService
{
    /// <summary>
    /// The message included in the exception thrown when a dialog is shown without a
    /// <see cref="Components.TwDialogProvider"/> in the render tree.
    /// </summary>
    internal const string missingProviderMessage =
        "No TwDialogProvider was found in the active render tree, so this dialog cannot be displayed. " +
        "Add a <TwDialogProvider /> to your layout.";

    /// <inheritdoc />
    public event Func<ITwDialogReference, Task>? DialogInstanceAddedAsync;

    /// <inheritdoc />
    public event Action<ITwDialogReference, TwDialogResult?>? OnDialogCloseRequested;

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync<TComponent>() where TComponent : IComponent =>
        ShowAsync<TComponent>(string.Empty, TwDialogParameters._default, TwDialogOptions._default);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync<TComponent>(string? title) where TComponent : IComponent =>
        ShowAsync<TComponent>(title, TwDialogParameters._default, TwDialogOptions._default);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync<TComponent>(string? title, TwDialogOptions options) where TComponent : IComponent =>
        ShowAsync<TComponent>(title, TwDialogParameters._default, options);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync<TComponent>(TwDialogOptions options) where TComponent : IComponent =>
        ShowAsync<TComponent>(string.Empty, TwDialogParameters._default, options);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync<TComponent>(TwDialogParameters parameters) where TComponent : IComponent =>
        ShowAsync<TComponent>(string.Empty, parameters, TwDialogOptions._default);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync<TComponent>(string? title, TwDialogParameters parameters) where TComponent : IComponent =>
        ShowAsync<TComponent>(title, parameters, TwDialogOptions._default);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync<TComponent>(string? title, TwDialogParameters parameters, TwDialogOptions? options) where TComponent : IComponent =>
        ShowAsync(typeof(TComponent), title, parameters, options ?? TwDialogOptions._default);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync<TComponent>(TwDialogParameters parameters, TwDialogOptions options) where TComponent : IComponent =>
        ShowAsync<TComponent>(string.Empty, parameters, options);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync(Type component) =>
        ShowAsync(component, string.Empty, TwDialogParameters._default, TwDialogOptions._default);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync(Type component, string? title) =>
        ShowAsync(component, title, TwDialogParameters._default, TwDialogOptions._default);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync(Type component, string? title, TwDialogOptions options) =>
        ShowAsync(component, title, TwDialogParameters._default, options);

    /// <inheritdoc />
    public Task<ITwDialogReference> ShowAsync(Type component, string? title, TwDialogParameters parameters) =>
        ShowAsync(component, title, parameters, TwDialogOptions._default);

    /// <inheritdoc />
    public async Task<ITwDialogReference> ShowAsync(Type component, string? title, TwDialogParameters parameters, TwDialogOptions options)
    {
        var dialogReference = await ShowCoreAsync(component, title, parameters, options);
        await dialogReference.RenderCompleteTaskCompletionSource.Task;
        return dialogReference;
    }

    /// <inheritdoc />
    public void Close(ITwDialogReference dialog) => Close(dialog, TwDialogResult.Ok());

    /// <inheritdoc />
    public virtual void Close(ITwDialogReference dialog, TwDialogResult? result) =>
        OnDialogCloseRequested?.Invoke(dialog, result);

    /// <inheritdoc />
    public virtual ITwDialogReference CreateReference() => new TwDialogReference(Guid.NewGuid(), this);

    private async Task<ITwDialogReference> ShowCoreAsync(Type component, string? title, TwDialogParameters parameters, TwDialogOptions options)
    {
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(options);

        if (!typeof(IComponent).IsAssignableFrom(component))
            throw new ArgumentException($"{component.FullName} must implement {nameof(IComponent)}.", nameof(component));

        var dialogReference = CreateReference();
        dialogReference.InjectOptions(options);
        dialogReference.InjectTitle(title);
        dialogReference.InjectRenderFragment(BuildContent(component, parameters, dialogReference));

        var handler = DialogInstanceAddedAsync ?? throw new InvalidOperationException(missingProviderMessage);
        await handler(dialogReference);

        return dialogReference;
    }

    private static RenderFragment BuildContent(Type component, TwDialogParameters parameters, ITwDialogReference dialogReference) => builder =>
    {
        builder.OpenComponent<CascadingValue<TwDialogInstance>>(0);
        builder.AddComponentParameter(1, nameof(CascadingValue<TwDialogInstance>.Value), new TwDialogInstance(dialogReference));
        builder.AddComponentParameter(2, nameof(CascadingValue<TwDialogInstance>.IsFixed), true);
        builder.AddComponentParameter(3, nameof(CascadingValue<TwDialogInstance>.ChildContent), (RenderFragment)(contentBuilder =>
        {
            var sequence = 0;
            contentBuilder.OpenComponent(sequence++, component);
            foreach (var parameter in parameters)
            {
                contentBuilder.AddAttribute(sequence++, parameter.Key, parameter.Value);
            }

            contentBuilder.AddComponentReferenceCapture(sequence, instance => dialogReference.InjectDialog(instance));
            contentBuilder.CloseComponent();
        }));
        builder.CloseComponent();
    };
}
