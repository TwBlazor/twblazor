// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// Represents a carousel that displays one slide (<see cref="TwCarouselItem"/>) at a time, with optional
/// arrow navigation, slide-picker indicators, swipe gestures, and automatic playback.
/// </summary>
/// <remarks>Use this component together with <see cref="TwCarouselItem"/> children, each representing one
/// slide. Only the slide at <see cref="SelectedIndex"/> is rendered at any given time - there is no slide
/// transition animation, keeping the component's output and behavior simple and predictable.</remarks>
public partial class TwCarousel : TwBlazorComponentBase, IAsyncDisposable
{
    /// <summary>
    /// The minimum horizontal swipe distance, in pixels, for <see cref="EnableSwipeGesture"/> to treat a
    /// touch gesture as a slide change rather than an incidental tap or drag.
    /// </summary>
    private const double swipeThreshold = 50;

    private readonly List<TwCarouselItem> _items = [];

    private double? touchStartX;

    private bool isPointerActive;

    private bool isManuallyPaused;

    private Timer? autoPlayTimer;

    private TwCarouselTheme theme => options.Theme.Components.Require<TwCarouselTheme>();

    /// <summary>
    /// Gets or sets the index of the currently displayed slide. Supports two-way binding via
    /// <see cref="SelectedIndexChanged"/>.
    /// </summary>
    [Parameter] public int SelectedIndex { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked whenever <see cref="SelectedIndex"/> changes, whether from the
    /// arrow buttons, an indicator, a keyboard press, a swipe gesture, or automatic playback.
    /// </summary>
    [Parameter] public EventCallback<int> SelectedIndexChanged { get; set; }

    /// <summary>
    /// Gets or sets whether the previous/next arrow buttons are displayed. Default is <see langword="true"/>.
    /// </summary>
    [Parameter] public bool ShowArrows { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the slide-picker indicator dots are displayed below the carousel. Default is
    /// <see langword="true"/>.
    /// </summary>
    [Parameter] public bool ShowIndicators { get; set; } = true;

    /// <summary>
    /// Gets or sets whether a horizontal touch swipe switches between slides. Default is
    /// <see langword="true"/>.
    /// </summary>
    [Parameter] public bool EnableSwipeGesture { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the carousel automatically advances to the next slide on a timer. A visible
    /// pause/play toggle is rendered whenever this is enabled, so playback can always be stopped (WCAG
    /// 2.2.2), and playback also pauses automatically while the pointer or keyboard focus is within the
    /// carousel. Default is <see langword="false"/>.
    /// </summary>
    [Parameter] public bool AutoPlay { get; set; }

    /// <summary>
    /// Gets or sets the delay between automatic slide changes when <see cref="AutoPlay"/> is enabled.
    /// Default is 5 seconds.
    /// </summary>
    [Parameter] public TimeSpan AutoPlayInterval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets whether navigating past the last slide wraps around to the first (and vice versa).
    /// When <see langword="false"/>, the arrow buttons are disabled at the boundaries instead. Default is
    /// <see langword="true"/>.
    /// </summary>
    [Parameter] public bool Loop { get; set; } = true;

    /// <summary>
    /// Gets or sets the accent color used for the active slide indicator dot. If <see langword="null"/>,
    /// the theme's default indicator color is used.
    /// </summary>
    [Parameter] public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="TwCarouselItem"/> children that make up the carousel's slides.
    /// </summary>
    [Parameter] public required RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Gets the currently selected slide, or <see langword="null"/> if <see cref="SelectedIndex"/> is out of
    /// range (e.g. no slides have registered yet).
    /// </summary>
    public TwCarouselItem? ActiveItem => SelectedIndex >= 0 && SelectedIndex < _items.Count ? _items[SelectedIndex] : null;

    /// <summary>
    /// Gets whether the currently selected slide is the first one.
    /// </summary>
    public bool IsFirstSlide => SelectedIndex <= 0;

    /// <summary>
    /// Gets whether the currently selected slide is the last one.
    /// </summary>
    public bool IsLastSlide => SelectedIndex >= _items.Count - 1;

    /// <summary>
    /// Gets whether automatic playback is currently paused, either because the user toggled the pause
    /// control, or because the pointer or keyboard focus is currently within the carousel.
    /// </summary>
    private bool isAutoPlayPaused => isManuallyPaused || isPointerActive;

    private string effectiveAriaLabel => string.IsNullOrEmpty(AriaLabel) && string.IsNullOrEmpty(AriaLabelledBy) ? "Carousel" : AriaLabel ?? string.Empty;

    private string containerClasses => new ClassBuilder(theme.Container)
        .AddClass(roundedBuilder.GetRounded(effectiveRounded))
        .AddClass(Class).Build();

    private string previousArrowClasses => new ClassBuilder(theme.ArrowButton)
        .AddClass(theme.ArrowButtonStart)
        .AddClass(theme.ArrowButtonDisabled, !Loop && IsFirstSlide).Build();

    private string nextArrowClasses => new ClassBuilder(theme.ArrowButton)
        .AddClass(theme.ArrowButtonEnd)
        .AddClass(theme.ArrowButtonDisabled, !Loop && IsLastSlide).Build();

    /// <summary>
    /// Registers a slide with the carousel. Slides register themselves, in markup order, from
    /// <see cref="TwCarouselItem.OnInitialized"/>.
    /// </summary>
    /// <param name="item">The slide to register.</param>
    internal void RegisterItem(TwCarouselItem item)
    {
        if (_items.Contains(item))
        {
            return;
        }

        _items.Add(item);
        StateHasChanged();
    }

    /// <summary>
    /// Advances to the next slide, wrapping around to the first slide when <see cref="Loop"/> is enabled.
    /// Does nothing if already on the last slide and <see cref="Loop"/> is disabled.
    /// </summary>
    public Task NextSlide()
    {
        if (_items.Count == 0)
        {
            return Task.CompletedTask;
        }

        if (IsLastSlide)
        {
            return Loop ? SetSelectedIndex(0) : Task.CompletedTask;
        }

        return SetSelectedIndex(SelectedIndex + 1);
    }

    /// <summary>
    /// Moves to the previous slide, wrapping around to the last slide when <see cref="Loop"/> is enabled.
    /// Does nothing if already on the first slide and <see cref="Loop"/> is disabled.
    /// </summary>
    public Task PreviousSlide()
    {
        if (_items.Count == 0)
        {
            return Task.CompletedTask;
        }

        if (IsFirstSlide)
        {
            return Loop ? SetSelectedIndex(_items.Count - 1) : Task.CompletedTask;
        }

        return SetSelectedIndex(SelectedIndex - 1);
    }

    /// <summary>
    /// Navigates directly to the slide at <paramref name="index"/>. Does nothing if the index is out of
    /// range or already selected.
    /// </summary>
    /// <param name="index">The zero-based index of the slide to display.</param>
    public Task GoToSlide(int index)
    {
        if (index < 0 || index >= _items.Count || index == SelectedIndex)
        {
            return Task.CompletedTask;
        }

        return SetSelectedIndex(index);
    }

    private async Task SetSelectedIndex(int index)
    {
        SelectedIndex = index;
        await SelectedIndexChanged.InvokeAsync(index);
    }

    private string GetIndicatorClasses(int index)
    {
        var builder = new ClassBuilder(theme.Indicator);

        if (index != SelectedIndex)
        {
            return builder.AddClass(theme.IndicatorInactive).Build();
        }

        return Color.HasValue
            ? builder.AddClass(colorBuilder.GetFilledVariantColor(Color)).Build()
            : builder.AddClass(theme.IndicatorActive).Build();
    }

    private void ToggleAutoPlayPaused() => isManuallyPaused = !isManuallyPaused;

    private void HandlePointerEnter() => isPointerActive = true;

    private void HandlePointerLeave() => isPointerActive = false;

    private Task HandleKeyDown(KeyboardEventArgs e) => e.Key switch
    {
        "ArrowLeft" => PreviousSlide(),
        "ArrowRight" => NextSlide(),
        _ => Task.CompletedTask,
    };

    private void HandleTouchStart(TouchEventArgs e)
    {
        if (!EnableSwipeGesture || e.ChangedTouches.Length == 0)
        {
            return;
        }

        touchStartX = e.ChangedTouches[0].ClientX;
    }

    private Task HandleTouchEnd(TouchEventArgs e)
    {
        if (!EnableSwipeGesture || touchStartX is not { } startX || e.ChangedTouches.Length == 0)
        {
            return Task.CompletedTask;
        }

        var deltaX = e.ChangedTouches[0].ClientX - startX;
        touchStartX = null;

        if (Math.Abs(deltaX) < swipeThreshold)
        {
            return Task.CompletedTask;
        }

        return deltaX < 0 ? NextSlide() : PreviousSlide();
    }

    /// <summary>
    /// Starts the automatic-playback timer once, after the first render, when <see cref="AutoPlay"/> is
    /// enabled. The timer's own callback re-checks <see cref="AutoPlay"/> and <see cref="isAutoPlayPaused"/>
    /// on every tick, so toggling <see cref="AutoPlay"/> off later simply stops it from advancing rather
    /// than needing to be recreated.
    /// </summary>
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender && AutoPlay)
        {
            autoPlayTimer = new Timer(OnAutoPlayTick, null, AutoPlayInterval, AutoPlayInterval);
        }
    }

    private void OnAutoPlayTick(object? state)
    {
        if (!AutoPlay || isAutoPlayPaused)
        {
            return;
        }

        _ = InvokeAsync(async () =>
        {
            await NextSlide();
            StateHasChanged();
        });
    }

    /// <summary>
    /// Stops and releases the automatic-playback timer, if one was started.
    /// </summary>
    public ValueTask DisposeAsync()
    {
        autoPlayTimer?.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
