// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Utilities;

namespace TwBlazor.A11yTests;

/// <summary>
/// WCAG contrast-ratio math used only to enrich an axe color-contrast failure with a concrete
/// "make this color pass" suggestion (see <see cref="AccessibilityScanTests"/>). Deliberately kept
/// here rather than in the shipped <see cref="ColorConverter"/> - real component consumers never
/// need to compute a minimum-passing color, only this test suite reporting on itself.
/// </summary>
internal static class ContrastCalculator
{
    /// <summary>
    /// Calculates the WCAG relative luminance of an sRGB color (0.0 = black, 1.0 = white), the basis
    /// for <see cref="ContrastRatio(int, int, int, int, int, int)"/>.
    /// </summary>
    /// <param name="r">The red value (0-255).</param>
    /// <param name="g">The green value (0-255).</param>
    /// <param name="b">The blue value (0-255).</param>
    /// <returns>The relative luminance (0.0 to 1.0).</returns>
    /// <remarks>See <see href="https://www.w3.org/TR/WCAG21/#dfn-relative-luminance"/>.</remarks>
    public static double RelativeLuminance(int r, int g, int b)
    {
        double Linearize(int channel)
        {
            var srgb = channel / 255.0;
            return srgb <= 0.03928 ? srgb / 12.92 : Math.Pow((srgb + 0.055) / 1.055, 2.4);
        }

        return 0.2126 * Linearize(r) + 0.7152 * Linearize(g) + 0.0722 * Linearize(b);
    }

    /// <summary>
    /// Calculates the WCAG contrast ratio between two sRGB colors.
    /// </summary>
    /// <returns>The contrast ratio, from 1.0 (identical luminance) to 21.0 (black against white).</returns>
    /// <remarks>See <see href="https://www.w3.org/TR/WCAG21/#dfn-contrast-ratio"/>.</remarks>
    public static double ContrastRatio(int r1, int g1, int b1, int r2, int g2, int b2)
    {
        var l1 = RelativeLuminance(r1, g1, b1);
        var l2 = RelativeLuminance(r2, g2, b2);
        var lighter = Math.Max(l1, l2);
        var darker = Math.Min(l1, l2);
        return (lighter + 0.05) / (darker + 0.05);
    }

    /// <summary>
    /// Calculates the WCAG contrast ratio between two hex colors (an alpha channel, if present, is ignored).
    /// </summary>
    public static double ContrastRatio(string hex1, string hex2)
    {
        var (r1, g1, b1) = HexToRgbComponents(hex1);
        var (r2, g2, b2) = HexToRgbComponents(hex2);
        return ContrastRatio(r1, g1, b1, r2, g2, b2);
    }

    /// <summary>
    /// Finds the smallest lightness adjustment to <paramref name="foregroundHex"/> (keeping its hue and
    /// saturation) that reaches <paramref name="requiredRatio"/> against <paramref name="backgroundHex"/>.
    /// </summary>
    /// <remarks>
    /// Searches only in the direction that already separates the foreground from the background -
    /// a foreground already lighter than the background only gets lighter, one already darker only gets
    /// darker - rather than considering the whole 0-100% lightness range. Crossing over the background's
    /// own luminance would technically also increase contrast, but changes what the color reads as (e.g.
    /// a light lavender-on-dark accent flipping to near-black) rather than just fixing it, which isn't
    /// the fix a designer normally wants from a "make this pass" suggestion.
    /// </remarks>
    /// <returns>
    /// The adjusted hex color and the lightness (0-100) it was moved to, or <see langword="null"/> if
    /// even the extreme end of that direction (pure white or pure black) can't reach
    /// <paramref name="requiredRatio"/> - meaning this hue/saturation needs to change, not just its
    /// lightness, to ever pass against this background.
    /// </returns>
    public static (string Hex, double LightnessPercent)? FindMinimumPassingColor(string foregroundHex, string backgroundHex, double requiredRatio)
    {
        var (fr, fg, fb) = HexToRgbComponents(foregroundHex);
        var (br, bg, bb) = HexToRgbComponents(backgroundHex);
        var (h, s, currentL) = ColorConverter.RgbToHsl(fr, fg, fb);
        var lighten = RelativeLuminance(fr, fg, fb) >= RelativeLuminance(br, bg, bb);

        bool Passes(double l)
        {
            var (rr, rg, rb) = ColorConverter.HslToRgb(h, s, l);
            return ContrastRatio(rr, rg, rb, br, bg, bb) >= requiredRatio;
        }

        var extremeL = lighten ? 1.0 : 0.0;
        if (!Passes(extremeL)) return null;

        // Luminance is monotonic in lightness for a fixed hue/saturation, so binary search converges
        // to the smallest lightness step (in the chosen direction) that reaches the required ratio.
        var low = lighten ? currentL : 0.0;
        var high = lighten ? 1.0 : currentL;
        for (var i = 0; i < 30; i++)
        {
            var mid = (low + high) / 2.0;
            if (Passes(mid) == lighten)
            {
                if (lighten) high = mid; else low = mid;
            }
            else
            {
                if (lighten) low = mid; else high = mid;
            }
        }

        var resultL = lighten ? high : low;
        var (resultR, resultG, resultB) = ColorConverter.HslToRgb(h, s, resultL);
        return ($"#{resultR:X2}{resultG:X2}{resultB:X2}", Math.Round(resultL * 100, 1));
    }

    /// <summary>
    /// Parses a hex color string's RGB components, ignoring any alpha channel.
    /// </summary>
    private static (int r, int g, int b) HexToRgbComponents(string hex)
    {
        hex = hex.TrimStart('#');
        if (hex.Length >= 6)
        {
            var r = Convert.ToInt32(hex[..2], 16);
            var g = Convert.ToInt32(hex[2..4], 16);
            var b = Convert.ToInt32(hex[4..6], 16);
            return (r, g, b);
        }

        throw new FormatException($"'{hex}' is not a valid hex color.");
    }
}
