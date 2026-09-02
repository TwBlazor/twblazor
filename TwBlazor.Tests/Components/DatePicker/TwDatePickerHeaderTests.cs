using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components.DatePicker;

namespace TwBlazor.Tests.Components.DatePicker;

public class TwDatePickerHeaderTests : TwBlazorTestBase
{
    [Fact]
    public void RendersButtons_AndInvokesCallbacks()
    {
        // Arrange
        var previousInvoked = false;
        var nextInvoked = false;
        var titleInvoked = false;

        // Act & Assert
        var cut = TestContext.Render<TwDatePickerHeader>(p => p
            .Add(x => x.OnPreviousClick, EventCallback.Factory.Create(this, () => previousInvoked = true))
            .Add(x => x.OnNextClick, EventCallback.Factory.Create(this, () => nextInvoked = true))
            .Add(x => x.OnTitleClick, EventCallback.Factory.Create(this, () => titleInvoked = true))
            .AddChildContent("Title")
        );

        var buttons = cut.FindAll("button");
        Assert.True(buttons.Count >= 3);

        // Previous
        buttons[0].Click();
        // Title switch
        buttons[1].Click();
        // Next
        buttons[2].Click();

        Assert.True(previousInvoked);
        Assert.True(titleInvoked);
        Assert.True(nextInvoked);
    }
}