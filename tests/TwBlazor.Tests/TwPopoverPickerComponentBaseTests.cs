using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TwBlazor.Components;

namespace TwBlazor.Tests;

public class TwPopoverPickerComponentBaseTests : TwBlazorTestBase
{
    public TwPopoverPickerComponentBaseTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void TriggerInputRef_DefaultsToNull_WhenNotOverriddenByDerivedPicker()
    {
        // Arrange & Act - TwDatePicker/TwTimePicker both override triggerInputRef with a real
        // element; a picker that doesn't must fall back to the base class's own default.
        var cut = TestContext.Render<TestPopoverPickerComponent>();

        // Assert
        Assert.Null(cut.Instance.TriggerInputRef);
    }

    [Fact]
    public async Task OnIconClickAsync_FocusesDefaultElementReference_WhenNoTriggerAndNoInputRoot()
    {
        // Arrange - with no derived triggerInputRef and no rendered InputRoot, OnIconClickAsync's
        // fallback chain (triggerInputRef ?? InputRoot?.RootRef ?? default) bottoms out at a
        // default(ElementReference) rather than throwing.
        var cut = TestContext.Render<TestPopoverPickerComponent>(p => p
            .Add(x => x.WithInputRoot, false));

        // Act
        await cut.Instance.ClickIconAsync();

        // Assert
        var invocation = Assert.Single(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.focusSurface");
        Assert.IsType<ElementReference>(invocation.Arguments[0]);
    }

    [Fact]
    public async Task OnIconClickAsync_FocusesInputRootSurface_WhenNoTriggerButInputRootIsRendered()
    {
        // Arrange - with no derived triggerInputRef but a rendered InputRoot, the fallback should
        // focus the InputRoot's own surface rather than a default(ElementReference).
        var cut = TestContext.Render<TestPopoverPickerComponent>(p => p
            .Add(x => x.WithInputRoot, true));

        // Act
        await cut.Instance.ClickIconAsync();

        // Assert
        var invocation = Assert.Single(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.focusSurface");
        Assert.IsType<ElementReference>(invocation.Arguments[0]);
    }

    [Fact]
    public async Task OnIconClickAsync_DoesNothing_WhenDisabled()
    {
        // Arrange
        var cut = TestContext.Render<TestPopoverPickerComponent>(p => p
            .Add(x => x.Disabled, true));

        // Act
        await cut.Instance.ClickIconAsync();

        // Assert
        Assert.DoesNotContain(TestContext.JSInterop.Invocations, i => i.Identifier == "twDialog.focusSurface");
    }
}

/// <summary>
/// Minimal concrete picker used only to exercise <see cref="TwPopoverPickerComponentBase"/>'s own
/// default behaviour (its <c>triggerInputRef</c> default and <c>OnIconClickAsync</c> fallback chain)
/// independently of any real derived picker, which always supplies its own trigger element.
/// </summary>
public class TestPopoverPickerComponent : TwPopoverPickerComponentBase
{
    [Parameter] public bool WithInputRoot { get; set; } = true;

    public ElementReference? TriggerInputRef => triggerInputRef;

    public Task ClickIconAsync() => OnIconClickAsync();

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (WithInputRoot)
        {
            builder.OpenComponent<TwInputRoot>(0);
            builder.AddComponentReferenceCapture(1, instance => InputRoot = (TwInputRoot)instance);
            builder.CloseComponent();
        }
    }
}
