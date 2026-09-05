// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Timers;
using TwBlazor.Models;

namespace TwBlazor.Services;

/// <summary>
/// Service for managing toast notifications.
/// </summary>
public sealed class TwToastService : ITwToastService
{
    private readonly System.Timers.Timer _timer;
    private readonly List<ToastModel> _toastList = [];
    private readonly object _lock = new();

    /// <summary>
    /// Event raised when the toast collection changes.
    /// </summary>
    public event EventHandler? ToasterChanged;

    /// <summary>
    /// Event raised when the clean-up timer elapses.
    /// </summary>
    public event EventHandler? ToasterTimerElapsed;

    /// <summary>
    /// Gets whether there are any active toasts.
    /// </summary>
    public bool HasToasts
    {
        get
        {
            lock (_lock)
            {
                return _toastList.Count > 0;
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TwToastService"/> class.
    /// </summary>
    public TwToastService()
    {
        _timer = new System.Timers.Timer
        {
            Interval = 1000, // Check every second for expired toasts
            AutoReset = true
        };
        _timer.Elapsed += OnTimerElapsed;
        _timer.Start();
    }

    /// <summary>
    /// Gets the current list of active toasts.
    /// </summary>
    /// <returns>List of active toast notifications.</returns>
    public List<ToastModel> GetToasts()
    {
        lock (_lock)
        {
            ClearBurntToast();
            return [.. _toastList];
        }
    }

    /// <summary>
    /// Adds a new toast notification.
    /// </summary>
    /// <param name="toast">The toast to add.</param>
    public void AddToast(ToastModel toast)
    {
        ArgumentNullException.ThrowIfNull(toast);

        lock (_lock)
        {
            _toastList.Add(toast);
        }

        // Only raise the event if it hasn't been raised by ClearBurntToast
        if (!ClearBurntToast())
            ToasterChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Removes a specific toast notification.
    /// </summary>
    /// <param name="toast">The toast to remove.</param>
    public void ClearToast(ToastModel toast)
    {
        if (toast == null)
            return;

        bool removed;
        lock (_lock)
        {
            removed = _toastList.Remove(toast);
        }

        // Only raise the event if it hasn't been raised by ClearBurntToast
        if (removed && !ClearBurntToast())
            ToasterChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Clears all active toasts.
    /// </summary>
    public void ClearAll()
    {
        bool hadToasts;
        lock (_lock)
        {
            hadToasts = _toastList.Count > 0;
            _toastList.Clear();
        }

        if (hadToasts)
            ToasterChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public void PauseToastForHover(Guid toastId)
    {
        lock (_lock)
        {
            _toastList.FirstOrDefault(t => t.Id == toastId)?.PauseForHover();
        }
    }

    /// <inheritdoc />
    public void ResumeToastForHover(Guid toastId)
    {
        lock (_lock)
        {
            _toastList.FirstOrDefault(t => t.Id == toastId)?.ResumeForHover();
        }
    }

    /// <inheritdoc />
    public void PauseToastForFocus(Guid toastId)
    {
        lock (_lock)
        {
            _toastList.FirstOrDefault(t => t.Id == toastId)?.PauseForFocus();
        }
    }

    /// <inheritdoc />
    public void ResumeToastForFocus(Guid toastId)
    {
        lock (_lock)
        {
            _toastList.FirstOrDefault(t => t.Id == toastId)?.ResumeForFocus();
        }
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        ClearBurntToast();
        ToasterTimerElapsed?.Invoke(this, EventArgs.Empty);
    }

    private bool ClearBurntToast()
    {
        List<ToastModel> toastsToDelete;

        lock (_lock)
        {
            toastsToDelete = [.. _toastList.Where(item => item.IsBurnt)];

            if (toastsToDelete.Count > 0)
            {
                foreach (var toast in toastsToDelete)
                {
                    _toastList.Remove(toast);
                }
            }
        }

        if (toastsToDelete.Count > 0)
        {
            ToasterChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Disposes the service and releases resources.
    /// </summary>
    public void Dispose()
    {
        if (_timer != null)
        {
            _timer.Elapsed -= OnTimerElapsed;
            _timer.Stop();
            _timer.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
