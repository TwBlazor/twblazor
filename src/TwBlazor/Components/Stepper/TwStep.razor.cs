// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Enums;

namespace TwBlazor.Components;

/// <summary>
/// Represents a single step within a <see cref="TwStepper"/>.
/// </summary>
/// <remarks>
/// A TwStep declares no markup of its own - <see cref="TwStepper"/> renders every step's indicator,
/// label, and description itself (the same registration pattern <see cref="TwTab"/> uses with
/// <see cref="TwTabContainer"/>), so declaring a TwStep just adds an entry to its parent's step list,
/// in the order it appears in markup.
/// </remarks>
public partial class TwStep : TwBlazorComponentBase
{
    /// <summary>
    /// The parent stepper this step belongs to. Set via cascading parameter.
    /// </summary>
    [CascadingParameter] public required TwStepper Parent { get; set; }

    /// <summary>
    /// Gets or sets the label displayed for this step.
    /// </summary>
    [Parameter] public required string Label { get; set; }

    /// <summary>
    /// Gets or sets a short secondary description shown beneath the label (e.g. "Optional").
    /// </summary>
    [Parameter] public string? Description { get; set; }

    /// <summary>
    /// Gets or sets whether this step is disabled and cannot be navigated to.
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets an icon to display in the step's indicator instead of its step number, while the
    /// step is active or upcoming. A completed step always shows a checkmark regardless of this value.
    /// </summary>
    [Parameter] public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets optional content for this step. On <see cref="StepperOrientation.Horizontal"/>, it
    /// is rendered in a shared panel below the step row while this step is active; on
    /// <see cref="StepperOrientation.Vertical"/>, it is rendered inline beneath this step's label while
    /// this step is active.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent.RegisterStep(this);
    }
}
