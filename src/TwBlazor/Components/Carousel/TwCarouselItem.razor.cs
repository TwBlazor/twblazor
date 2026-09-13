// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;

namespace TwBlazor.Components;

/// <summary>
/// Represents a single slide within a <see cref="TwCarousel"/>.
/// </summary>
/// <remarks>A TwCarouselItem is only ever used as a child of a <see cref="TwCarousel"/>. It renders nothing
/// itself - its <see cref="ChildContent"/> is rendered by the parent carousel only while this slide is the
/// active one.</remarks>
public partial class TwCarouselItem : TwBlazorComponentBase
{
    /// <summary>
    /// The parent carousel that this slide belongs to. This property is required and is set via cascading parameters.
    /// </summary>
    [CascadingParameter] public required TwCarousel Parent { get; set; }

    /// <summary>
    /// The content to display while this slide is active. This is a required parameter.
    /// </summary>
    [Parameter] public required RenderFragment ChildContent { get; set; }

    protected override void OnInitialized()
    {
        Parent.RegisterItem(this);
        base.OnInitialized();
    }
}
