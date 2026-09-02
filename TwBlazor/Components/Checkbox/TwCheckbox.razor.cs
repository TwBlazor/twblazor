// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Builders;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a generic checkbox input component for Blazor that supports two-way data binding and customizable value
/// types.
/// </summary>
/// <remarks>Use <see cref="TwCheckbox{T}"/> to create checkbox inputs that bind to values of type <typeparamref
/// name="T"/>. The component supports two-way binding via the <see cref="Value"/> and <see cref="ValueChanged"/>
/// parameters. The <see cref="Name"/> property can be used to associate the checkbox with a form field or for
/// accessibility purposes.</remarks>
/// <typeparam name="T">The type of value associated with the checkbox. Determines the data type used for binding and value changes.</typeparam>
public partial class TwCheckbox<T> : TwBlazorInputComponentBase
{
    private TwCheckboxTheme theme => options.Theme.Components.Require<TwCheckboxTheme>();

    /// <summary>
    /// Gets or sets the name associated with the component.
    /// </summary>
    [Parameter] public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name from a parent CheckboxGroup (cascaded value).
    /// </summary>
    [CascadingParameter(Name = "GroupName")] public string? GroupName { get; set; }

    /// <summary>
    /// Gets the effective name to use, preferring the explicit Name parameter over the cascaded GroupName.
    /// </summary>
    private string effectiveName => !string.IsNullOrWhiteSpace(Name) ? Name : GroupName ?? string.Empty;

    /// <summary>
    /// Gets or sets the color of the checkbox.
    /// </summary>
    [Parameter] public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the current value of the parameter.
    /// </summary>
    [Parameter] public T Value { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback that is invoked when the value changes.
    /// </summary>
    /// <remarks>Use this property to handle value change events in two-way data binding scenarios. The
    /// callback is triggered when the component's value is updated, allowing parent components to respond to the
    /// change.</remarks>
    [Parameter] public EventCallback<T> ValueChanged { get; set; } = default!;

    private string classes =>
        new ClassBuilder(theme.Base)
        .AddClass(roundedBuilder.GetRounded())
        .AddClass(GetCheckboxColor(Color))
        .AddClass(Disabled ? theme.Disabled : theme.Hover)
        .AddClass(Class).Build();

    private string labelClasses =>
        new ClassBuilder(theme.LabelBase)
        .AddClass(ReadOnly || Disabled ? theme.LabelNonInteractiveCursor : theme.LabelInteractiveCursor)
        .AddClass(Disabled ? theme.LabelDisabled : string.Empty)
        .AddClass(LabelClass).Build();

    private string checkClasses =>
        new ClassBuilder(theme.IconWrapper)
        .AddClass(options.Theme.Colors.TextColors.Dark.Dark, Color == Enums.Color.Light)
        .AddClass(options.Theme.Colors.TextColors.Medium.Light, Color != Enums.Color.Light)
        .Build();

    private bool isChecked => Value is bool boolValue && boolValue;

    private string GetCheckboxColor(Color? color) => ColorBuilder.GetPaletteColor(color, theme.Colors, theme.Colors.Primary);

    private async Task HandleChange(ChangeEventArgs e)
    {
        if (ReadOnly || Disabled)
            return;

        if (e.Value is bool newValue)
        {
            Value = (T)(object)newValue;
            await ValueChanged.InvokeAsync(Value);
        }
    }
}
