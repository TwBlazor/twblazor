// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Configuration;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Builders;

/// <summary>
/// Provides utility methods for building chip CSS classes.
/// </summary>
public class ChipBuilder(TwBlazorOptions options, ButtonBuilder buttonBuilder, ColorBuilder colorBuilder)
{
    private TwChipTheme theme => options.Theme.Components.Require<TwChipTheme>();

    /// <summary>
    /// Gets the base structure classes for a chip.
    /// </summary>
    /// <param name="size">The chip size.</param>
    /// <param name="clickable">Whether the chip is clickable.</param>
    /// <param name="disabled">Whether the chip is disabled.</param>
    public string GetBaseClasses(ChipSize size, bool clickable, bool disabled)
    {
        var classes = new ClassBuilder(theme.Base);

        // Size-specific classes
        classes.AddClass(size switch
        {
            ChipSize.Small => theme.Sm,
            ChipSize.Medium => theme.Md,
            ChipSize.Large => theme.Lg,
            _ => theme.Md
        });

        // Clickable/interactive states
        if (clickable && !disabled)
        {
            classes.AddClass(buttonBuilder.GetCursorClasses(false, false));
        }

        if (disabled)
        {
            classes.AddClass(buttonBuilder.GetCursorClasses(true, false));
        }

        return classes.Build();
    }

    /// <summary>
    /// Generates the CSS class string for a button based on its visual variant, color, disabled state, and theme.
    /// </summary>
    /// <remarks>If the button is disabled, the returned CSS classes will reflect the disabled state
    /// regardless of the specified color or variant.</remarks>
    /// <param name="variant">The visual style variant to apply to the button. Determines the overall appearance, such as filled, outlined,
    /// text, or tonal.</param>
    /// <param name="color">The color to use for the button, which influences the resulting CSS classes and visual styling.</param>
    /// <param name="disabled">A value indicating whether the button is disabled. If set to <see langword="true"/>, disabled styles are
    /// applied.</param>
    /// <param name="clickable">Whether the chip is interactive. When <see langword="false"/>, hover/active state
    /// classes are stripped so a non-interactive chip doesn't visually imply interactivity it doesn't have.</param>
    /// <returns>A string containing the CSS classes that correspond to the specified button variant, color, and state.</returns>
    public string GetVariantClasses(ButtonVariant variant, Color color, bool disabled, bool clickable = true)
    {
        if (disabled)
        {
            return buttonBuilder.GetDisabledClasses(variant);
        }

        var classes = variant switch
        {
            ButtonVariant.Outlined => colorBuilder.GetOutlinedVariantColor(color),
            ButtonVariant.Text => colorBuilder.GetTextVariantColor(color),
            _ => colorBuilder.GetFilledVariantColor(color)
        };

        // The theme's variant color strings bake in "hover:"/"active:" classes intended for
        // always-interactive buttons. A non-clickable chip is just a label, so those classes are
        // stripped here to avoid implying interactivity (a hover/press background change) it doesn't
        // actually have.
        return clickable ? classes : StripInteractionStateClasses(classes);
    }

    private static string StripInteractionStateClasses(string classes)
    {
        var kept = classes
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(c => !c.StartsWith("hover:", StringComparison.Ordinal) && !c.StartsWith("active:", StringComparison.Ordinal));

        return string.Join(' ', kept);
    }
}
