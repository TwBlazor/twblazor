// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Models;

namespace TwBlazor.Services;

/// <summary>
/// Service for managing toast notifications.
/// </summary>
public interface ITwToastService : IDisposable
{
    /// <summary>
    /// Event raised when the toast collection changes.
    /// </summary>
    event EventHandler? ToasterChanged;

    /// <summary>
    /// Event raised when the clean-up timer elapses.
    /// </summary>
    event EventHandler? ToasterTimerElapsed;

    /// <summary>
    /// Gets whether there are any active toasts.
    /// </summary>
    bool HasToasts { get; }

    /// <summary>
    /// Gets the current list of active toasts.
    /// </summary>
    /// <returns>List of active toast notifications.</returns>
    List<ToastModel> GetToasts();

    /// <summary>
    /// Adds a new toast notification.
    /// </summary>
    /// <param name="toast">The toast to add.</param>
    void AddToast(ToastModel toast);

    /// <summary>
    /// Removes a specific toast notification.
    /// </summary>
    /// <param name="toast">The toast to remove.</param>
    void ClearToast(ToastModel toast);

    /// <summary>
    /// Clears all active toasts.
    /// </summary>
    void ClearAll();

    /// <summary>
    /// Marks a toast as hovered by the pointer, pausing its auto-dismiss timer.
    /// </summary>
    /// <param name="toastId">The <see cref="ToastModel.Id"/> of the toast to pause.</param>
    /// <remarks>
    /// Hover and focus are tracked independently (see <see cref="PauseToastForFocus"/>) so that a toast
    /// which still has keyboard focus doesn't have its timer resumed just because the pointer moved away.
    /// </remarks>
    void PauseToastForHover(Guid toastId);

    /// <summary>
    /// Marks a toast as no longer hovered by the pointer, resuming its auto-dismiss timer only if the
    /// toast also doesn't currently have keyboard focus.
    /// </summary>
    /// <param name="toastId">The <see cref="ToastModel.Id"/> of the toast to resume.</param>
    void ResumeToastForHover(Guid toastId);

    /// <summary>
    /// Marks a toast as having keyboard focus, pausing its auto-dismiss timer.
    /// </summary>
    /// <param name="toastId">The <see cref="ToastModel.Id"/> of the toast to pause.</param>
    /// <remarks>
    /// Hover and focus are tracked independently (see <see cref="PauseToastForHover"/>) so that a toast
    /// still being hovered doesn't have its timer resumed just because keyboard focus left it.
    /// </remarks>
    void PauseToastForFocus(Guid toastId);

    /// <summary>
    /// Marks a toast as no longer having keyboard focus, resuming its auto-dismiss timer only if the
    /// pointer also isn't currently hovering the toast.
    /// </summary>
    /// <param name="toastId">The <see cref="ToastModel.Id"/> of the toast to resume.</param>
    void ResumeToastForFocus(Guid toastId);
}
