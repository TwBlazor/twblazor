// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Components;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;
using TwBlazor.Models;
using TwBlazor.Utilities;

namespace TwBlazor.Components;

public partial class TwBreadcrumbItem : TwBlazorComponentBase
{
    private TwBreadcrumbTheme theme => options.Theme.Components.Require<TwBreadcrumbTheme>();

    [CascadingParameter] private TwBreadcrumb? parent { get; set; }

    /// <summary>
    /// Allows for dynamic generation of properties for <see cref="TwBreadcrumb"/> instead of defining them inline.
    /// </summary>
    [Parameter] public BreadcrumbItem? Breadcrumb { get; set; }

    /// <inheritdoc cref="BreadcrumbItem.Icon" />
    [Parameter] public Icon? Icon { get; set; }

    /// <inheritdoc cref="BreadcrumbItem.Label" />
    [Parameter] public string? Label { get; set; }

    /// <inheritdoc cref="BreadcrumbItem.Href" />
    [Parameter] public string? Href { get; set; }

    /// <inheritdoc cref="BreadcrumbItem.AriaCurrent" />
    [Parameter] public bool AriaCurrent { get; set; }

    /// <summary>
    /// Explicitly overrides whether this is the first breadcrumb in the trail (suppressing its
    /// leading separator), bypassing the cascading-parent lookup used for inline (ChildContent)
    /// usage. Set by <see cref="TwBreadcrumb"/> when rendering from its <see cref="TwBreadcrumb.Breadcrumbs"/>
    /// list, where items aren't wrapped in a CascadingValue back to the parent.
    /// </summary>
    [Parameter] public bool? IsFirst { get; set; }

    protected override void OnInitialized()
    {
        parent?.AddItem(this);
    }

    internal bool isFirst => IsFirst ?? (parent == null || parent.inlineBreadcrumbs.Count == 0 || parent.inlineBreadcrumbs[0] == this);

    private string labelClasses => new ClassBuilder(theme.Label).AddClass(Class).Build();
}
