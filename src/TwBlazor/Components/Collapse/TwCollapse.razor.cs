// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a collapsible drawer component that shows or hides content when triggered.
/// </summary>
/// <remarks>
/// The TwCollapse component provides an accessible, animated panel that expands and collapses
/// when the trigger button is clicked. It supports two-way binding via <see cref="IsOpen"/> and
/// <see cref="IsOpenChanged"/>, and can notify consumers of each toggle via <see cref="OnToggle"/>.
/// </remarks>
public partial class TwCollapse : TwBlazorComponentBase
{
    /// <summary>
    /// Gets or sets the title text displayed in the collapse trigger button.
    /// </summary>
    /// <remarks>
    /// Ignored when <see cref="HeaderContent"/> is provided.
    /// </remarks>
    [Parameter] public string? Title { get; set; }

    /// <summary>
    /// Gets or sets custom content to display inside the collapse trigger button.
    /// </summary>
    /// <remarks>
    /// When provided, takes precedence over <see cref="Title"/>.
    /// </remarks>
    [Parameter] public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the content displayed inside the collapsible panel.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the collapsible panel is open.
    /// </summary>
    [Parameter] public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when <see cref="IsOpen"/> changes.
    /// </summary>
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the collapse is toggled, providing the new open state.
    /// </summary>
    [Parameter] public EventCallback<bool> OnToggle { get; set; }

    private TwCollapseTheme theme => options.Theme.Components.Require<TwCollapseTheme>();

    private string containerClasses =>
        new ClassBuilder(theme.Container)
        .AddClass(shadowBuilder.GetShadow(effectiveShadow))
        .AddClass(roundedBuilder.GetRounded(effectiveRounded))
        .AddClass(Class)
        .Build();

    private string triggerClasses =>
        new ClassBuilder(theme.Trigger)
        .AddClass(roundedBuilder.GetRounded(effectiveRounded), !IsOpen)
        .AddClass(roundedBuilder.GetRoundedTop(effectiveRounded), IsOpen)
        .Build();

    private string iconClasses =>
        new ClassBuilder(theme.Icon)
        .AddClass(theme.IconOpen, IsOpen)
        .Build();

    private string contentClasses =>
        new ClassBuilder(theme.Content)
        .AddClass("hidden", !IsOpen)
        .Build();

    private async Task ToggleAsync()
    {
        IsOpen = !IsOpen;

        if (IsOpenChanged.HasDelegate)
            await IsOpenChanged.InvokeAsync(IsOpen);

        if (OnToggle.HasDelegate)
            await OnToggle.InvokeAsync(IsOpen);

        StateHasChanged();
    }
}
