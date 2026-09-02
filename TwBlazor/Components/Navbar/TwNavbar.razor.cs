// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

public partial class TwNavbar : TwBlazorComponentBase
{
    private TwSidebarTheme theme => options.Theme.Components.Require<TwSidebarTheme>();

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the navbar should be fixed at the top.
    /// </summary>
    [Parameter] public bool Fixed { get; set; }

    private string navbarClasses =>
        new ClassBuilder(theme.Navbar)
            .AddClass("sticky top-0", !Fixed)
            .AddClass("fixed top-0 left-0 right-0", Fixed)
            .AddClass(Class).Build();

    // "navigation bar" restates the <nav> landmark's own role, which is redundant for assistive
    // technology (already announced as "navigation") and indistinguishable from other <nav> regions
    // on the page (e.g. TwSidebar's "sidebar navigation"). "Top navigation" actually distinguishes it.
    protected override void OnInitialized()
    {
        if (string.IsNullOrWhiteSpace(AriaLabel))
            AriaLabel = "Top navigation";

        base.OnInitialized();
    }
}
