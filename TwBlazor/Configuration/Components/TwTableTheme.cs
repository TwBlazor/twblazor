// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Components;

/// <summary>
/// Theme configuration for table components (<see cref="TwBlazor.Components.TwTable"/> and
/// <see cref="TwBlazor.Components.TwDataTable{TItem}"/>).
/// Override any property to customize table styles globally.
/// </summary>
/// <remarks>
/// <see cref="TwBlazor.Components.TwDataTable{TItem}"/> does not define its own table theme - its
/// auto-generated header row (used when rendering <c>Columns</c>) reuses <see cref="Header"/> so both
/// components stay visually consistent from a single configuration point.
/// </remarks>
[ExcludeFromCodeCoverage]
public class TwTableTheme
{
    /// <summary>
    /// Gets or sets the base classes applied to the table element itself.
    /// </summary>
    public required string Base { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the table element when its <c>NoBorder</c> parameter is <c>false</c> (the default).
    /// </summary>
    public required string Bordered { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the table header, controlling its background, text colour and typography.
    /// </summary>
    public required string Header { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to header cells when the table's <c>Bordered</c> parameter is <c>true</c>.
    /// </summary>
    public required string HeaderBorderedCells { get; set; }

    /// <summary>
    /// Gets or sets the base background classes applied to the table body.
    /// </summary>
    public required string Body { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the table body when its <c>Striped</c> parameter is <c>true</c>.
    /// </summary>
    public required string BodyStriped { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to the table body when its <c>Hoverable</c> parameter is <c>true</c>.
    /// </summary>
    public required string BodyHoverable { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to data cells (<c>td</c>) in the body and footer when <c>Bordered</c> is <c>true</c>.
    /// </summary>
    public required string BorderedCells { get; set; }

    /// <summary>
    /// Gets or sets the classes applied to header-style cells (<c>th</c>) in the body and footer when <c>Bordered</c> is <c>true</c>.
    /// </summary>
    public required string BorderedHeaderCells { get; set; }
}
