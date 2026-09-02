// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Configuration;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Builders;

/// <summary>
/// Provides utility methods for generating CSS shadow classes based on elevation levels.
/// </summary>
public class ShadowBuilder(TwBlazorOptions options)
{
    /// <summary>
    /// Gets the Tailwind CSS shadow class for the specified shadow level.
    /// </summary>
    /// <param name="shadow">The shadow level.</param>
    /// <returns>The corresponding Tailwind CSS class.</returns>
    public string GetShadow(Shadow? shadow) => shadow switch
    {
        Shadow.None => options.Theme.Shadows.None,
        Shadow.Sm => options.Theme.Shadows.Sm,
        Shadow.Md => options.Theme.Shadows.Md,
        Shadow.Lg => options.Theme.Shadows.Lg,
        null => string.Empty,
        _ => options.Theme.Shadows.Sm
    };

    /// <summary>
    /// Gets the Tailwind CSS shadow classes for button states from theme configuration.
    /// </summary>
    /// <param name="buttonTheme">The button theme configuration containing shadow settings.</param>
    /// <param name="overrideShadow">
    /// When set, overrides the theme's default button shadow (e.g. an explicit <see cref="Shadow"/> parameter
    /// passed to the component instance). Leave <see langword="null"/> to fall back to <paramref
    /// name="buttonTheme"/>'s configured shadow.
    /// </param>
    /// <param name="includeHover">Whether to include hover state shadow.</param>
    /// <param name="includeActive">Whether to include active state shadow.</param>
    /// <returns>The corresponding Tailwind CSS classes.</returns>
    public string GetButtonShadow(TwButtonTheme? buttonTheme, Shadow? overrideShadow = null, bool includeHover = true, bool includeActive = true)
    {
        var shadow = overrideShadow ?? buttonTheme?.ButtonShadow;
        var baseShadow = GetShadow(shadow);
        if (string.IsNullOrEmpty(baseShadow) || shadow == Shadow.None)
            return string.Empty;

        var builder = new ClassBuilder(baseShadow);

        if (includeHover)
        {
            var hoverShadow = shadow switch
            {
                Shadow.Sm => options.Theme.Shadows.HoverSm,
                Shadow.Md => options.Theme.Shadows.HoverMd,
                Shadow.Lg => options.Theme.Shadows.HoverLg,
                _ => options.Theme.Shadows.None
            };
            builder.AddClass(hoverShadow);
        }

        builder.AddClass(options.Theme.Shadows.ActiveMd, includeActive);

        return builder.Build();
    }
}
