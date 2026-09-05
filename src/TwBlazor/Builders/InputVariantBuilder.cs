// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Builders;

/// <summary>
/// Provides utility methods for generating CSS classes for input component variants.
/// </summary>
public class InputVariantBuilder(RoundedBuilder roundedBuilder)
{
    /// <summary>
    /// Gets the Tailwind CSS classes for text input variants.
    /// </summary>
    /// <param name="variant">The input variant.</param>
    /// <param name="theme">Optional theme configuration.</param>
    /// <returns>The corresponding Tailwind CSS classes.</returns>
    public string GetClasses(InputVariant variant, TwInputTheme theme)
    {
        return variant switch
        {
            InputVariant.Default => new ClassBuilder()
                .AddClass(theme.FilledBorder)
                .AddClass(theme.FocusBorder)
                .AddClass("bg-transparent px-0")
                .Build(),

            InputVariant.Outlined => new ClassBuilder()
                .AddClass(theme.OutlinedBorder)
                .AddClass(theme.FocusBorder)
                .AddClass(roundedBuilder.GetRounded())
                .AddClass("bg-transparent px-3")
                .Build(),

            // Only the top corners are rounded here: FilledBorder draws a flat, full-width bottom
            // border (border-b), so rounding all four corners made that border cut across the
            // rounded bottom corners of the filled background instead of following them.
            InputVariant.Filled => new ClassBuilder()
                .AddClass(theme.FilledBorder)
                .AddClass(theme.FocusBorder)
                .AddClass(theme.FilledBackgroundColor)
                .AddClass(roundedBuilder.GetRoundedTop())
                .AddClass("px-3")
                .Build(),

            _ => new ClassBuilder()
                .AddClass(theme.FilledBorder)
                .AddClass(theme.FocusBorder)
                .AddClass("bg-transparent px-0")
                .Build()
        };
    }
}
