// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Enums;
using TwBlazor.Extensions;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents an icon component that displays Bootstrap icons.
/// </summary>
/// <remarks>
/// The TwIcon component renders a Bootstrap icon with optional click and mouse over interactions.
/// When an <see cref="OnClick"/> callback is provided, the icon is wrapped in a button for accessibility.
/// </remarks>
public partial class TwIcon : TwBlazorComponentBase
{
    /// <summary>
    /// Gets or sets the callback that is invoked when the icon is clicked.
    /// </summary>
    /// <remarks>
    /// When this callback has a delegate assigned, the icon will be rendered as a clickable button
    /// with a pointer cursor and proper accessibility attributes.
    /// </remarks>
    [Parameter] public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets whether the button state for the icon button is disabled if an onclick parameter is passed.
    /// </summary>
    /// /// <remarks>
    /// This will only work if a callback has a delegate assigned, the icon will be rendered as a clickable button
    /// with a pointer cursor and proper accessibility attributes.
    /// </remarks>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the mouse hovers over the icon.
    /// </summary>
    [Parameter] public EventCallback OnMouseOver { get; set; }

    /// <summary>
    /// Gets or sets the icon to display.
    /// </summary>
    /// <remarks>
    /// The icon enum value is converted to its corresponding Bootstrap icon name.
    /// </remarks>
    [Parameter] public Icon Icon { get; set; }

    /// <summary>
    /// Gets or sets the button variant to display.
    /// </summary>
    /// <remarks>
    /// This is only applied if you are using an icon button by passing in an OnClick ref.
    /// </remarks>
    [Parameter] public ButtonVariant ButtonVariant { get; set; } = ButtonVariant.Text;

    /// <summary>
    /// Gets or sets whether the wrapped icon button skips its default button chrome (sizing, variant/color
    /// classes, focus ring, shadow), leaving only <see cref="TwBlazorComponentBase.Class"/> applied.
    /// </summary>
    /// <remarks>
    /// This is only applied if you are using an icon button by passing in an OnClick ref. Useful when embedding
    /// the icon inside an already-styled control, such as a chip's close button.
    /// </remarks>
    [Parameter] public bool Plain { get; set; }

    /// <summary>
    /// Gets or sets the color of the icon.
    /// </summary>
    /// <remarks>
    /// When specified, applies a text color class to the icon. If null, no color class is applied.
    /// </remarks>
    [Parameter] public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the CSS class name applied to the root element of the component.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of the component by specifying one or more CSS
    /// class names. Ensure that the provided class names are valid to achieve the desired styling.</remarks>
    [Parameter] public string RootClass { get; set; } = string.Empty;

    private string iconName => EnumExtensions.GetDescriptionFromName(Icon);
    private string color => colorBuilder.GetTextColor(Color);

    /// <summary>
    /// Gets whether the caller has supplied an accessible name for this icon. When false (the common,
    /// purely-decorative case) the icon is hidden from assistive tech via aria-hidden; when true, the
    /// caller intends the icon to convey meaning on its own, so aria-hidden is omitted.
    /// </summary>
    private bool hasAccessibleName => !string.IsNullOrEmpty(AriaLabel) || !string.IsNullOrEmpty(AriaLabelledBy);

    /// <summary>
    /// Gets the classes applied to the rendered <see cref="TwButton"/> when <see cref="OnClick"/> is set.
    /// </summary>
    /// <remarks>
    /// <see cref="TwBlazorComponentBase.Class"/> is treated as styling for the interactive element (padding, hover, focus, etc.),
    /// not the icon glyph itself, so it is combined here with <see cref="RootClass"/> rather than applied
    /// to the inner &lt;i&gt; - otherwise the button's clickable/focusable area wouldn't match its visual size.
    /// </remarks>
    private string buttonClasses => new ClassBuilder(RootClass)
        .AddClass(Class)
        .Build();

    /// <summary>
    /// Gets the classes for the icon glyph when it is rendered inside a <see cref="TwButton"/>, i.e. without
    /// <see cref="TwBlazorComponentBase.Class"/> - which is applied to the button instead. See <see cref="buttonClasses"/>.
    /// </summary>
    /// <remarks>
    /// Deliberately omits <see cref="color"/>: the wrapping <see cref="TwButton"/> already resolves the
    /// correct text color for the chosen variant/color pairing, and the glyph inherits it via <c>currentColor</c>.
    /// </remarks>
    private string bareIconClasses => new ClassBuilder($"bi bi-{iconName}")
        .Build();

    private string iconClasses => new ClassBuilder($"bi bi-{iconName}")
        .AddClass(Class)
        .AddClass(color)
        .Build();
}
