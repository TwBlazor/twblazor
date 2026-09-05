// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace TwBlazor.Configuration.Color;

/// <summary>
/// Neutral (gray-scale, non-semantic) text tokens shared across components for body/heading text and
/// its muted variants. Unlike <see cref="TwTextColor"/>/<see cref="TwBlazorPalette"/> (a distinct color
/// per semantic role - primary, danger, etc.), every property here is a single reusable class string
/// already covering both light and dark mode, sized by prominence so components that just need "the"
/// default text color, or one of its quieter variants, reference one of these instead of retyping the
/// same literal classes (and occasionally drifting to a slightly different gray shade).
/// </summary>
/// <remarks>
/// Add more weights here as new components need them. Each stays a single string property so the full
/// class names remain visible to Tailwind's content scanner, which can't see classes assembled only at
/// runtime from a compiled DLL.
/// </remarks>
[ExcludeFromCodeCoverage]
public class TwNeutralTextPalette
{
    /// <summary>
    /// Gets or sets the default body/heading text color - the highest-contrast neutral text
    /// (e.g. <c>text-gray-950 dark:text-white</c>).
    /// </summary>
    public string Heading { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a secondary text color, one step quieter than <see cref="Heading"/> - used for
    /// legends and supporting copy (e.g. <c>text-gray-700 dark:text-gray-300</c>).
    /// </summary>
    public string Secondary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a muted text color for form labels and captions
    /// (e.g. <c>text-gray-600 dark:text-gray-400</c>).
    /// </summary>
    public string Muted { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quietest text color, for icons, separators, and other decorative or
    /// low-emphasis text (e.g. <c>text-gray-500 dark:text-gray-400</c>).
    /// </summary>
    public string Subtle { get; set; } = string.Empty;
}
