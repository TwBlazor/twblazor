// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Configuration;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Builders;

/// <summary>
/// Builds the shared "box" classes (background, border, rounded corners, shadow) for every popover
/// panel (date/time/color picker panels) from <see cref="TwOverlayTheme"/>. Mirrors
/// <see cref="DialogBuilder.GetSurfaceClasses"/>'s instance-override/theme-default/global-default
/// resolution, so popovers and the modal dialog behave the same way instead of the popover family
/// having no such override tier at all.
/// </summary>
public class PopoverBuilder(TwBlazorOptions options, RoundedBuilder roundedBuilder, ShadowBuilder shadowBuilder)
{
    private TwOverlayTheme theme => options.Theme.Components.Require<TwOverlayTheme>();

    /// <summary>
    /// Gets the classes for a popover panel's surface (background, border, rounded corners, shadow).
    /// </summary>
    /// <param name="rounded">Instance-level rounded override.</param>
    /// <param name="shadow">Instance-level shadow override.</param>
    /// <param name="customClass">Additional custom classes to append.</param>
    public string GetSurfaceClasses(Rounded? rounded, Shadow? shadow, string? customClass = null)
    {
        var effectiveRounded = rounded ?? theme.PopoverRounded ?? options.Theme.Rounded.DefaultRounded;
        var effectiveShadow = shadow ?? theme.PopoverShadow ?? options.Theme.Shadows.DefaultShadow;

        return new ClassBuilder(theme.PopoverBackground)
            .AddClass(theme.PopoverBorder)
            .AddClass(roundedBuilder.GetRounded(effectiveRounded))
            .AddClass(shadowBuilder.GetShadow(effectiveShadow))
            .AddClass(customClass ?? string.Empty, !string.IsNullOrWhiteSpace(customClass))
            .Build();
    }
}
