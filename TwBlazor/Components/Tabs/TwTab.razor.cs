// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a tab within a tab container, allowing users to navigate between different content sections.
/// </summary>
/// <remarks>A TwTab is typically used as a child of a TwTabContainer component. The tab can be enabled or
/// disabled using the Disabled property, and its appearance can be customized with the Color property. The tab's label
/// is displayed to the user, and it is activated by the parent tab container. Only one tab is active at a time within a
/// container.</remarks>
public partial class TwTab : TwBlazorComponentBase
{
    /// <summary>
    /// The parent tab container that this tab belongs to. This property is required and is set via cascading parameters.
    /// </summary>
    [CascadingParameter] public required TwTabContainer Parent { get; set; }

    /// <summary>
    /// The label to display for this tab. This is a required parameter and is used to identify the tab in the user interface.
    /// </summary>
    [Parameter] public required string Label { get; set; }

    /// <summary>
    /// The content to be displayed when this tab is active. This is a required parameter and should contain the markup or components that make up the tab's content.
    /// </summary>
    [Parameter] public required RenderFragment ChildContent { get; set; }

    /// <summary>
    /// The color of the tab, which can be set to customize its appearance. If not specified, it will inherit the color from the parent tab container.
    /// </summary>
    [Parameter] public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is disabled and cannot be interacted with by the user.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the component will not respond to user input. This is
    /// typically used to prevent interaction during loading states or when certain conditions are not met.</remarks>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// A reference to the rendered <see cref="TwButton"/> instance for this tab within the parent
    /// <see cref="TwTabContainer"/>'s tablist. Used to move keyboard focus to this tab (roving tabindex
    /// pattern) when it becomes active via arrow-key navigation.
    /// </summary>
    internal TwButton? buttonRef { get; set; }

    private TwTabTheme theme => options.Theme.Components.Require<TwTabTheme>();

    /// <summary>
    /// Gets the CSS classes that are applied to the tab element based on its current state and configuration.
    /// </summary>
    /// <remarks>The returned class string reflects the tab's active, disabled, and dense states, as well as
    /// color and text options provided by the parent component. These classes control the tab's appearance, including
    /// padding, text styling, and interactive effects.</remarks>
    public string TabClasses => new ClassBuilder(Parent.Dense ? theme.TabDensePadding : theme.TabPadding)
        .AddClass(theme.TabBase)
        .AddClass(colorBuilder.GetTextColor(Color))
        .AddClass(theme.ActiveIndicator, (Parent.ActiveTab == this && !Disabled))
        .AddClass(theme.InactiveIndicator, Parent.ActiveTab != this && !Disabled)
        .AddClass(theme.DisabledTab, Disabled)
        .AddClass(Class).Build();

    protected override void OnInitialized()
    {
        Color ??= Parent.TabColor;
        Parent.ActivateTab(this);
        base.OnInitialized();
    }
}