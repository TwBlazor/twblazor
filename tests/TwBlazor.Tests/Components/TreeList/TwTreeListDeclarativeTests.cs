using Bunit;
using Microsoft.AspNetCore.Components;
using TwBlazor.Components;

namespace TwBlazor.Tests.Components.TreeList;

/// <summary>
/// Covers building a tree by nesting <see cref="TwTreeListItem"/> tags directly, including that
/// checkbox cascading/indeterminate aggregation correctly sees the whole registered subtree.
/// </summary>
public class TwTreeListDeclarativeTests : TwBlazorTestBase
{
    private static RenderFragment BuildDocumentsTree(bool resumeChecked, bool reportChecked) => builder =>
    {
        builder.OpenComponent<TwTreeListItem>(0);
        builder.AddComponentParameter(1, "Label", "Documents");
        builder.AddComponentParameter(2, "ChildContent", (RenderFragment)(childBuilder =>
        {
            childBuilder.OpenComponent<TwTreeListItem>(0);
            childBuilder.AddComponentParameter(1, "Label", "resume.pdf");
            childBuilder.AddComponentParameter(2, "Checked", resumeChecked);
            childBuilder.CloseComponent();

            childBuilder.OpenComponent<TwTreeListItem>(3);
            childBuilder.AddComponentParameter(4, "Label", "report.pdf");
            childBuilder.AddComponentParameter(5, "Checked", reportChecked);
            childBuilder.CloseComponent();
        }));
        builder.CloseComponent();
    };

    [Fact]
    public void RendersLabel_ForDeclarativeItem()
    {
        // Arrange & Act
        RenderFragment tree = builder =>
        {
            builder.OpenComponent<TwTreeListItem>(0);
            builder.AddComponentParameter(1, "Label", "readme.md");
            builder.CloseComponent();
        };
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ChildContent, tree));

        // Assert
        Assert.Contains("readme.md", cut.Find("li").TextContent);
    }

    [Fact]
    public void InvokesOnClick_ForDeclarativeLeaf()
    {
        // Arrange
        var clicked = false;
        RenderFragment tree = builder =>
        {
            builder.OpenComponent<TwTreeListItem>(0);
            builder.AddComponentParameter(1, "Label", "readme.md");
            builder.AddComponentParameter(2, "OnClick", EventCallback.Factory.Create(this, () => clicked = true));
            builder.CloseComponent();
        };
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ChildContent, tree));

        // Act
        cut.Find("li").Click();

        // Assert
        Assert.True(clicked);
    }

    [Fact]
    public void NestedDeclarativeItem_RendersAsChildRow()
    {
        // Arrange & Act - "Documents" defaults to Collapsed, but its children must still exist in the
        // DOM (hidden, not omitted) so checkbox aggregation below can see them regardless of whether
        // the branch is currently expanded.
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ChildContent, BuildDocumentsTree(resumeChecked: false, reportChecked: false)));

        // Assert
        var items = cut.FindAll("li");
        Assert.Equal(3, items.Count);
        Assert.Contains(items, i => i.TextContent.Contains("resume.pdf"));
    }

    [Fact]
    public void ParentCheckbox_ShowsIndeterminate_WhenDeclarativeChildrenDisagree()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ShowCheckboxes, true)
            .Add(p => p.ChildContent, BuildDocumentsTree(resumeChecked: true, reportChecked: false)));

        // Assert - "Documents" is the first <li> in document order.
        var documentsItem = cut.FindAll("li")[0];
        Assert.Equal("mixed", documentsItem.GetAttribute("aria-checked"));
    }

    [Fact]
    public void ParentCheckbox_ShowsChecked_WhenAllDeclarativeChildrenAgree()
    {
        // Arrange & Act
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ShowCheckboxes, true)
            .Add(p => p.ChildContent, BuildDocumentsTree(resumeChecked: true, reportChecked: true)));

        // Assert
        var documentsItem = cut.FindAll("li")[0];
        Assert.Equal("true", documentsItem.GetAttribute("aria-checked"));
    }

    [Fact]
    public void TogglingALeafCheckbox_UpdatesAncestorAggregate_EvenThoughAncestorWasNeverClicked()
    {
        // Arrange - "Documents" itself never receives any click; it can only learn its children
        // disagree via the cascaded root reference triggering a re-render of the whole tree.
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ShowCheckboxes, true)
            .Add(p => p.ChildContent, BuildDocumentsTree(resumeChecked: false, reportChecked: false)));

        // Act - "resume.pdf" is the second checkbox in document order (after "Documents").
        cut.FindAll("input[type='checkbox']")[1].Change(true);

        // Assert
        var documentsItem = cut.FindAll("li")[0];
        Assert.Equal("mixed", documentsItem.GetAttribute("aria-checked"));
    }

    [Fact]
    public void TogglingParentCheckbox_CascadesToDeclarativeChildren()
    {
        // Arrange
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ShowCheckboxes, true)
            .Add(p => p.ChildContent, BuildDocumentsTree(resumeChecked: false, reportChecked: false)));

        // Act - the first checkbox input belongs to "Documents" (document order: parent, then children).
        var checkboxes = cut.FindAll("input[type='checkbox']");
        checkboxes[0].Change(true);

        // Assert - re-query after the change since the tree re-renders.
        var items = cut.FindAll("li");
        Assert.Equal("true", items[0].GetAttribute("aria-checked"));
        Assert.Equal("true", items[1].GetAttribute("aria-checked"));
        Assert.Equal("true", items[2].GetAttribute("aria-checked"));
    }

    [Fact]
    public void CheckedChanged_IsInvoked_ForDirectlyToggledDeclarativeItem()
    {
        // Arrange
        bool? valueFromCallback = null;
        RenderFragment tree = builder =>
        {
            builder.OpenComponent<TwTreeListItem>(0);
            builder.AddComponentParameter(1, "Label", "readme.md");
            builder.AddComponentParameter(2, "CheckedChanged", EventCallback.Factory.Create<bool>(this, v => valueFromCallback = v));
            builder.CloseComponent();
        };
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ShowCheckboxes, true)
            .Add(p => p.ChildContent, tree));

        // Act
        cut.Find("input[type='checkbox']").Change(true);

        // Assert
        Assert.True(valueFromCallback);
    }

    [Fact]
    public void ExpandCollapse_TogglesAriaExpanded_ForDeclarativeParent()
    {
        // Arrange
        var cut = TestContext.Render<TwTreeList>(parameters => parameters
            .Add(p => p.ChildContent, BuildDocumentsTree(resumeChecked: false, reportChecked: false)));

        var documentsItem = cut.FindAll("li")[0];
        Assert.Equal("false", documentsItem.GetAttribute("aria-expanded"));

        // Act
        documentsItem.Click();

        // Assert
        documentsItem = cut.FindAll("li")[0];
        Assert.Equal("true", documentsItem.GetAttribute("aria-expanded"));
    }
}
