// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using TwBlazor.Utilities;

namespace TwBlazor.A11yTests;

public class ContrastCalculatorTests
{
    #region RelativeLuminance Tests

    [Fact]
    public void RelativeLuminance_Black_IsZero()
    {
        Assert.Equal(0.0, ContrastCalculator.RelativeLuminance(0, 0, 0), precision: 5);
    }

    [Fact]
    public void RelativeLuminance_White_IsOne()
    {
        Assert.Equal(1.0, ContrastCalculator.RelativeLuminance(255, 255, 255), precision: 5);
    }

    [Fact]
    public void RelativeLuminance_IsMonotonic_AsChannelsBrighten()
    {
        var darker = ContrastCalculator.RelativeLuminance(50, 50, 50);
        var lighter = ContrastCalculator.RelativeLuminance(150, 150, 150);

        Assert.True(lighter > darker);
    }

    #endregion

    #region ContrastRatio Tests

    [Fact]
    public void ContrastRatio_BlackAgainstWhite_IsMaximum()
    {
        // WCAG's defined maximum, black against white.
        var result = ContrastCalculator.ContrastRatio(0, 0, 0, 255, 255, 255);

        Assert.Equal(21.0, result, precision: 2);
    }

    [Fact]
    public void ContrastRatio_IdenticalColors_IsOne()
    {
        var result = ContrastCalculator.ContrastRatio(100, 100, 100, 100, 100, 100);

        Assert.Equal(1.0, result, precision: 5);
    }

    [Fact]
    public void ContrastRatio_IsOrderIndependent()
    {
        var forward = ContrastCalculator.ContrastRatio(0, 0, 0, 255, 255, 255);
        var reversed = ContrastCalculator.ContrastRatio(255, 255, 255, 0, 0, 0);

        Assert.Equal(forward, reversed, precision: 10);
    }

    [Fact]
    public void ContrastRatio_HexOverload_MatchesRgbOverload()
    {
        var hexResult = ContrastCalculator.ContrastRatio("#9810fa", "#1d232a");
        var rgbResult = ContrastCalculator.ContrastRatio(0x98, 0x10, 0xfa, 0x1d, 0x23, 0x2a);

        Assert.Equal(rgbResult, hexResult, precision: 10);
    }

    [Fact]
    public void ContrastRatio_HexOverload_IgnoresAlphaChannel()
    {
        var withAlpha = ContrastCalculator.ContrastRatio("#9810faAA", "#1d232a");
        var withoutAlpha = ContrastCalculator.ContrastRatio("#9810fa", "#1d232a");

        Assert.Equal(withoutAlpha, withAlpha, precision: 10);
    }

    #endregion

    #region FindMinimumPassingColor Tests

    [Fact]
    public void FindMinimumPassingColor_ReturnsColor_ThatActuallyPasses()
    {
        // The reproduced axe failure from the /tabs dark-mode a11y scan: purple text on a dark
        // background falling short of the 4.5:1 AA threshold for normal-size text.
        var result = ContrastCalculator.FindMinimumPassingColor("#9810fa", "#1d232a", 4.5);

        Assert.NotNull(result);
        var actualRatio = ContrastCalculator.ContrastRatio(result.Value.Hex, "#1d232a");
        Assert.True(actualRatio >= 4.5, $"Suggested color {result.Value.Hex} only reaches {actualRatio:F2}:1");
    }

    [Fact]
    public void FindMinimumPassingColor_Lightens_WhenForegroundAlreadyLighterThanBackground()
    {
        // #9810fa is lighter than the near-black #1d232a background, so the fix should brighten it
        // further rather than flipping it to become darker than the background.
        var result = ContrastCalculator.FindMinimumPassingColor("#9810fa", "#1d232a", 4.5);

        Assert.NotNull(result);
        Assert.True(result.Value.LightnessPercent > 50, "Expected the suggested color to be lightened, not darkened.");
    }

    [Fact]
    public void FindMinimumPassingColor_Darkens_WhenForegroundAlreadyDarkerThanBackground()
    {
        // A dark foreground on a light background should get darker, not flip to become the lighter
        // of the two.
        var result = ContrastCalculator.FindMinimumPassingColor("#777777", "#f5f5f5", 4.5);

        Assert.NotNull(result);
        var (_, _, originalLightness) = ColorConverter.RgbToHsl(0x77, 0x77, 0x77);
        Assert.True(result.Value.LightnessPercent < originalLightness * 100, "Expected the suggested color to be darkened, not lightened.");
    }

    [Fact]
    public void FindMinimumPassingColor_ReturnsNull_WhenRatioIsUnreachable()
    {
        // Middle gray against middle gray can never reach 21:1 no matter how it's lightened or
        // darkened along the same hue/saturation, since one extreme is white (contrast well under 21)
        // and this asks for a ratio no real pair of colors besides pure black/white can hit.
        var result = ContrastCalculator.FindMinimumPassingColor("#808080", "#808080", 21.0);

        Assert.Null(result);
    }

    [Fact]
    public void FindMinimumPassingColor_AlreadyPassing_ReturnsCloseToOriginal()
    {
        // Black on white already comfortably exceeds a modest requirement - the binary search should
        // converge back near the original lightness rather than needlessly pushing to an extreme.
        var result = ContrastCalculator.FindMinimumPassingColor("#000000", "#ffffff", 4.5);

        Assert.NotNull(result);
        Assert.True(result.Value.LightnessPercent < 5, $"Expected minimal movement from black, got {result.Value.LightnessPercent}% lightness.");
    }

    #endregion
}
