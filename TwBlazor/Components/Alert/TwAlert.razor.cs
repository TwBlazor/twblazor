// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Builders;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents an alert component that displays informational messages to users.
/// </summary>
/// <remarks>
/// The TwAlert component is a flexible notification element that can display text or custom content
/// with optional icons and a dismissible close button. It supports various color schemes through
/// the Color parameter and can be shown or hidden using the Dismissed parameter.
/// </remarks>
public partial class TwAlert : TwBlazorComponentBase
{
    private TwAlertTheme theme => options.Theme.Components.Require<TwAlertTheme>();

    /// <summary>
    /// Gets or sets the text content to display in the alert.
    /// </summary>
    /// <remarks>
    /// This text is displayed when no ChildContent is provided. If both Text and ChildContent are set,
    /// ChildContent takes precedence.
    /// </remarks>
    [Parameter] public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the icon to display at the start of the alert.
    /// </summary>
    [Parameter] public Icon? StartIcon { get; set; }

    /// <summary>
    /// Gets or sets the icon to display at the end of the alert.
    /// </summary>
    [Parameter] public Icon? EndIcon { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the alert is dismissed.
    /// </summary>
    [Parameter] public EventCallback OnDismiss { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the alert can be dismissed by the user.
    /// </summary>
    /// <remarks>
    /// When set to true, a close button will be displayed that allows the user to dismiss the alert.
    /// </remarks>
    [Parameter] public bool Dismissible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the alert is currently dismissed (hidden).
    /// </summary>
    [Parameter] public bool Dismissed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the alert should use compact padding.
    /// </summary>
    [Parameter] public bool Dense { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the Dismissed state changes.
    /// </summary>
    [Parameter] public EventCallback<bool> DismissedChanged { get; set; }

    /// <summary>
    /// Gets or sets the custom content to display in the alert.
    /// </summary>
    /// <remarks>
    /// When provided, this content replaces the Text parameter.
    /// </remarks>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the color scheme for the alert.
    /// </summary>
    [Parameter] public Color? Color { get; set; }

    private string classes =>
        new ClassBuilder("tw-alert")
        .AddClass(shadowBuilder.GetShadow(effectiveShadow))
        .AddClass(roundedBuilder.GetRounded(effectiveRounded))
        .AddClass(GetAlertColor(Color))
        .AddClass("hidden", Dismissed)
        .AddClass("py-2 px-4", Dense)
        .AddClass("py-4 px-6", !Dense)
        .AddClass("flex items-center gap-2 transition-colors duration-300", !Dismissed)
        .AddClass(Class)
        .Build();

    private string dismissButtonClasses =>
        new ClassBuilder()
        .AddClass("inline-flex items-center justify-center w-8 h-8 rounded-full")
        .AddClass(EndIcon is null ? "ml-auto" : "ml-2")
        .AddClass("text-current opacity-60 hover:opacity-100 hover:bg-white hover:bg-opacity-20 dark:hover:bg-gray-800 dark:hover:bg-opacity-20 transition-[opacity,background-color] duration-200 focus:outline-none focus:ring-2 focus:ring-current focus:ring-opacity-50")
        .Build();

    private string GetAlertColor(Color? color) => ColorBuilder.GetPaletteColor(color, theme.Colors, string.Empty);

    private async Task HandleDismiss()
    {
        Dismissed = true;

        if (DismissedChanged.HasDelegate)
        {
            await DismissedChanged.InvokeAsync(Dismissed);
        }

        if (OnDismiss.HasDelegate)
        {
            await OnDismiss.InvokeAsync();
        }

        StateHasChanged();
    }
}
