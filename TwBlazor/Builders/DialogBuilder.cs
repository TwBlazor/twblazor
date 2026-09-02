// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// Design and API shape inspired by MudBlazor's dialog service styling concerns
// (https://github.com/MudBlazor/MudBlazor/tree/dev/src/MudBlazor/Services/Dialog), MIT License.

using TwBlazor.Configuration;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Builders;

/// <summary>
/// Provides utility methods for building dialog CSS classes.
/// </summary>
public class DialogBuilder(TwBlazorOptions options, RoundedBuilder roundedBuilder, ShadowBuilder shadowBuilder)
{
    private TwDialogTheme theme => options.Theme.Components.Require<TwDialogTheme>();

    /// <summary>
    /// Gets the classes for the fixed backdrop/positioning overlay that hosts a dialog.
    /// </summary>
    /// <param name="position">The screen position of the dialog.</param>
    /// <param name="backdropClass">Additional custom classes to append.</param>
    public string GetBackdropClasses(DialogPosition? position, string? backdropClass = null)
    {
        return new ClassBuilder(theme.Backdrop)
            .AddClass(GetPositionClasses(position))
            .AddClass(backdropClass ?? string.Empty, !string.IsNullOrWhiteSpace(backdropClass))
            .Build();
    }

    /// <summary>
    /// Gets the classes for the dialog surface (the visible card containing the dialog content).
    /// </summary>
    /// <param name="maxWidth">The maximum width breakpoint for the dialog.</param>
    /// <param name="fullWidth">Whether the dialog should stretch to fill <paramref name="maxWidth"/>.</param>
    /// <param name="fullScreen">Whether the dialog should stretch to fill the entire screen.</param>
    /// <param name="rounded">Instance-level rounded override.</param>
    /// <param name="shadow">Instance-level shadow override.</param>
    /// <param name="customClass">Additional custom classes to append.</param>
    public string GetSurfaceClasses(DialogMaxWidth? maxWidth, bool fullWidth, bool fullScreen, Rounded? rounded, Shadow? shadow, string? customClass = null)
    {
        var effectiveShadow = shadow ?? theme.DialogShadow ?? options.Theme.Shadows.DefaultShadow;

        var builder = new ClassBuilder(theme.Surface)
            .AddClass(shadowBuilder.GetShadow(effectiveShadow))
            .AddClass(theme.FullScreen, fullScreen);

        if (fullScreen)
        {
            builder = builder.AddClass(options.Theme.Rounded.None);
        }
        else
        {
            var effectiveRounded = rounded ?? theme.DialogRounded ?? options.Theme.Rounded.DefaultRounded;
            builder = builder
                .AddClass(roundedBuilder.GetRounded(effectiveRounded))
                .AddClass(GetMaxWidthClasses(maxWidth))
                .AddClass(theme.FullWidth, fullWidth);
        }

        return builder
            .AddClass(customClass ?? string.Empty, !string.IsNullOrWhiteSpace(customClass))
            .Build();
    }

    /// <summary>
    /// Gets the Tailwind CSS max-width class for the specified breakpoint.
    /// </summary>
    /// <param name="maxWidth">The maximum width breakpoint.</param>
    public string GetMaxWidthClasses(DialogMaxWidth? maxWidth) => maxWidth switch
    {
        DialogMaxWidth.Small => theme.SmallWidth,
        DialogMaxWidth.Medium => theme.MediumWidth,
        DialogMaxWidth.Large => theme.LargeWidth,
        DialogMaxWidth.False => string.Empty,
        null => theme.SmallWidth,
        _ => theme.SmallWidth
    };

    /// <summary>
    /// Gets the flex alignment classes used to position the dialog surface within the backdrop.
    /// </summary>
    /// <param name="position">The screen position of the dialog.</param>
    public string GetPositionClasses(DialogPosition? position) => position switch
    {
        DialogPosition.Center => options.Theme.Position.Center,
        DialogPosition.CenterLeft => options.Theme.Position.CenterLeft,
        DialogPosition.CenterRight => options.Theme.Position.CenterRight,
        DialogPosition.TopCenter => options.Theme.Position.TopCenter,
        DialogPosition.TopLeft => options.Theme.Position.TopLeft,
        DialogPosition.TopRight => options.Theme.Position.TopRight,
        DialogPosition.BottomCenter => options.Theme.Position.BottomCenter,
        DialogPosition.BottomLeft => options.Theme.Position.BottomLeft,
        DialogPosition.BottomRight => options.Theme.Position.BottomRight,
        _ => options.Theme.Position.Center
    };
}
