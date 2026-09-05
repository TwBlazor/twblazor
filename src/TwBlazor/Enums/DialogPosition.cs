// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// Design and API shape inspired by MudBlazor's DialogPosition enum
// (https://github.com/MudBlazor/MudBlazor/blob/dev/src/MudBlazor/Enums/DialogPosition.cs), MIT License.

namespace TwBlazor.Enums;

/// <summary>
/// The on-screen position of a dialog shown via <see cref="Services.ITwDialogService"/>.
/// </summary>
public enum DialogPosition
{
    /// <summary>
    /// The dialog appears in the center of the screen.
    /// </summary>
    Center,

    /// <summary>
    /// The dialog appears vertically centered on the left side of the screen.
    /// </summary>
    CenterLeft,

    /// <summary>
    /// The dialog appears vertically centered on the right side of the screen.
    /// </summary>
    CenterRight,

    /// <summary>
    /// The dialog appears at the top of the screen, horizontally centered.
    /// </summary>
    TopCenter,

    /// <summary>
    /// The dialog appears in the upper-left corner of the screen.
    /// </summary>
    TopLeft,

    /// <summary>
    /// The dialog appears in the upper-right corner of the screen.
    /// </summary>
    TopRight,

    /// <summary>
    /// The dialog appears at the bottom of the screen, horizontally centered.
    /// </summary>
    BottomCenter,

    /// <summary>
    /// The dialog appears in the lower-left corner of the screen.
    /// </summary>
    BottomLeft,

    /// <summary>
    /// The dialog appears in the lower-right corner of the screen.
    /// </summary>
    BottomRight
}
