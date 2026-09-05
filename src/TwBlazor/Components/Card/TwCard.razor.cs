// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a card component that provides a contained surface for grouping related content.
/// </summary>
/// <remarks>
/// The TwCard component uses the Surface Container theme tokens by default
/// (<c>bg-gray-100 dark:bg-gray-800</c>) with On Surface text colours.
/// It supports an optional title, optional header/footer sections,
/// and respects the global shadow and rounded corner configuration.
/// </remarks>
public partial class TwCard : TwBlazorComponentBase
{
    private TwCardTheme theme => options.Theme.Components.Require<TwCardTheme>();

    /// <summary>
    /// Gets or sets the title text displayed at the top of the card.
    /// </summary>
    /// <remarks>
    /// When provided, renders as a heading inside the card header area.
    /// If <see cref="HeaderContent"/> is also provided, the title is rendered before it.
    /// </remarks>
    [Parameter] public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the heading level (1-6) rendered for <see cref="Title"/>, so the card's heading can
    /// fit correctly into the surrounding page's heading outline instead of always being an &lt;h3&gt;
    /// regardless of context. Defaults to 3.
    /// </summary>
    [Parameter] public int TitleLevel { get; set; } = 3;

    /// <summary>
    /// Gets or sets custom header content rendered below the title.
    /// </summary>
    [Parameter] public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the main content of the card.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether the card has a visible border.
    /// </summary>
    /// <remarks>
    /// When true, applies the Outline Variant token (<c>border-gray-200 dark:border-gray-700</c>).
    /// Default is true.
    /// </remarks>
    [Parameter] public bool Bordered { get; set; } = true;

    private string cardClasses => new ClassBuilder()
        .AddClass(roundedBuilder.GetRounded(effectiveRounded))
        .AddClass(shadowBuilder.GetShadow(effectiveShadow))
        .AddClass(theme.Container)
        .AddClass(theme.Bordered, Bordered)
        .AddClass(Class)
        .Build();

    private bool hasHeader => !string.IsNullOrEmpty(Title) || HeaderContent is not null;

    /// <summary>
    /// Renders <see cref="Title"/> as an &lt;h1&gt;-&lt;h6&gt; element per <see cref="TitleLevel"/>. A
    /// dynamic tag name isn't expressible directly in .razor markup, so the render tree is built by hand.
    /// </summary>
    private RenderFragment renderTitle => builder =>
    {
        var tag = $"h{Math.Clamp(TitleLevel, 1, 6)}";
        builder.OpenElement(0, tag);
        builder.AddAttribute(1, "class", theme.Title);
        builder.AddContent(2, Title);
        builder.CloseElement();
    };
}
