using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TwBlazor.Services;

namespace TwBlazor.Tests.Services;

/// <summary>
/// A minimal component used as dialog content across dialog service/component tests.
/// </summary>
public class DialogTestContent : ComponentBase
{
    [Parameter] public string? Message { get; set; }

    [CascadingParameter] public TwDialogInstance? DialogInstance { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "p");
        builder.AddAttribute(1, "class", "dialog-test-content");
        builder.AddContent(2, Message);
        builder.CloseElement();

        builder.OpenElement(3, "button");
        builder.AddAttribute(4, "class", "dialog-test-close");
        builder.AddAttribute(5, "onclick", EventCallback.Factory.Create(this, () => DialogInstance?.Close("closed-from-content")));
        builder.AddContent(6, "Close");
        builder.CloseElement();
    }
}
