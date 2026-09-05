// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace TwBlazor.Utilities;

public struct ClassBuilder
{
    private string classes;
    public ClassBuilder(string value) => classes = value;

    public ClassBuilder AddValue(string value)
    {
        classes += value;
        return this;
    }

    /// <summary>
    /// Adds a css class to the string.
    /// </summary>
    /// <param name="value">The css class to add.</param>
    /// <returns>ClassBuilder</returns>
    public ClassBuilder AddClass(string value) => AddValue(" " + value?.Trim());

    /// <summary>
    /// Adds a conditional css class to the string.
    /// </summary>
    /// <param name="value">The css class to add.</param>
    /// <param name="condition">Only add if condition is true.</param>
    /// <returns>ClassBuilder</returns>
    public ClassBuilder AddClass(string value, bool condition) => condition ? this.AddClass(value) : this;

    /// <summary>
    /// Builds the final string of css classes.
    /// </summary>
    /// <returns>The final constructed string of css classes.</returns>
    public string Build() => classes != null ? classes.Trim() : string.Empty;
}
