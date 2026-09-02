// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Configuration;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using TwBlazor.Configuration.Color;

/// <summary>
/// The root theme configuration, aggregating the rounded, shadow, and color scales along with per-component
/// theme overrides.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwBlazorTheme
{
    /// <summary>
    /// Gets or sets the shadow (elevation) scale used across all components.
    /// </summary>
    public TwBlazorShadow Shadows { get; set; } = new();

    /// <summary>
    /// Gets or sets the border-radius (rounded corner) scale used across all components.
    /// </summary>
    public TwBlazorRounded Rounded { get; set; } = new();

    /// <summary>
    /// Gets or sets the shared color configuration used across all components.
    /// </summary>
    public TwBlazorColor Colors { get; set; } = new();

    /// <summary>
    /// Gets or sets the classes for each screen-anchored position (e.g. toasts, dialogs can be positioned using this).
    /// </summary>
    public required TwPosition Position { get; set; }

    /// <summary>
    /// Gets or sets the per-component theme overrides. See <see cref="TwBlazorComponents"/>.
    /// </summary>
    public TwBlazorComponents Components { get; set; } = [];
}

/// <summary>
/// Shared color configuration used across all components: text, hover, focus, border, background, and surface colors.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwBlazorColor
{
    /// <summary>
    /// Gets or sets the color palette for light-mode text colors, by weight (light/medium/dark).
    /// </summary>
    public TwTextColor TextColors { get; set; } = new();

    /// <summary>
    /// Gets or sets the color palette for dark-mode text colors, by weight (light/medium/dark).
    /// </summary>
    public TwTextColor DarkTextColors { get; set; } = new();

    /// <summary>
    /// Gets or sets the color palette applied on hover for both light and dark modes.
    /// </summary>
    public TwBlazorPalette HoverColors { get; set; } = new();

    /// <summary>
    /// Gets or sets the base classes for the focus ring applied to focusable elements.
    /// </summary>
    public string FocusRingBase { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color palette applied to the focus ring for both light and dark modes.
    /// </summary>
    public TwBlazorPalette FocusColors { get; set; } = new();

    /// <summary>
    /// Gets or sets the color palette applied to borders for both light and dark modes.
    /// </summary>
    public TwBlazorPalette BorderColors { get; set; } = new();

    /// <summary>
    /// Gets or sets the background color palette, by weight (light/medium/dark), for dark mode.
    /// </summary>
    public TwBackgroundColor DarkBackground { get; set; } = new();

    /// <summary>
    /// Gets or sets the background color palette, by weight (light/medium/dark), for light mode.
    /// </summary>
    public TwBackgroundColor LightBackground { get; set; } = new();

    /// <summary>
    /// Gets or sets the color palette for button surface variants (filled, text, outlined).
    /// </summary>
    public TwSurfaceColor SurfaceColors { get; set; } = new();

    /// <summary>
    /// Gets or sets the shared neutral (gray-scale) surface tokens - card/dialog backgrounds,
    /// dividing borders, and hover tints - reused across components that aren't tied to a
    /// semantic color. See <see cref="TwSurfacePalette"/>.
    /// </summary>
    public TwSurfacePalette NeutralSurface { get; set; } = new();

    /// <summary>
    /// Gets or sets the shared neutral (gray-scale) text tokens - default body/heading text and its
    /// quieter variants - reused across components that aren't tied to a semantic color.
    /// See <see cref="TwNeutralTextPalette"/>.
    /// </summary>
    public TwNeutralTextPalette NeutralText { get; set; } = new();
}

/// <summary>
/// A color palette split by weight, used for text colors in a single mode (light or dark).
/// </summary>
[ExcludeFromCodeCoverage]
public class TwTextColor
{
    /// <summary>
    /// Gets or sets the color palette for light-weight text.
    /// </summary>
    public TwBlazorPalette Light { get; set; } = new();

    /// <summary>
    /// Gets or sets the color palette for medium-weight text.
    /// </summary>
    public TwBlazorPalette Medium { get; set; } = new();

    /// <summary>
    /// Gets or sets the color palette for dark-weight text.
    /// </summary>
    public TwBlazorPalette Dark { get; set; } = new();
}

/// <summary>
/// A color palette split by weight, used for background colors in a single mode (light or dark).
/// </summary>
[ExcludeFromCodeCoverage]
public class TwBackgroundColor
{
    /// <summary>
    /// Gets or sets the color palette for light-weight backgrounds.
    /// </summary>
    public TwBlazorPalette Light { get; set; } = new();

    /// <summary>
    /// Gets or sets the color palette for medium-weight backgrounds.
    /// </summary>
    public TwBlazorPalette Medium { get; set; } = new();

    /// <summary>
    /// Gets or sets the color palette for dark-weight backgrounds.
    /// </summary>
    public TwBlazorPalette Dark { get; set; } = new();
}

/// <summary>
/// Classes for each screen-anchored position used to place floating elements such as toasts and dialogs.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwPosition
{
    /// <summary>
    /// Gets or sets the classes for the center position.
    /// </summary>
    public required string Center { get; set; }

    /// <summary>
    /// Gets or sets the classes for the center-left position.
    /// </summary>
    public required string CenterLeft { get; set; }

    /// <summary>
    /// Gets or sets the classes for the center-right position.
    /// </summary>
    public required string CenterRight { get; set; }

    /// <summary>
    /// Gets or sets the classes for the top-center position.
    /// </summary>
    public required string TopCenter { get; set; }

    /// <summary>
    /// Gets or sets the classes for the top-left position.
    /// </summary>
    public required string TopLeft { get; set; }

    /// <summary>
    /// Gets or sets the classes for the top-right position.
    /// </summary>
    public required string TopRight { get; set; }

    /// <summary>
    /// Gets or sets the classes for the bottom-center position.
    /// </summary>
    public required string BottomCenter { get; set; }

    /// <summary>
    /// Gets or sets the classes for the bottom-left position.
    /// </summary>
    public required string BottomLeft { get; set; }

    /// <summary>
    /// Gets or sets the classes for the bottom-right position.
    /// </summary>
    public required string BottomRight { get; set; }
}

/// <summary>
/// A type-keyed bag of component themes. Only add the component themes you want to customize -
/// e.g. <c>new() { new TwBlazorSliderTheme { ... }, new TwBlazorSidebarTheme { ... } }</c> - and
/// look them up by type with <see cref="Get{TTheme}"/>. Consumers that don't find an entry for a
/// given theme type fall back to that component's built-in default.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class TwBlazorComponents : IEnumerable<object>
{
    private readonly Dictionary<Type, object> _themes = [];

    public TwBlazorComponents Add<TTheme>(TTheme theme) where TTheme : class
    {
        _themes[typeof(TTheme)] = theme;
        return this;
    }

    public TTheme? Get<TTheme>() where TTheme : class
        => _themes.TryGetValue(typeof(TTheme), out var theme) ? (TTheme)theme : null;

    /// <summary>
    /// Gets the registered theme, or throws if it was never added via <see cref="Add{TTheme}"/>.
    /// Use this from a component's theme lookup so a missing config fails loudly instead of
    /// silently rendering with no classes.
    /// </summary>
    public TTheme Require<TTheme>() where TTheme : class
        => Get<TTheme>() ?? throw new InvalidOperationException(
            $"No {typeof(TTheme).Name} has been configured on the theme. " +
            $"Add one via `Components.Add(new {typeof(TTheme).Name} {{ ... }})` in your theme definition.");

    public bool Contains<TTheme>() where TTheme : class => _themes.ContainsKey(typeof(TTheme));

    public IEnumerator<object> GetEnumerator() => _themes.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}