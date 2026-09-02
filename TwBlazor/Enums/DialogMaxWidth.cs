// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// Design and API shape inspired by MudBlazor's MaxWidth enum
// (https://github.com/MudBlazor/MudBlazor/blob/dev/src/MudBlazor/Enums/MaxWidth.cs), MIT License.

namespace TwBlazor.Enums;

/// <summary>
/// A maximum width breakpoint for a dialog shown via <see cref="Services.ITwDialogService"/>.
/// </summary>
public enum DialogMaxWidth
{
    /// <summary>
    /// No maximum width is applied.
    /// </summary>
    False,

    /// <summary>
    /// A small maximum width. This is the default.
    /// </summary>
    Small,

    /// <summary>
    /// A medium maximum width.
    /// </summary>
    Medium,

    /// <summary>
    /// A large maximum width.
    /// </summary>
    Large
}
