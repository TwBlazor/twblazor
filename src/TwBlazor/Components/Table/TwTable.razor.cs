// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a customizable table component for Blazor applications that enables rendering of table headers, body, and
/// footers with configurable styling and behaviour.
/// </summary>
/// <remarks>The TwTable component provides flexibility for displaying tabular data by allowing developers to
/// supply custom content for the header, body, and footer sections using RenderFragment parameters. It supports
/// additional styling options such as striped rows, hover effects, and borders, which can be controlled through
/// component parameters. Additional CSS classes can be applied to each section for further customization. This
/// component is intended to be used as a building block for creating accessible and visually consistent tables in
/// Blazor projects.</remarks>
public partial class TwTable : TwBlazorComponentBase
{
    private TwTableTheme theme => options.Theme.Components.Require<TwTableTheme>();

    /// <summary>
    /// Optional caption content for the table, rendered as a <c>&lt;caption&gt;</c> element. Provides an accessible
    /// name/description for the table that is programmatically associated with it. Visually hidden by default
    /// (via the <c>sr-only</c> class) unless <see cref="CaptionVisible"/> is set to <c>true</c>.
    /// </summary>
    [Parameter]
    public RenderFragment? Caption { get; set; }

    /// <summary>
    /// Whether the <see cref="Caption"/> should be visible. Defaults to <c>false</c>, rendering the caption
    /// visually hidden (but still available to assistive technology).
    /// </summary>
    [Parameter]
    public bool CaptionVisible { get; set; } = false;

    /// <summary>
    /// Content to render in the table header (thead).
    /// </summary>
    [Parameter]
    public RenderFragment? TableHeader { get; set; }

    /// <summary>
    /// Content to render in the table body (tbody).
    /// </summary>
    [Parameter]
    public RenderFragment? TableBody { get; set; }

    /// <summary>
    /// Content to render in the table footer (tfoot).
    /// </summary>
    [Parameter]
    public RenderFragment? TableFooter { get; set; }

    /// <summary>
    /// Additional CSS classes for the header section.
    /// </summary>
    [Parameter]
    public string? HeaderClass { get; set; }

    /// <summary>
    /// Additional CSS classes for the body section.
    /// </summary>
    [Parameter]
    public string? BodyClass { get; set; }

    /// <summary>
    /// Additional CSS classes for the footer section.
    /// </summary>
    [Parameter]
    public string? FooterClass { get; set; }

    /// <summary>
    /// Whether to show striped rows.
    /// </summary>
    [Parameter]
    public bool Striped { get; set; }

    /// <summary>
    /// Whether to show hover effect on rows.
    /// </summary>
    [Parameter]
    public bool Hoverable { get; set; }

    /// <summary>
    /// Whether to show borders on all cells.
    /// </summary>
    [Parameter]
    public bool Bordered { get; set; } = false;

    /// <summary>
    /// Whether to hide the outer table border.
    /// </summary>
    [Parameter]
    public bool NoBorder { get; set; } = false;

    private string tableClasses => new ClassBuilder()
        .AddClass(theme.Base)
        .AddClass(theme.Bordered, !NoBorder)
        .AddClass(roundedBuilder.GetRounded(effectiveRounded))
        .AddClass(Class)
        .Build();

    private string headerClasses => new ClassBuilder()
        .AddClass(theme.Header)
        .AddClass(theme.HeaderBorderedCells, Bordered)
        .AddClass(HeaderClass ?? string.Empty)
        .Build();

    private string bodyClasses => new ClassBuilder()
        .AddClass(theme.Body)
        .AddClass(theme.BodyStriped, Striped)
        .AddClass(theme.BodyHoverable, Hoverable)
        .AddClass(theme.BorderedCells, Bordered)
        .AddClass(theme.BorderedHeaderCells, Bordered)
        .AddClass(BodyClass ?? string.Empty)
        .Build();

    private string captionClasses => new ClassBuilder()
        .AddClass("sr-only", !CaptionVisible)
        .Build();

    private string footerClasses => new ClassBuilder()
        .AddClass(theme.BorderedCells, Bordered)
        .AddClass(theme.BorderedHeaderCells, Bordered)
        .AddClass(FooterClass ?? string.Empty)
        .Build();
}
