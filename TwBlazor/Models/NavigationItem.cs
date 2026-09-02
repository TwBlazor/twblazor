// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using TwBlazor.Enums;

namespace TwBlazor.Models;

/// <summary>
/// Represents a navigation item in a menu or sidebar.
/// </summary>
[ExcludeFromCodeCoverage]
public class NavigationItem
{
    public string? Id { get; set; }
    public string? Href { get; set; }
    public Icon? Icon { get; set; }
    public string? Label { get; set; }
    public List<NavigationItem> NavigationItems { get; set; } = [];
    public bool Collapsed { get; set; } = true;
    public bool TopNavigation { get; set; }
    public bool Hidden { get; set; }
    public bool New { get; set; }
}
