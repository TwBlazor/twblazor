// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Builders;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a generic select dropdown component that supports two-way data binding.
/// </summary>
/// <typeparam name="T">The type of values in the dropdown options.</typeparam>
/// <remarks>
/// The TwSelect component provides a flexible dropdown control that can bind to collections of any type.
/// It supports customizable styling, placeholder text, required validation, and property-based display text.
/// The component implements two-way binding through the <see cref="SelectedValue"/> and <see cref="SelectedValueChanged"/> parameters.
/// </remarks>
public partial class TwSelect<T> : TwBlazorTextInputComponentBase
{
    [Inject] private InputVariantBuilder inputVariantBuilder { get; set; } = null!;

    private TwInputTheme theme => options.Theme.Components.Require<TwInputTheme>();

    /// <summary>
    /// Gets or sets the collection of values to display in the dropdown.
    /// </summary>
    [Parameter] public IEnumerable<T> Values { get; set; } = [];

    /// <summary>
    /// Gets or sets the currently selected value.
    /// </summary>
    [Parameter] public T SelectedValue { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback that is invoked when the selected value changes.
    /// </summary>
    [Parameter] public EventCallback<T> SelectedValueChanged { get; set; } = default!;

    /// <summary>
    /// Gets or sets the placeholder text displayed when no value is selected.
    /// </summary>
    [Parameter] public string Placeholder { get; set; } = "Select an option...";

    /// <summary>
    /// Gets or sets whether the select is required (no empty option will be shown).
    /// </summary>
    [Parameter] public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the name of the property to display for complex objects.
    /// </summary>
    /// <remarks>
    /// When Values contains complex objects, specify the property name to display as the option text.
    /// If null or empty, the object's ToString() method will be used.
    /// </remarks>
    [Parameter] public string PropertyName { get; set; } = string.Empty;

    private Dictionary<int, T> parsedValues { get; set; } = [];

    private int selectedValueId;

    /// <summary>
    /// Gets the CSS classes applied to the select element.
    /// </summary>
    private string classes => new ClassBuilder(theme.SelectBase)
        // A native <select> focuses on mouse click same as keyboard, so theme.FocusBorder's plain
        // "focus:" variant (shared with text inputs, where showing the border on click is fine)
        // is rewritten to "focus-visible:" here so the border only appears on keyboard focus.
        .AddClass(inputVariantBuilder.GetClasses(effectiveVariant, theme).Replace("focus:", "focus-visible:", StringComparison.Ordinal))
        .AddClass("px-3", effectiveVariant == InputVariant.Default)
        // Default/Outlined variants make the field's own background bg-transparent so it blends
        // with the surrounding page - fine for <input>, but a native <select> popup renders using
        // the element's own background/text colors, so a transparent one falls back to the OS's
        // native (often light) popup surface and can pair unreadable white dark-mode text onto it.
        // The Filled variant already sets a real background, so it's left alone here.
        .AddClass("!bg-white dark:!bg-gray-800", effectiveVariant != InputVariant.Filled)
        .AddClass(Disabled ? "opacity-40 cursor-not-allowed" : string.Empty)
        .AddClass(ReadOnly ? "!bg-none" : string.Empty)
        .AddClass(ReadOnly && !Disabled ? "pointer-events-none" : string.Empty)
        .AddClass(Class)
        .Build();

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (string.IsNullOrEmpty(RootId))
        {
            RootId = Guid.NewGuid().ToString("N");
        }

        PopulateValues();

        // Only Disabled maps to the native disabled attribute. ReadOnly is not a valid attribute
        // for select elements in HTML, so it must stay focusable/announced via aria-readonly - see
        // the aria-readonly attribute on the <select> markup and the ReadOnly guard in HandleChange,
        // which together keep the control in the tab order while blocking value changes.
        if (Disabled && !Attributes.ContainsKey("disabled"))
        {
            Attributes["disabled"] = true;
        }
        else if (!Disabled)
        {
            Attributes.Remove("disabled");
        }

        // A <select> with no "name" is omitted from form submissions; default to Id (already unique
        // per instance) unless the consumer supplied their own, same as TwTextfield.
        if (!Attributes.ContainsKey("name") && !string.IsNullOrEmpty(Id))
        {
            Attributes["name"] = Id;
        }
    }

    private void PopulateValues()
    {
        parsedValues = [];
        var valueId = 1;

        foreach (var value in Values)
        {
            parsedValues.Add(valueId, value);

            if (EqualityComparer<T>.Default.Equals(SelectedValue, value))
            {
                selectedValueId = valueId;
            }

            valueId++;
        }
    }

    private string GetDisplayText(T value)
    {
        // Only a genuinely absent (null) value should render as empty - default(T) is a
        // legitimate, selectable value for non-nullable value types (e.g. 0 for int).
        if (value is null)
            return string.Empty;

        if (!string.IsNullOrWhiteSpace(PropertyName))
        {
            var property = value.GetType().GetProperty(PropertyName);
            if (property != null)
            {
                var propertyValue = property.GetValue(value);
                return propertyValue?.ToString() ?? string.Empty;
            }
        }

        return value.ToString() ?? string.Empty;
    }

    private async Task HandleChange(ChangeEventArgs e)
    {
        if (ReadOnly || Disabled)
            return;

        if (int.TryParse(e.Value?.ToString(), out var newValueId))
        {
            selectedValueId = newValueId;
            if (parsedValues.TryGetValue(selectedValueId, out var selectedItem))
            {
                SelectedValue = selectedItem;
                if (SelectedValueChanged.HasDelegate)
                {
                    await SelectedValueChanged.InvokeAsync(SelectedValue);
                }
            }
        }
    }
}
