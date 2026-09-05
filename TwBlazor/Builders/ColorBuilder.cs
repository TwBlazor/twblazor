// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Configuration;
using TwBlazor.Configuration.Color;
using TwBlazor.Enums;

namespace TwBlazor.Builders;


/// <summary>
/// Provides utility methods for generating CSS class strings based on color values for various UI elements.
/// </summary>
/// <remarks>The ColorBuilder class includes methods to retrieve CSS class names for text, backgrounds,
/// checkboxes, switches, alerts, and sliders according to a specified color. These methods are intended to standardize
/// the application of color styles in UI components by mapping a Color enumeration value to the appropriate CSS
/// classes. If a null color is provided, the methods return an empty string or a default value, depending on the
/// method. This class is typically used in UI frameworks where consistent color theming is required.</remarks>
public class ColorBuilder(TwBlazorOptions options)
{
    /// <summary>
    /// Resolves a <see cref="Color"/> against a component's own <see cref="TwBlazorPalette"/> (e.g. a
    /// theme's <c>Colors</c> property), falling back to <paramref name="fallback"/> when
    /// <paramref name="color"/> is <see langword="null"/> or not recognized.
    /// </summary>
    /// <param name="color">The color to resolve.</param>
    /// <param name="palette">The component theme's color palette to resolve against.</param>
    /// <param name="fallback">
    /// The value to return when <paramref name="color"/> is <see langword="null"/> or unrecognized -
    /// typically <c>palette.Primary</c> or <see cref="string.Empty"/>, matching the calling component's
    /// existing default.
    /// </param>
    public static string GetPaletteColor(Color? color, TwBlazorPalette palette, string fallback) => color switch
    {
        Color.Primary => palette.Primary,
        Color.Accent => palette.Accent,
        Color.Success => palette.Success,
        Color.Danger => palette.Danger,
        Color.Warning => palette.Warning,
        Color.Info => palette.Info,
        Color.Light => palette.Light,
        Color.Dark => palette.Dark,
        _ => fallback,
    };

    public string GetTextColor(Color? color)
    {
        // The six saturated hues pair their light-mode "-600" shade (text.Medium) with the dark-mode
        // "-200" pastel shade (darkText.Light) rather than darkText.Medium's "-600" shade: this method
        // is used for plain colored text (tabs, icons, links) that can end up directly on a dark
        // surface, and "-600" text doesn't have enough contrast against a dark background - unlike
        // darkText.Medium's other consumers (e.g. Pagination's active button), which pair it with a
        // light pastel background even in dark mode.
        return color switch
        {
            Color.Primary => $"{options.Theme.Colors.TextColors.Medium.Primary} {options.Theme.Colors.DarkTextColors.Light.Primary}",
            Color.Accent => $"{options.Theme.Colors.TextColors.Medium.Accent} {options.Theme.Colors.DarkTextColors.Light.Accent}",
            Color.Success => $"{options.Theme.Colors.TextColors.Medium.Success} {options.Theme.Colors.DarkTextColors.Light.Success}",
            Color.Danger => $"{options.Theme.Colors.TextColors.Medium.Danger} {options.Theme.Colors.DarkTextColors.Light.Danger}",
            Color.Warning => $"{options.Theme.Colors.TextColors.Medium.Warning} {options.Theme.Colors.DarkTextColors.Light.Warning}",
            Color.Info => $"{options.Theme.Colors.TextColors.Medium.Info} {options.Theme.Colors.DarkTextColors.Light.Info}",
            Color.Light => $"{options.Theme.Colors.TextColors.Medium.Light} {options.Theme.Colors.DarkTextColors.Medium.Light}",
            Color.Dark => $"{options.Theme.Colors.TextColors.Medium.Dark} {options.Theme.Colors.DarkTextColors.Medium.Dark}",
            // Empty (not a neutral default) is deliberate: TwIcon is the other consumer of this
            // method, and an icon with no explicit Color is meant to inherit its surrounding
            // element's text color via `currentColor` - forcing one here would break that and mismatch
            // icons against the button/chip/etc text they sit inside. TwTab supplies its own fallback
            // instead, since a tab button has no useful color to inherit otherwise.
            _ => string.Empty,
        };
    }

    /// <summary>
    /// Gets the border color classes associated with the specified color.
    /// </summary>
    /// <param name="color">The color for which to retrieve the border color. If null or an unsupported color, the default (blue) border color is returned.</param>
    /// <returns>A string representing the border color classes for the specified color.</returns>
    public string GetBorderColor(Color? color)
    {
        return color switch
        {
            Color.Primary => options.Theme.Colors.BorderColors.Primary,
            Color.Accent => options.Theme.Colors.BorderColors.Accent,
            Color.Success => options.Theme.Colors.BorderColors.Success,
            Color.Danger => options.Theme.Colors.BorderColors.Danger,
            Color.Warning => options.Theme.Colors.BorderColors.Warning,
            Color.Info => options.Theme.Colors.BorderColors.Info,
            Color.Light => options.Theme.Colors.BorderColors.Light,
            Color.Dark => options.Theme.Colors.BorderColors.Dark,
            _ => options.Theme.Colors.BorderColors.Primary,
        };
    }

    /// <summary>
    /// Generates a focus ring style string based on the specified color.
    /// </summary>
    /// <remarks>This method maps specific colors to their corresponding focus ring styles as defined in the
    /// theme. Buttons in this theme remove the native outline (<c>focus:outline-none</c>), so this ring is the
    /// only visible focus indicator - if a null or unrecognized color is passed, the blue focus ring is used as a
    /// fallback rather than omitting the ring entirely, to avoid keyboard focus becoming invisible.</remarks>
    /// <param name="color">The color used to determine the focus ring style. If null or not recognized, the default (blue) focus ring is used.</param>
    /// <returns>A string representing the focus ring style for the specified color. Falls back to the blue focus ring if the
    /// color is null or not supported.</returns>
    public string GetFocusRing(Color? color)
    {
        return color switch
        {
            Color.Primary => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Primary}",
            Color.Accent => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Accent}",
            Color.Success => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Success}",
            Color.Danger => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Danger}",
            Color.Warning => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Warning}",
            Color.Info => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Info}",
            Color.Light => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Light}",
            Color.Dark => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Dark}",
            _ => $"{options.Theme.Colors.FocusRingBase} {options.Theme.Colors.FocusColors.Primary}"
        };
    }

    /// <summary>
    /// Gets the outlined variant color associated with the specified color.
    /// </summary>
    /// <remarks>This method uses a switch expression to map predefined colors to their corresponding outlined
    /// variant colors in the theme.</remarks>
    /// <param name="color">The color for which to retrieve the outlined variant. If null or an unsupported color, an empty string is returned.</param>
    /// <returns>A string representing the outlined variant color corresponding to the specified color. Returns an empty string if
    /// the color is not recognized.</returns>
    public string GetOutlinedVariantColor(Color? color)
    {
        return color switch
        {
            Color.Primary => options.Theme.Colors.SurfaceColors.Outlined.Primary,
            Color.Accent => options.Theme.Colors.SurfaceColors.Outlined.Accent,
            Color.Success => options.Theme.Colors.SurfaceColors.Outlined.Success,
            Color.Danger => options.Theme.Colors.SurfaceColors.Outlined.Danger,
            Color.Warning => options.Theme.Colors.SurfaceColors.Outlined.Warning,
            Color.Info => options.Theme.Colors.SurfaceColors.Outlined.Info,
            Color.Light => options.Theme.Colors.SurfaceColors.Outlined.Light,
            Color.Dark => options.Theme.Colors.SurfaceColors.Outlined.Dark,
            _ => string.Empty
        };
    }

    /// <summary>
    /// Gets the text variant color associated with the specified color.
    /// </summary>
    /// <remarks>This method uses a switch expression to map predefined colors to their corresponding text
    /// variant colors in the theme.</remarks>
    /// <param name="color">The color for which to retrieve the text variant. If null or an unsupported color, an empty string is returned.</param>
    /// <returns>A string representing the text variant color corresponding to the specified color. Returns an empty string if
    /// the color is not recognized.</returns>
    public string GetTextVariantColor(Color? color)
    {
        return color switch
        {
            Color.Primary => options.Theme.Colors.SurfaceColors.Text.Primary,
            Color.Accent => options.Theme.Colors.SurfaceColors.Text.Accent,
            Color.Success => options.Theme.Colors.SurfaceColors.Text.Success,
            Color.Danger => options.Theme.Colors.SurfaceColors.Text.Danger,
            Color.Warning => options.Theme.Colors.SurfaceColors.Text.Warning,
            Color.Info => options.Theme.Colors.SurfaceColors.Text.Info,
            Color.Light => options.Theme.Colors.SurfaceColors.Text.Light,
            Color.Dark => options.Theme.Colors.SurfaceColors.Text.Dark,
            _ => string.Empty
        };
    }

    /// <summary>
    /// Gets the filled variant color associated with the specified color.
    /// </summary>
    /// <remarks>This method uses a switch expression to map predefined colors to their corresponding filled
    /// variant colors in the theme.</remarks>
    /// <param name="color">The color for which to retrieve the filled variant. If null or an unsupported color, an empty string is returned.</param>
    /// <returns>A string representing the filled variant color corresponding to the specified color. Returns an empty string if
    /// the color is not recognized.</returns>
    public string GetFilledVariantColor(Color? color)
    {
        return color switch
        {
            Color.Primary => options.Theme.Colors.SurfaceColors.Filled.Primary,
            Color.Accent => options.Theme.Colors.SurfaceColors.Filled.Accent,
            Color.Success => options.Theme.Colors.SurfaceColors.Filled.Success,
            Color.Danger => options.Theme.Colors.SurfaceColors.Filled.Danger,
            Color.Warning => options.Theme.Colors.SurfaceColors.Filled.Warning,
            Color.Info => options.Theme.Colors.SurfaceColors.Filled.Info,
            Color.Light => options.Theme.Colors.SurfaceColors.Filled.Light,
            Color.Dark => options.Theme.Colors.SurfaceColors.Filled.Dark,
            _ => string.Empty
        };
    }
}
