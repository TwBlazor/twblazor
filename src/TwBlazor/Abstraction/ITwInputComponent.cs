// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Utilities;

namespace TwBlazor.Abstraction;

public interface ITwInputComponent
{
    /// <summary>
    /// Gets or sets the unique identifier for the root element.
    /// </summary>
    string RootId { get; set; }

    /// <summary>
    /// Gets or sets the CSS class name applied to the root element of the component.
    /// </summary>
    string RootClass { get; set; }

    /// <summary>
    /// Gets or sets the additional attributed applied to the root element of the component.
    /// </summary>
    Dictionary<string, object> RootAttributes { get; set; }

    /// <summary>
    /// Gets or sets the text label associated with the component.
    /// </summary>
    string Label { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier for the associated label element.
    /// </summary>
    /// <remarks>Use this property to specify the ID of a label element that is associated with the component
    /// for accessibility purposes. Setting this value enables assistive technologies to correctly identify and describe
    /// the component.</remarks>
    string LabelId { get; set; }
    /// <summary>
    /// Gets or sets a collection of additional attributes to apply to the label element.
    /// </summary>
    /// <remarks>Use this property to add arbitrary HTML attributes to the rendered label, such as custom data
    /// attributes, CSS classes, or ARIA attributes. Existing attributes with the same name may be
    /// overwritten.</remarks>
    Dictionary<string, object> LabelAttributes { get; set; }
    /// <summary>
    /// Gets or sets the classes for the label.
    /// </summary>
    string LabelClass { get; set; }

    /// <summary>
    /// Gets or sets whether the input component is readonly.
    /// </summary>
    /// <remarks>When true, the component cannot be edited by the user but will still display its current value.</remarks>
    bool ReadOnly { get; set; }

    /// <summary>
    /// Gets or sets whether the input component is disabled.
    /// </summary>
    /// <remarks>When true, the component is visually dimmed and cannot be interacted with.</remarks>
    bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets whether the input component currently fails validation.
    /// </summary>
    /// <remarks>When true (and <see cref="ErrorMessage"/> is set), the control is marked aria-invalid and
    /// programmatically associated with the rendered error text via aria-describedby, so assistive tech
    /// announces the error alongside the field instead of leaving it undiscoverable.</remarks>
    bool Invalid { get; set; }

    /// <summary>
    /// Gets or sets the validation error message to display and associate with the input.
    /// </summary>
    /// <remarks>Only rendered and referenced by aria-describedby when <see cref="Invalid"/> is true.</remarks>
    string ErrorMessage { get; set; }

    /// <summary>
    /// Gets the CSS classes applied to the label element.
    /// </summary>
    /// <remarks>
    /// Combines the default label styling (block display, margin, font size, and weight) with any custom
    /// classes specified in <see cref="LabelClass"/>.
    /// </remarks>
    public string LabelClasses => new ClassBuilder(LabelClass).Build();

    /// <summary>
    /// Closes the component's picker dialog. Override in picker components to handle close logic.
    /// </summary>
    [Microsoft.JSInterop.JSInvokable("Close")]
    public virtual Task Close() => Task.CompletedTask;
}
