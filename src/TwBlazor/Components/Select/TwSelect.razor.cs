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
    /// <remarks>
    /// Not used when <see cref="Multiple"/> is <see langword="true"/> - bind <see cref="SelectedValues"/> instead.
    /// </remarks>
    [Parameter] public T SelectedValue { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback that is invoked when the selected value changes.
    /// </summary>
    [Parameter] public EventCallback<T> SelectedValueChanged { get; set; } = default!;

    /// <summary>
    /// Gets or sets whether more than one option can be selected at once.
    /// </summary>
    /// <remarks>
    /// Renders the underlying element as a native <c>&lt;select multiple&gt;</c> rather than a bespoke
    /// listbox, so mobile/touch browsers still get their own platform multi-select UI. Bind
    /// <see cref="SelectedValues"/>/<see cref="SelectedValuesChanged"/> rather than
    /// <see cref="SelectedValue"/>/<see cref="SelectedValueChanged"/> when this is <see langword="true"/>.
    /// </remarks>
    [Parameter] public bool Multiple { get; set; }

    /// <summary>
    /// Gets or sets the currently selected values when <see cref="Multiple"/> is <see langword="true"/>.
    /// </summary>
    [Parameter] public IEnumerable<T> SelectedValues { get; set; } = [];

    /// <summary>
    /// Gets or sets the callback that is invoked when the selection changes while <see cref="Multiple"/> is <see langword="true"/>.
    /// </summary>
    [Parameter] public EventCallback<IEnumerable<T>> SelectedValuesChanged { get; set; }

    /// <summary>
    /// Gets or sets the placeholder text displayed when no value is selected.
    /// </summary>
    /// <remarks>
    /// Not shown when <see cref="Multiple"/> is <see langword="true"/> - a multi-select doesn't need a
    /// dedicated "nothing selected" option, since simply selecting nothing already represents that.
    /// </remarks>
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

    private HashSet<int> selectedValueIds { get; set; } = [];

    /// <summary>
    /// Gets the CSS classes applied to the select element.
    /// </summary>
    private string classes => new ClassBuilder(theme.SelectBase)
        // A native <select> focuses on mouse click same as keyboard, so theme.FocusBorder's plain
        // "focus:" variant (shared with text inputs, where showing the border on click is fine)
        // is rewritten to "focus-visible:" here so the border only appears on keyboard focus.
        .AddClass(inputVariantBuilder.GetClasses(effectiveVariant, theme).Replace("focus:", "focus-visible:", StringComparison.Ordinal))
        .AddClass(theme.SelectDefaultPadding, effectiveVariant == InputVariant.Default)
        // Default/Outlined variants make the field's own background bg-transparent so it blends
        // with the surrounding page - fine for <input>, but a native <select> popup renders using
        // the element's own background/text colors, so a transparent one falls back to the OS's
        // native (often light) popup surface and can pair unreadable white dark-mode text onto it.
        // The Filled variant already sets a real background, so it's left alone here.
        .AddClass(theme.SelectNativeBackground, effectiveVariant != InputVariant.Filled)
        .AddClass(Disabled ? $"{options.Theme.Interaction.DisabledOpacity} {options.Theme.Interaction.DisabledCursor}" : string.Empty)
        // Multiple reuses the same "no dropdown arrow" override as ReadOnly - a multi-select renders
        // as an inline scrollable listbox rather than a closed popup, so the arrow (which implies a
        // collapsed dropdown you click open) doesn't apply to it either.
        .AddClass(ReadOnly || Multiple ? theme.SelectReadOnlyBackground : string.Empty)
        .AddClass(ReadOnly && !Disabled ? options.Theme.Interaction.PointerEventsNone : string.Empty)
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

        var selectedSet = Multiple ? new HashSet<T>(SelectedValues, EqualityComparer<T>.Default) : null;
        var newSelectedIds = Multiple ? new HashSet<int>() : null;

        foreach (var value in Values)
        {
            parsedValues.Add(valueId, value);

            if (Multiple)
            {
                if (selectedSet!.Contains(value))
                {
                    newSelectedIds!.Add(valueId);
                }
            }
            else if (EqualityComparer<T>.Default.Equals(SelectedValue, value))
            {
                selectedValueId = valueId;
            }

            valueId++;
        }

        if (Multiple)
        {
            selectedValueIds = newSelectedIds!;
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

        if (Multiple)
        {
            // ChangeEventArgs.Value for a native <select multiple> is the array of selected option
            // values (see the @onchange "Multiple option selection" binding support in Blazor docs),
            // not a single scalar - so it's read as a string[] here rather than parsed as one int.
            if (e.Value is not string[] selectedIdStrings)
                return;

            var newIds = new HashSet<int>();
            var newValues = new List<T>();

            foreach (var idString in selectedIdStrings)
            {
                if (int.TryParse(idString, out var id) && parsedValues.TryGetValue(id, out var value))
                {
                    newIds.Add(id);
                    newValues.Add(value);
                }
            }

            selectedValueIds = newIds;
            SelectedValues = newValues;

            if (SelectedValuesChanged.HasDelegate)
            {
                await SelectedValuesChanged.InvokeAsync(SelectedValues);
            }

            return;
        }

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
