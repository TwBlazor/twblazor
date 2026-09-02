// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a container component for grouping multiple <see cref="TwButton"/> components together.
/// </summary>
/// <remarks>
/// TwButtonGroup provides a way to visually group related buttons with consistent spacing and layout.
/// Buttons within the group can be oriented horizontally or vertically, and support customizable spacing.
/// </remarks>
public partial class TwButtonGroup : TwBlazorComponentBase
{
    private TwGroupsTheme groupsTheme => options.Theme.Components.Require<TwGroupsTheme>();
    private TwInputTheme inputTheme => options.Theme.Components.Require<TwInputTheme>();

    /// <summary>
    /// Gets or sets the content to display within the button group.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the legend text for the button group.
    /// </summary>
    [Parameter] public string? Legend { get; set; }

    /// <summary>
    /// Gets or sets the CSS class for the legend element.
    /// </summary>
    [Parameter] public string? LegendClass { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the button group.
    /// </summary>
    /// <remarks>
    /// Default is horizontal. Set to false for vertical orientation.
    /// </remarks>
    [Parameter] public bool Vertical { get; set; } = false;

    /// <summary>
    /// Gets or sets whether buttons should take full width of the container.
    /// </summary>
    [Parameter] public bool FullWidth { get; set; } = false;

    private string classes =>
        new ClassBuilder(groupsTheme.ButtonGroupBase)
        .AddClass(Vertical ? groupsTheme.ButtonGroupVertical : groupsTheme.ButtonGroupHorizontal)
        .AddClass(FullWidth ? groupsTheme.ButtonGroupFullWidth : string.Empty)
        .AddClass(FullWidth && !Vertical ? groupsTheme.ButtonGroupFullWidthRow : string.Empty)
        .AddClass(groupsTheme.Gap)
        .AddClass(Class)
        .Build();

    private string legendClasses =>
        new ClassBuilder(inputTheme.InputLegendBase)
        .AddClass(LegendClass ?? string.Empty)
        .Build();

    /// <summary>
    /// The id given to the rendered legend element so the group container can reference it via
    /// aria-labelledby when no explicit <see cref="TwBlazorComponentBase.AriaLabel"/> was supplied,
    /// giving an icon-only toolbar labelled only by <see cref="Legend"/> an accessible name.
    /// </summary>
    private string legendId => $"{Id}-legend";
}
