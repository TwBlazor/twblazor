// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Abstraction;
using TwBlazor.Utilities;

namespace TwBlazor;

public class TwBlazorInputComponentBase : TwBlazorComponentBase, ITwInputComponent
{
    /// <inheritdoc cref="ITwInputComponent.RootId" />
    [Parameter] public string RootId { get; set; } = string.Empty;

    /// <inheritdoc cref="ITwInputComponent.RootClass" />
    [Parameter] public string RootClass { get; set; } = string.Empty;

    /// <inheritdoc cref="ITwInputComponent.RootAttributes" />
    [Parameter] public Dictionary<string, object> RootAttributes { get; set; } = [];

    /// <inheritdoc cref="ITwInputComponent.Label" />
    [Parameter] public string Label { get; set; } = string.Empty;

    /// <inheritdoc cref="ITwInputComponent.LabelId" />
    [Parameter] public string LabelId { get; set; } = string.Empty;

    /// <inheritdoc cref="ITwInputComponent.LabelAttributes" />
    [Parameter] public Dictionary<string, object> LabelAttributes { get; set; } = [];

    /// <inheritdoc cref="ITwInputComponent.LabelClass" />
    [Parameter] public string LabelClass { get; set; } = string.Empty;

    /// <inheritdoc cref="ITwInputComponent.ReadOnly" />
    [Parameter] public bool ReadOnly { get; set; }

    /// <inheritdoc cref="ITwInputComponent.Disabled" />
    [Parameter] public bool Disabled { get; set; }

    /// <inheritdoc cref="ITwInputComponent.Invalid" />
    [Parameter] public bool Invalid { get; set; }

    /// <inheritdoc cref="ITwInputComponent.ErrorMessage" />
    [Parameter] public string ErrorMessage { get; set; } = string.Empty;

    /// <inheritdoc cref="ITwInputComponent.LabelClasses" />
    public string LabelClasses => new ClassBuilder(LabelClass ?? string.Empty).Build();

    /// <summary>
    /// Gets the id of the rendered error message element when the field is invalid, or null otherwise.
    /// Leaf components use this both to render the error text (via TwInputRoot) and to reference it from
    /// the control's aria-describedby, so assistive tech announces the error alongside the field.
    /// </summary>
    protected string? errorId => Invalid && !string.IsNullOrWhiteSpace(ErrorMessage) ? $"{Id}-error" : null;

    /// <summary>
    /// Closes the component's picker dialog. Override in picker components to handle close logic.
    /// </summary>
    [Microsoft.JSInterop.JSInvokable("Close")]
    public virtual Task Close() => Task.CompletedTask;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // If the consumer passed Attributes (unmatched values) on the input component itself,
        // prefer forwarding them to the RootAttributes so they end up on the TwInputRoot element.
        if ((RootAttributes == null || RootAttributes.Count == 0) && Attributes != null && Attributes.Count > 0)
        {
            RootAttributes = Attributes;
        }
    }
}
