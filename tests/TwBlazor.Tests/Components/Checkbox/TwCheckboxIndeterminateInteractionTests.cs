using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TwBlazor.Components;

namespace TwBlazor.Tests.Components.Checkbox;

/// <summary>
/// Reproduces the "select all" + three independently-bound children pattern shown in the Checkbox
/// docs page's Indeterminate State example, using a real <see cref="ComponentBase"/> host (not just
/// isolated <see cref="TwCheckbox{T}"/> renders) so the interaction goes through Blazor's normal
/// EventCallback/re-render pipeline exactly as the docs page does.
/// </summary>
public class TwCheckboxIndeterminateInteractionTests : TwBlazorTestBase
{
    [Fact]
    public void SelectAll_StaysIndeterminate_WhenOnlyOneOfThreeChildrenIsUnchecked()
    {
        // Arrange - all three children start checked, so "select all" starts fully checked too.
        var cut = TestContext.Render<SelectAllHost>();
        Assert.True(cut.Find("#select-all").HasAttribute("checked"));

        // Act - untick just one of the three children (two remain checked).
        cut.Find("#apples").Change(false);

        // Assert - "select all" should show indeterminate (a dash), not flip all the way to unchecked.
        var selectAll = cut.Find("#select-all");
        Assert.False(selectAll.HasAttribute("checked"));
        Assert.Single(cut.FindAll("rect"));
    }

    [Fact]
    public void SelectAll_BecomesFullyUnchecked_OnlyAfterAllChildrenAreUnchecked()
    {
        // Arrange
        var cut = TestContext.Render<SelectAllHost>();

        // Act
        cut.Find("#apples").Change(false);
        cut.Find("#bananas").Change(false);
        cut.Find("#cherries").Change(false);

        // Assert - now that all three are unchecked, "select all" should be unchecked with no dash.
        var selectAll = cut.Find("#select-all");
        Assert.False(selectAll.HasAttribute("checked"));
        Assert.Empty(cut.FindAll("rect"));
    }

    private sealed class SelectAllHost : ComponentBase
    {
        private bool _apples = true;
        private bool _bananas = true;
        private bool _cherries = true;

        private bool? SelectAllState =>
            (_apples, _bananas, _cherries) switch
            {
                (true, true, true) => true,
                (false, false, false) => false,
                _ => null
            };

        private void OnSelectAllChanged(bool? value)
        {
            var newValue = value ?? false;
            _apples = newValue;
            _bananas = newValue;
            _cherries = newValue;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<TwCheckbox<bool?>>(0);
            builder.AddComponentParameter(1, "Id", "select-all");
            builder.AddComponentParameter(2, "Value", SelectAllState);
            builder.AddComponentParameter(3, "ValueChanged", EventCallback.Factory.Create<bool?>(this, OnSelectAllChanged));
            builder.CloseComponent();

            builder.OpenComponent<TwCheckbox<bool>>(4);
            builder.AddComponentParameter(5, "Id", "apples");
            builder.AddComponentParameter(6, "Value", _apples);
            builder.AddComponentParameter(7, "ValueChanged", EventCallback.Factory.Create<bool>(this, v => _apples = v));
            builder.CloseComponent();

            builder.OpenComponent<TwCheckbox<bool>>(8);
            builder.AddComponentParameter(9, "Id", "bananas");
            builder.AddComponentParameter(10, "Value", _bananas);
            builder.AddComponentParameter(11, "ValueChanged", EventCallback.Factory.Create<bool>(this, v => _bananas = v));
            builder.CloseComponent();

            builder.OpenComponent<TwCheckbox<bool>>(12);
            builder.AddComponentParameter(13, "Id", "cherries");
            builder.AddComponentParameter(14, "Value", _cherries);
            builder.AddComponentParameter(15, "ValueChanged", EventCallback.Factory.Create<bool>(this, v => _cherries = v));
            builder.CloseComponent();
        }
    }
}
