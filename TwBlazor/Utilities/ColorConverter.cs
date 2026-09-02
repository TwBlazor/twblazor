// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Utilities;

/// <summary>
/// Provides utility methods for converting between different color format representations (Hex, RGB, HSL).
/// </summary>
public static class ColorConverter
{
    private const string defaultRgb = "rgb(0, 0, 0)";
    private const string defaultHsl = "hsl(0, 0%, 0%)";
    /// <summary>
    /// Converts a hexadecimal color string to an RGB or RGBA string representation.
    /// </summary>
    /// <param name="hex">The hexadecimal color string (e.g., "#FF5733" or "#FF5733AA").</param>
    /// <param name="includeAlpha">Whether to include alpha channel in the output if present in the hex string.</param>
    /// <returns>An RGB or RGBA string representation (e.g., "rgb(255, 87, 51)" or "rgba(255, 87, 51, 0.67)").</returns>
    public static string HexToRgb(string hex, bool includeAlpha = false)
    {
        if (string.IsNullOrEmpty(hex) || !hex.StartsWith('#')) return defaultRgb;

        try
        {
            hex = hex.TrimStart('#');

            if (hex.Length == 8 && !includeAlpha)
            {
                hex = hex[..6];
            }

            if (hex.Length == 6)
            {
                var r = Convert.ToInt32(hex[..2], 16);
                var g = Convert.ToInt32(hex[2..4], 16);
                var b = Convert.ToInt32(hex[4..6], 16);
                return $"rgb({r}, {g}, {b})";
            }
            else if (hex.Length == 8 && includeAlpha)
            {
                var r = Convert.ToInt32(hex[..2], 16);
                var g = Convert.ToInt32(hex[2..4], 16);
                var b = Convert.ToInt32(hex[4..6], 16);
                var a = Convert.ToInt32(hex[6..8], 16) / 255.0;
                return $"rgba({r}, {g}, {b}, {a:F2})";
            }
        }
        catch (FormatException)
        {
            return defaultRgb;
        }
        catch (OverflowException)
        {
            return defaultRgb;
        }
        catch (ArgumentOutOfRangeException)
        {
            return defaultRgb;
        }

        return defaultRgb;
    }

    /// <summary>
    /// Converts a hexadecimal color string to an HSL or HSLA string representation.
    /// </summary>
    /// <param name="hex">The hexadecimal color string (e.g., "#FF5733" or "#FF5733AA").</param>
    /// <param name="includeAlpha">Whether to include alpha channel in the output if present in the hex string.</param>
    /// <returns>An HSL or HSLA string representation (e.g., "hsl(9, 100%, 65%)" or "hsla(9, 100%, 65%, 0.67)").</returns>
    public static string HexToHsl(string hex, bool includeAlpha = false)
    {
        if (string.IsNullOrEmpty(hex) || !hex.StartsWith('#')) return defaultHsl;

        try
        {
            hex = hex.TrimStart('#');

            if (hex.Length == 8 && !includeAlpha)
            {
                hex = hex[..6];
            }

            var r = Convert.ToInt32(hex[..2], 16);
            var g = Convert.ToInt32(hex[2..4], 16);
            var b = Convert.ToInt32(hex[4..6], 16);

            (var h, var s, var l) = RgbToHsl(r, g, b);

            var hDeg = (int)Math.Round(h);
            var sPercent = (int)Math.Round(s * 100);
            var lPercent = (int)Math.Round(l * 100);

            if (hex.Length == 8 && includeAlpha)
            {
                var a = Convert.ToInt32(hex[6..8], 16) / 255.0;
                return $"hsla({hDeg}, {sPercent}%, {lPercent}%, {a:F2})";
            }

            return $"hsl({hDeg}, {sPercent}%, {lPercent}%)";
        }
        catch (FormatException)
        {
            return defaultHsl;
        }
        catch (OverflowException)
        {
            return defaultHsl;
        }
        catch (ArgumentOutOfRangeException)
        {
            return defaultHsl;
        }
    }

    /// <summary>
    /// Converts an RGB or RGBA string representation to a hexadecimal color string.
    /// </summary>
    /// <param name="rgb">The RGB or RGBA string (e.g., "rgb(255, 87, 51)" or "rgba(255, 87, 51, 0.67)").</param>
    /// <param name="includeAlpha">Whether to include alpha channel in the output if present in the RGB string.</param>
    /// <param name="fallbackValue">The fallback value to return if parsing fails. Defaults to "#000000".</param>
    /// <returns>A hexadecimal color string (e.g., "#FF5733" or "#FF5733AA").</returns>
    public static string RgbToHex(string rgb, bool includeAlpha = false, string fallbackValue = "#000000")
    {
        if (string.IsNullOrEmpty(rgb)) return fallbackValue;

        try
        {
            var cleaned = rgb.Trim().ToLower();

            if (cleaned.StartsWith("rgb(") || cleaned.StartsWith("rgba("))
            {
                var isRgba = cleaned.StartsWith("rgba(");
                var values = cleaned.Replace("rgba(", "").Replace("rgb(", "").Replace(")", "").Split(',');

                var r = int.Parse(values[0].Trim());
                var g = int.Parse(values[1].Trim());
                var b = int.Parse(values[2].Trim());

                if (isRgba && values.Length > 3 && includeAlpha)
                {
                    var a = (int)(double.Parse(values[3].Trim()) * 255);
                    return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
                }

                return $"#{r:X2}{g:X2}{b:X2}";
            }
        }
        catch (FormatException)
        {
            return fallbackValue;
        }
        catch (OverflowException)
        {
            return fallbackValue;
        }
        catch (ArgumentException)
        {
            return fallbackValue;
        }
        catch (IndexOutOfRangeException)
        {
            return fallbackValue;
        }

        return fallbackValue;
    }

    /// <summary>
    /// Converts an HSL or HSLA string representation to a hexadecimal color string.
    /// </summary>
    /// <param name="hsl">The HSL or HSLA string (e.g., "hsl(9, 100%, 65%)" or "hsla(9, 100%, 65%, 0.67)").</param>
    /// <param name="includeAlpha">Whether to include alpha channel in the output if present in the HSL string.</param>
    /// <param name="fallbackValue">The fallback value to return if parsing fails. Defaults to "#000000".</param>
    /// <returns>A hexadecimal color string (e.g., "#FF5733" or "#FF5733AA").</returns>
    public static string HslToHex(string hsl, bool includeAlpha = false, string fallbackValue = "#000000")
    {
        if (string.IsNullOrEmpty(hsl)) return fallbackValue;

        try
        {
            var cleaned = hsl.Trim().ToLower();

            if (cleaned.StartsWith("hsl(") || cleaned.StartsWith("hsla("))
            {
                var isHsla = cleaned.StartsWith("hsla(");
                var values = cleaned.Replace("hsla(", "").Replace("hsl(", "").Replace(")", "").Replace("%", "").Split(',');

                var h = double.Parse(values[0].Trim());
                var s = double.Parse(values[1].Trim()) / 100.0;
                var l = double.Parse(values[2].Trim()) / 100.0;

                (var r, var g, var b) = HslToRgb(h, s, l);

                if (isHsla && values.Length > 3 && includeAlpha)
                {
                    var a = (int)(double.Parse(values[3].Trim()) * 255);
                    return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
                }

                return $"#{r:X2}{g:X2}{b:X2}";
            }
        }
        catch (FormatException)
        {
            return fallbackValue;
        }
        catch (OverflowException)
        {
            return fallbackValue;
        }
        catch (ArgumentException)
        {
            return fallbackValue;
        }
        catch (IndexOutOfRangeException)
        {
            return fallbackValue;
        }

        return fallbackValue;
    }

    /// <summary>
    /// Converts RGB color values to HSL color values.
    /// </summary>
    /// <param name="r">The red value (0-255).</param>
    /// <param name="g">The green value (0-255).</param>
    /// <param name="b">The blue value (0-255).</param>
    /// <returns>A tuple containing HSL values where h is in degrees (0-360), s and l are in range (0.0-1.0).</returns>
    public static (double h, double s, double l) RgbToHsl(int r, int g, int b)
    {
        var rd = r / 255.0;
        var gd = g / 255.0;
        var bd = b / 255.0;

        var max = Math.Max(rd, Math.Max(gd, bd));
        var min = Math.Min(rd, Math.Min(gd, bd));
        var delta = max - min;

        double h = 0;
        double s = 0;
        var l = (max + min) / 2.0;

        if (Math.Abs(delta) > 1e-10)
        {
            s = l < 0.5 ? delta / (max + min) : delta / (2.0 - max - min);

            // Determine which RGB component is the maximum using integer comparisons
            int maxComponent;
            if (r >= g && r >= b)
                maxComponent = 0; // red
            else if (g >= r && g >= b)
                maxComponent = 1; // green
            else
                maxComponent = 2; // blue

            switch (maxComponent)
            {
                case 0: // red is max
                    h = ((gd - bd) / delta + (gd < bd ? 6 : 0)) * 60;
                    break;
                case 1: // green is max
                    h = ((bd - rd) / delta + 2) * 60;
                    break;
                default: // blue is max
                    h = ((rd - gd) / delta + 4) * 60;
                    break;
            }
        }

        return (h, s, l);
    }

    /// <summary>
    /// Converts HSL color values to RGB color values.
    /// </summary>
    /// <param name="h">The hue value in degrees (0-360).</param>
    /// <param name="s">The saturation value (0.0 to 1.0).</param>
    /// <param name="l">The lightness value (0.0 to 1.0).</param>
    /// <returns>A tuple containing the RGB values (0-255 for each component).</returns>
    public static (int r, int g, int b) HslToRgb(double h, double s, double l)
    {
        double rd, gd, bd;

        if (Math.Abs(s) < 1e-9)
        {
            rd = gd = bd = l;
        }
        else
        {
            var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            var p = 2 * l - q;
            rd = HueToRgb(p, q, h / 360.0 + 1.0 / 3.0);
            gd = HueToRgb(p, q, h / 360.0);
            bd = HueToRgb(p, q, h / 360.0 - 1.0 / 3.0);
        }

        return ((int)Math.Round(rd * 255), (int)Math.Round(gd * 255), (int)Math.Round(bd * 255));
    }

    /// <summary>
    /// Helper method to convert a hue value to an RGB component.
    /// </summary>
    /// <param name="p">The first calculated value.</param>
    /// <param name="q">The second calculated value.</param>
    /// <param name="t">The hue offset value.</param>
    /// <returns>The RGB component value (0.0 to 1.0).</returns>
    private static double HueToRgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1.0 / 6.0) return p + (q - p) * 6 * t;
        if (t < 1.0 / 2.0) return q;
        if (t < 2.0 / 3.0) return p + (q - p) * (2.0 / 3.0 - t) * 6;
        return p;
    }
}
