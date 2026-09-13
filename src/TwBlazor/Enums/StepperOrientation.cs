// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Enums;

/// <summary>
/// Represents the layout direction of a <see cref="TwBlazor.Components.TwStepper"/>.
/// </summary>
public enum StepperOrientation
{
    /// <summary>
    /// Steps flow left-to-right in a row. Below the "sm" breakpoint the full row collapses to a
    /// compact progress bar with a "Step X of N" label, so the control stays usable on narrow screens.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Steps stack top-to-bottom, each with its own connecting line and optional inline content
    /// beneath its label while active. Needs no responsive collapse, since it never overflows horizontally.
    /// </summary>
    Vertical
}
