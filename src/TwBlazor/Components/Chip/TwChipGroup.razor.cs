// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a container component for grouping multiple <see cref="TwChip"/> components together.
/// </summary>
/// <remarks>
/// TwChipGroup provides a way to visually group related chips with consistent spacing and wrapping behavior.
/// Chips within the group automatically wrap to multiple lines when space is limited.
/// </remarks>
public partial class TwChipGroup : TwBlazorComponentBase
{
    private TwGroupsTheme groupsTheme => options.Theme.Components.Require<TwGroupsTheme>();
    private TwInputTheme inputTheme => options.Theme.Components.Require<TwInputTheme>();

    /// <summary>
    /// Gets or sets the content to display within the chip group.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the legend text for the chip group.
    /// </summary>
    [Parameter] public string? Legend { get; set; }

    /// <summary>
    /// Gets or sets the CSS class for the legend element.
    /// </summary>
    [Parameter] public string? LegendClass { get; set; }

    /// <summary>
    /// Gets or sets the alignment of chips within the chip group.
    /// </summary>
    /// <remarks>
    /// Default is "start". Possible values: "start", "center", "end".
    /// </remarks>
    [Parameter] public string Alignment { get; set; } = "start";

    /// <summary>
    /// Gets the id assigned to the legend element, used to wire it up as the group's accessible name.
    /// </summary>
    private string legendId => $"{Id}-legend";

    /// <summary>
    /// Gets the <c>aria-labelledby</c> value applied to the group container. Points at the legend element
    /// when <see cref="Legend"/> is set and the consumer hasn't explicitly supplied
    /// <see cref="TwBlazorComponentBase.AriaLabel"/> or their own <see cref="TwBlazorComponentBase.AriaLabelledBy"/>.
    /// An explicitly-supplied <see cref="TwBlazorComponentBase.AriaLabelledBy"/> always wins.
    /// </summary>
    private string? effectiveAriaLabelledBy
    {
        get
        {
            if (AriaLabelledBy is not null)
            {
                return AriaLabelledBy;
            }

            var legendProvidesLabel = !string.IsNullOrWhiteSpace(Legend) && string.IsNullOrWhiteSpace(AriaLabel);
            return legendProvidesLabel ? legendId : null;
        }
    }

    private string classes =>
        new ClassBuilder(groupsTheme.ChipGroupBase)
        .AddClass(Alignment switch
        {
            "center" => groupsTheme.ChipGroupAlignCenter,
            "end" => groupsTheme.ChipGroupAlignEnd,
            _ => groupsTheme.ChipGroupAlignStart
        })
        .AddClass(groupsTheme.Gap)
        .AddClass(Class)
        .Build();

    private string legendClasses =>
        new ClassBuilder(inputTheme.InputLegendBase)
        .AddClass(LegendClass ?? string.Empty)
        .Build();
}
