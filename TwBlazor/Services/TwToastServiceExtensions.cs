// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Enums;
using TwBlazor.Models;

namespace TwBlazor.Services;

/// <summary>
/// Extension methods for <see cref="ITwToastService"/> to simplify toast creation.
/// </summary>
public static class TwToastServiceExtensions
{
    /// <summary>
    /// Shows a success toast notification.
    /// </summary>
    /// <param name="service">The toast service.</param>
    /// <param name="title">The title of the toast.</param>
    /// <param name="message">The message content.</param>
    /// <param name="duration">The duration in milliseconds before auto-dismiss. Default is 5000ms.</param>
    public static void ShowSuccess(this ITwToastService service, string title, string? message = null, int duration = 5000)
    {
        service.AddToast(new ToastModel
        {
            Title = title,
            Message = message ?? string.Empty,
            Color = Color.Success,
            Icon = Icon.Check_Circle,
            Duration = duration
        });
    }

    /// <summary>
    /// Shows an error toast notification.
    /// </summary>
    /// <param name="service">The toast service.</param>
    /// <param name="title">The title of the toast.</param>
    /// <param name="message">The message content.</param>
    /// <param name="duration">The duration in milliseconds before auto-dismiss. Default is 5000ms.</param>
    public static void ShowError(this ITwToastService service, string title, string? message = null, int duration = 5000)
    {
        service.AddToast(new ToastModel
        {
            Title = title,
            Message = message ?? string.Empty,
            Color = Color.Danger,
            Icon = Icon.X_Circle,
            Duration = duration
        });
    }

    /// <summary>
    /// Shows a warning toast notification.
    /// </summary>
    /// <param name="service">The toast service.</param>
    /// <param name="title">The title of the toast.</param>
    /// <param name="message">The message content.</param>
    /// <param name="duration">The duration in milliseconds before auto-dismiss. Default is 5000ms.</param>
    public static void ShowWarning(this ITwToastService service, string title, string? message = null, int duration = 5000)
    {
        service.AddToast(new ToastModel
        {
            Title = title,
            Message = message ?? string.Empty,
            Color = Color.Warning,
            Icon = Icon.Exclamation_Triangle,
            Duration = duration
        });
    }

    /// <summary>
    /// Shows an info toast notification.
    /// </summary>
    /// <param name="service">The toast service.</param>
    /// <param name="title">The title of the toast.</param>
    /// <param name="message">The message content.</param>
    /// <param name="duration">The duration in milliseconds before auto-dismiss. Default is 5000ms.</param>
    public static void ShowInfo(this ITwToastService service, string title, string? message = null, int duration = 5000)
    {
        service.AddToast(new ToastModel
        {
            Title = title,
            Message = message ?? string.Empty,
            Color = Color.Primary,
            Icon = Icon.Info_Circle,
            Duration = duration
        });
    }

    /// <summary>
    /// Shows a custom toast notification.
    /// </summary>
    /// <param name="service">The toast service.</param>
    /// <param name="title">The title of the toast.</param>
    /// <param name="message">The message content.</param>
    /// <param name="color">The color theme of the toast.</param>
    /// <param name="icon">The icon to display.</param>
    /// <param name="duration">The duration in milliseconds before auto-dismiss. Default is 5000ms.</param>
    public static void Show(this ITwToastService service, string title, string? message = null, Color? color = null, Icon? icon = null, int duration = 5000)
    {
        service.AddToast(new ToastModel
        {
            Title = title,
            Message = message ?? string.Empty,
            Color = color,
            Icon = icon,
            Duration = duration
        });
    }
}
