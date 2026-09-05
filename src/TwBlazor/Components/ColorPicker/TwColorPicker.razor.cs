// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

/// <summary>
/// A color picker component that supports multiple color formats (Hex, RGB, HSL) with optional alpha channel.
/// Provides both a visual color picker dialog and text input for manual color entry.
/// </summary>
public partial class TwColorPicker : TwBlazorTextInputComponentBase, IAsyncDisposable
{
    private const string defaultColor = "#000000";
    /// <summary>
    /// Gets or sets the JavaScript runtime for interop operations.
    /// </summary>
    [Inject] public IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the current color value. Supports Hex, RGB, and HSL formats.
    /// Default is "#000000" (black).
    /// </summary>
    [Parameter] public string Value { get; set; } = defaultColor;
    /// <summary>
    /// Gets or sets the event callback that is invoked when the color value changes.
    /// </summary>
    [Parameter] public EventCallback<string> ValueChanged { get; set; } = default!;
    /// <summary>
    /// Gets or sets the event callback that is invoked when the input receives focus.
    /// </summary>
    [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; } = default!;
    /// <summary>
    /// Gets or sets the event name for binding. Default is "onchange".
    /// </summary>
    [Parameter] public string BindEvent { get; set; } = "onchange";
    /// <summary>
    /// Gets or sets whether to show and allow editing of the alpha (transparency) channel.
    /// Default is false.
    /// </summary>
    [Parameter] public bool ShowAlpha { get; set; } = false;
    /// <summary>
    /// Gets or sets whether to show the mode switch button to toggle between color formats.
    /// Default is false.
    /// </summary>
    [Parameter] public bool ShowModeSwitch { get; set; } = false;
    /// <summary>
    /// Gets or sets the output format for the color value (Hex, RGB, or HSL).
    /// Default is <see cref="ColorMode.Hex"/>.
    /// </summary>
    [Parameter] public ColorMode OutputFormat { get; set; } = ColorMode.Hex;

    /// <summary>
    /// Overrides automatic device detection for whether the browser's native color picker should be used
    /// instead of the custom popover dialog. Leave unset (<see langword="null"/>) to auto-detect based on
    /// the client platform (iOS and Android use the native picker by default).
    /// </summary>
    [Parameter] public bool? PreferNativePicker { get; set; }

    /// <summary>
    /// Indicates whether the browser's native color input UI is being used for the swatch preview instead
    /// of the custom dialog, either because <see cref="PreferNativePicker"/> was explicitly set or because
    /// the client platform (iOS/Android) was detected via JS interop.
    /// </summary>
    private bool useNativePicker;

    private TwInputRoot? inputRoot;
    private DotNetObjectReference<TwColorPicker>? dotNetRef;
    private bool registeredOutsideHandler;
    private string displayValue = string.Empty;
    private bool showDialog = false;

    /// <summary>
    /// Opaque token (captured via JS interop from the element focused just before the dialog opened,
    /// almost always the swatch) used to restore focus there once the dialog closes.
    /// </summary>
    private string? focusReturnToken;

    /// <summary>
    /// Reference to the popover dialog wrapper element, used to move focus into it when it opens.
    /// </summary>
    private ElementReference panelRef;

    /// <summary>
    /// Set when the dialog opens so the next <see cref="OnAfterRenderAsync"/> moves focus into it.
    /// </summary>
    private bool pendingOpenFocus;

    private TwColorPickerTheme colorPickerTheme => options.Theme.Components.Require<TwColorPickerTheme>();

    private string inputContainerClasses => new ClassBuilder(colorPickerTheme.InputContainer)
        .Build();

    private string previewClasses => new ClassBuilder(colorPickerTheme.Swatch)
        .AddClass(Disabled ? colorPickerTheme.SwatchDisabled : colorPickerTheme.SwatchHover)
        .Build();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        displayValue = Value;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (string.IsNullOrEmpty(RootId))
        {
            RootId = Guid.NewGuid().ToString("N");
        }

        displayValue = Value;
    }

    /// <summary>
    /// Determines, via <see cref="PreferNativePicker"/> or JS-based device detection, whether the browser's
    /// native color input should be used for the swatch preview instead of the custom dialog.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            useNativePicker = PreferNativePicker ?? await DeviceDetector.PrefersNativePickerAsync(JSRuntime);
            if (useNativePicker)
            {
                StateHasChanged();
            }
        }

        // Move focus into the color picker dialog whenever it opens. Focuses the first focusable
        // element within the dialog (falls back to the dialog surface itself). Also (re-)arm the Tab
        // focus trap and background inert-ing every time the dialog (re)opens.
        if (showDialog && pendingOpenFocus && panelRef.Context != null)
        {
            pendingOpenFocus = false;
            await JSRuntime.InvokeVoidAsync("twPicker.positionPanel", panelRef);
            await JSRuntime.InvokeVoidAsync("twDialog.trapFocus", panelRef);
            await JSRuntime.InvokeVoidAsync("twDialog.setBackgroundInert", inputRoot?.RootRef);
            await JSRuntime.InvokeVoidAsync("twDialog.focusSurface", panelRef);
        }
    }

    /// <summary>
    /// Releases the Tab focus trap and clears background inert-ing. Must be called (and awaited)
    /// while the dialog is still mounted - i.e. before <see cref="showDialog"/> is set to false -
    /// since it needs <see cref="panelRef"/> to still resolve to a live DOM node.
    /// </summary>
    private async Task ReleasePanelTrapAsync()
    {
        await JSRuntime.InvokeVoidAsync("twDialog.releaseFocusTrap", panelRef);
        await JSRuntime.InvokeVoidAsync("twDialog.clearBackgroundInert");
    }

    /// <summary>
    /// Gets the accessible name for the custom color swatch, reflecting the currently selected color
    /// instead of a static, never-updating label.
    /// </summary>
    private string swatchAriaLabel => $"Selected color: {Value}";

    private string GetPlaceholder()
    {
        return OutputFormat switch
        {
            ColorMode.Rgb => ShowAlpha ? "rgba(r, g, b, a)" : "rgb(r, g, b)",
            ColorMode.Hsl => ShowAlpha ? "hsla(h, s%, l%, a)" : "hsl(h, s%, l%)",
            _ => ShowAlpha ? "#RRGGBBAA" : "#RRGGBB"
        };
    }

    private async Task HandleTextInputChangeAsync()
    {
        if (!string.IsNullOrWhiteSpace(displayValue))
        {
            Value = NormalizeColorValue(displayValue);

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }
        }
    }

    private string NormalizeColorValue(string colorValue)
    {
        if (string.IsNullOrWhiteSpace(colorValue)) return defaultColor;

        colorValue = colorValue.Trim();

        // If it's already RGB or HSL format, return as-is
        if (colorValue.StartsWith("rgb", StringComparison.OrdinalIgnoreCase) ||
            colorValue.StartsWith("hsl", StringComparison.OrdinalIgnoreCase))
        {
            return colorValue;
        }

        // Otherwise treat as hex and normalize
        return NormalizeHexValue(colorValue);
    }

    private string NormalizeHexValue(string hexValue)
    {
        if (string.IsNullOrWhiteSpace(hexValue)) return defaultColor;

        hexValue = hexValue.Trim();
        if (!hexValue.StartsWith('#')) hexValue = "#" + hexValue;

        var hex = hexValue.TrimStart('#');

        if (!ShowAlpha && hex.Length == 8)
        {
            hex = hex[..6];
        }

        return "#" + hex;
    }

    private string GetPreviewColor()
    {
        if (string.IsNullOrWhiteSpace(Value)) return defaultColor;

        // If it's RGB or HSL, convert to hex for preview
        if (Value.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
        {
            return ColorConverter.RgbToHex(Value, includeAlpha: ShowAlpha, fallbackValue: defaultColor);
        }
        else if (Value.StartsWith("hsl", StringComparison.OrdinalIgnoreCase))
        {
            return ColorConverter.HslToHex(Value, includeAlpha: ShowAlpha, fallbackValue: defaultColor);
        }

        return Value;
    }

    private async Task ShowDialog()
    {
        if (!Disabled && !ReadOnly)
        {
            // Capture whatever currently has focus (almost always the swatch, since clicking or
            // activating it is what triggers this) so it can be restored once the dialog closes.
            focusReturnToken = await JSRuntime.InvokeAsync<string?>("twDialog.captureFocus");
            showDialog = true;
            pendingOpenFocus = true;
            await RegisterOutsideClickAsync();
        }
    }

    /// <summary>
    /// Restores focus to whatever element was focused (captured via <see cref="focusReturnToken"/>) right
    /// before the dialog opened, typically the swatch. No-ops if no token was captured (e.g. the dialog
    /// is being closed a second time).
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="TwDatePicker"/>/<see cref="TwTimePicker"/>, this component doesn't need a
    /// "suppress the next focus-triggered reopen" guard: the dialog only ever opens from
    /// <see cref="ShowDialog"/>, which is wired to the swatch's <c>@onclick</c>/<c>@onkeydown</c>
    /// (Enter/Space), not an <c>@onfocus</c> handler. The <c>.focus()</c> call this method makes via
    /// <c>twDialog.restoreFocus</c> therefore can't re-trigger <see cref="ShowDialog"/> the way it
    /// could re-trigger those other components' focus-driven open handlers.
    /// </remarks>
    private async Task RestoreFocusAsync()
    {
        if (string.IsNullOrEmpty(focusReturnToken)) return;

        var token = focusReturnToken;
        focusReturnToken = null;
        await JSRuntime.InvokeVoidAsync("twDialog.restoreFocus", token);
    }

    /// <summary>
    /// Handles color changes from the native &lt;input type="color"&gt; swatch, used instead of the custom
    /// dialog when <see cref="useNativePicker"/> is <see langword="true"/>.
    /// </summary>
    private async Task OnNativeColorChangedAsync(ChangeEventArgs e)
    {
        var newValue = e.Value?.ToString();
        if (string.IsNullOrWhiteSpace(newValue)) return;

        Value = newValue;
        displayValue = newValue;
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }

    private async Task OnDialogValueChanged(string newValue)
    {
        Value = newValue;
        displayValue = newValue;
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }
        StateHasChanged();
    }

    private async Task OnDialogClose(bool confirmed)
    {
        await ReleasePanelTrapAsync();
        showDialog = false;
        await UnregisterOutsideClickAsync();
        await RestoreFocusAsync();
        StateHasChanged();
    }

    private async Task OnDialogKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            await ReleasePanelTrapAsync();
            showDialog = false;
            await UnregisterOutsideClickAsync();
            await RestoreFocusAsync();
            StateHasChanged();
        }
    }

    private async Task RegisterOutsideClickAsync()
    {
        if (registeredOutsideHandler) return;
        dotNetRef ??= DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("twPicker.registerOutsideClick", inputRoot?.RootRef, dotNetRef);
        registeredOutsideHandler = true;
    }

    private async Task UnregisterOutsideClickAsync()
    {
        if (!registeredOutsideHandler) return;
        await JSRuntime.InvokeVoidAsync("twPicker.unregisterOutsideClick", inputRoot?.RootRef);
        dotNetRef?.Dispose();
        dotNetRef = null;
        registeredOutsideHandler = false;
    }

    /// <summary>
    /// Closes the color picker dialog. This method is invoked from JavaScript when clicking outside the dialog.
    /// </summary>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    [JSInvokable("Close")]
    public override async Task Close()
    {
        if (showDialog)
        {
            await ReleasePanelTrapAsync();
        }
        showDialog = false;
        await UnregisterOutsideClickAsync();
        await RestoreFocusAsync();
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        await UnregisterOutsideClickAsync();
        GC.SuppressFinalize(this);
    }
}
