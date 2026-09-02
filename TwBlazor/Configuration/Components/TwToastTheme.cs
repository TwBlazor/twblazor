// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Configuration.Color;
using TwBlazor.Enums;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for toast components (<see cref="TwBlazor.Components.TwToastProvider"/>).
/// Override any property to customize toast styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwToastTheme
{
    /// <summary>
    /// Gets or sets the color palette applied to toasts.
    /// </summary>
    public required TwBlazorPalette Colors { get; set; }

    /// <summary>
    /// Gets or sets the default border radius for toasts.
    /// </summary>
    public Rounded? ToastRounded { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to the toast container.
    /// </summary>
    public required string Container { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to each toast.
    /// </summary>
    public required string Toast { get; set; }

    /// <summary>
    /// Gets or sets the classes for the toast header.
    /// </summary>
    public required string HeaderClasses { get; set; }

    /// <summary>
    /// Gets or sets the classes for the toast title.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the classes for the toast message.
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Gets or sets the classes for the toast timestamp.
    /// </summary>
    public required string Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the classes for the toast icon container.
    /// </summary>
    public required string IconContainer { get; set; }

    /// <summary>
    /// Gets or sets the classes for the close button.
    /// </summary>
    public required string CloseButton { get; set; }

    /// <summary>
    /// Gets or sets a Tailwind CSS width class applied to every individual toast.
    /// </summary>
    public required string ToastWidth { get; set; }
}
