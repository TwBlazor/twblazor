// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Configuration;
using TwBlazor.Enums;

namespace TwBlazor.Abstraction;

/// <summary>
/// Defines the contract for all TwBlazor components.
/// </summary>
public interface ITwComponent
{
    /// <summary>
    /// Gets or sets the unique identifier for the component.
    /// </summary>
    /// <remarks>If not provided, a unique ID will be automatically generated.</remarks>
    string? Id { get; set; }

    /// <summary>
    /// Gets or sets the CSS class(es) to apply to the component.
    /// </summary>
    string Class { get; set; }

    /// <summary>
    /// Gets or sets inline CSS styles to apply to the component.
    /// </summary>
    string? Style { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes to apply to the component.
    /// </summary>
    /// <remarks>This captures any unmatched HTML attributes that can be passed to the component.</remarks>
    Dictionary<string, object> Attributes { get; set; }

    /// <summary>
    /// Gets or sets the ARIA label for accessibility.
    /// </summary>
    /// <remarks>Provides a text label for assistive technologies when a visible label is not present.</remarks>
    string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the ID of another element that labels this component for accessibility.
    /// </summary>
    /// <remarks>References another element that serves as the label for this component.</remarks>
    string? AriaLabelledBy { get; set; }

    /// <summary>
    /// Gets or sets the shadow (elevation) level for the component.
    /// </summary>
    /// <remarks>
    /// If not set, uses the global default from <see cref="TwBlazorShadow.DefaultShadow"/>.
    /// Set to <see cref="Shadow.None"/> to explicitly remove shadow.
    /// </remarks>
    Shadow? Shadow { get; set; }

    /// <summary>
    /// Gets or sets the border radius for the component.
    /// </summary>
    /// <remarks>
    /// If not set, uses the global default from <see cref="TwBlazorRounded.DefaultRounded"/>.
    /// Set to <see cref="Rounded.None"/> to explicitly remove border radius.
    /// </remarks>
    Rounded? Rounded { get; set; }
}
