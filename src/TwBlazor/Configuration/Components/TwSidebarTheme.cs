// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for the sidebar component (<see cref="TwBlazor.Components.TwSidebar"/>).
/// Override any property to customize sidebar styles globally.
/// </summary>
[ExcludeFromCodeCoverage]
public class TwSidebarTheme
{
    /// <summary>
    /// Gets or sets the classes for the top navbar.
    /// </summary>
    public required string Navbar { get; set; }

    /// <summary>
    /// Gets or sets the classes for the sidebar panel itself.
    /// </summary>
    public required string Sidebar { get; set; }

    /// <summary>
    /// Gets or sets the classes for the main content area beside the sidebar.
    /// </summary>
    public required string MainContent { get; set; }

    /// <summary>
    /// Gets or sets the classes for the root element wrapping the main content area.
    /// </summary>
    public required string MainContentRoot { get; set; }

    /// <summary>
    /// Gets or sets the base classes applied to every navigation item.
    /// </summary>
    public required string NavigationItemBase { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the currently active navigation item.
    /// </summary>
    public required string NavigationItemActive { get; set; }

    /// <summary>
    /// Gets or sets the classes for the dropdown container that holds a navigation item's nested items.
    /// </summary>
    public required string NavigationDropdownContainer { get; set; }
}
