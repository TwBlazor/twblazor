// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the carousel components (<see cref="TwBlazor.Components.TwCarousel"/>,
/// <see cref="TwBlazor.Components.TwCarouselItem"/>).
/// Override any property to customize carousel styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwCarouselTheme
{
    /// <summary>
    /// Gets or sets the classes for the root container element.
    /// </summary>
    public required string Container { get; set; }

    /// <summary>
    /// Gets or sets the classes for the viewport that holds the currently visible slide and the
    /// overlaid arrow/pause controls.
    /// </summary>
    public required string Viewport { get; set; }

    /// <summary>
    /// Gets or sets the classes for the wrapper rendered around each slide's content.
    /// </summary>
    public required string Slide { get; set; }

    /// <summary>
    /// Gets or sets the classes shared by both the previous and next arrow buttons.
    /// </summary>
    public required string ArrowButton { get; set; }

    /// <summary>
    /// Gets or sets the classes that position the previous arrow button at the start edge of the viewport.
    /// </summary>
    public required string ArrowButtonStart { get; set; }

    /// <summary>
    /// Gets or sets the classes that position the next arrow button at the end edge of the viewport.
    /// </summary>
    public required string ArrowButtonEnd { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to an arrow button when it can no longer navigate (only reachable
    /// when <see cref="TwBlazor.Components.TwCarousel.Loop"/> is disabled and the boundary slide is active).
    /// </summary>
    public required string ArrowButtonDisabled { get; set; }

    /// <summary>
    /// Gets or sets the classes for the automatic-playback pause/play toggle button, shown whenever
    /// <see cref="TwBlazor.Components.TwCarousel.AutoPlay"/> is enabled.
    /// </summary>
    public required string PlayPauseButton { get; set; }

    /// <summary>
    /// Gets or sets the classes for the row of slide-picker indicator dots.
    /// </summary>
    public required string IndicatorContainer { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to every indicator dot, regardless of state.
    /// </summary>
    public required string Indicator { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the indicator dot for the currently selected slide, when no
    /// explicit <see cref="TwBlazor.Components.TwCarousel.Color"/> is set.
    /// </summary>
    public required string IndicatorActive { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to indicator dots for slides that are not currently selected.
    /// </summary>
    public required string IndicatorInactive { get; set; }
}
