using System.ComponentModel;
using TwBlazor.Extensions;

namespace TwBlazor.Tests.Extensions;

public class EnumExtensionsTests
{
    private enum TestEnum
    {
        [Description("First Value")]
        FirstValue,

        [Description("Second Value")]
        SecondValue,

        ValueWithoutDescription,

        [Description("")]
        ValueWithEmptyDescription
    }

    private enum EmptyEnum
    {
    }

    #region GetDescriptionFromName Tests

    [Fact]
    public void GetDescriptionFromName_WithDescription_ReturnsDescription()
    {
        // Arrange
        var enumValue = TestEnum.FirstValue;
        var expectedDescription = "First Value";

        // Act
        var result = EnumExtensions.GetDescriptionFromName(enumValue);

        // Assert
        Assert.Equal(expectedDescription, result);
    }

    [Fact]
    public void GetDescriptionFromName_WithoutDescription_ReturnsEnumName()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithoutDescription;
        var expectedName = "ValueWithoutDescription";

        // Act
        var result = EnumExtensions.GetDescriptionFromName(enumValue);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetDescriptionFromName_WithEmptyDescription_ReturnsEmptyString()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithEmptyDescription;
        var expectedDescription = "";

        // Act
        var result = EnumExtensions.GetDescriptionFromName(enumValue);

        // Assert
        Assert.Equal(expectedDescription, result);
    }

    [Fact]
    public void GetDescriptionFromName_WithMultipleValues_ReturnsCorrectDescriptions()
    {
        // Arrange & Act & Assert
        Assert.Equal("First Value", EnumExtensions.GetDescriptionFromName(TestEnum.FirstValue));
        Assert.Equal("Second Value", EnumExtensions.GetDescriptionFromName(TestEnum.SecondValue));
    }

    [Fact]
    public void GetDescriptionFromName_WithUndefinedEnumValue_ReturnsNumericString()
    {
        // Arrange - a value with no matching named field (GetField returns null),
        // e.g. an int cast to the enum type that doesn't correspond to any member.
        var enumValue = (TestEnum)999;

        // Act
        var result = EnumExtensions.GetDescriptionFromName(enumValue);

        // Assert
        Assert.Equal("999", result);
    }

    #endregion

    #region GetNameFromDescription Tests

    [Fact]
    public void GetNameFromDescription_WithMatchingDescription_ReturnsEnumValue()
    {
        // Arrange
        var description = "First Value";
        var expectedValue = TestEnum.FirstValue;

        // Act
        var result = EnumExtensions.GetNameFromDescription<TestEnum>(description);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedValue, result!.Value);
    }

    [Fact]
    public void GetNameFromDescription_WithNonMatchingDescription_ReturnsNull()
    {
        // Arrange
        var description = "Non Existent Value";

        // Act
        var result = EnumExtensions.GetNameFromDescription<TestEnum>(description);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetNameFromDescription_WithEmptyDescription_ReturnsMatchingValue()
    {
        // Arrange
        var description = "";

        // Act
        var result = EnumExtensions.GetNameFromDescription<TestEnum>(description);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestEnum.ValueWithEmptyDescription, result!.Value);
    }

    [Fact]
    public void GetNameFromDescription_WithMultipleDescriptions_ReturnsCorrectValues()
    {
        // Arrange & Act & Assert
        var firstResult = EnumExtensions.GetNameFromDescription<TestEnum>("First Value");
        var secondResult = EnumExtensions.GetNameFromDescription<TestEnum>("Second Value");

        Assert.NotNull(firstResult);
        Assert.NotNull(secondResult);
        Assert.Equal(TestEnum.FirstValue, firstResult!.Value);
        Assert.Equal(TestEnum.SecondValue, secondResult!.Value);
    }

    [Fact]
    public void GetNameFromDescription_CaseSensitive_ReturnsNull()
    {
        // Arrange
        var description = "first value"; // lowercase

        // Act
        var result = EnumExtensions.GetNameFromDescription<TestEnum>(description);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region Round Trip Tests

    [Fact]
    public void RoundTrip_GetDescriptionThenGetName_ReturnsOriginalValue()
    {
        // Arrange
        var originalValue = TestEnum.SecondValue;

        // Act
        var description = EnumExtensions.GetDescriptionFromName(originalValue);
        var result = EnumExtensions.GetNameFromDescription<TestEnum>(description);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(originalValue, result!.Value);
    }

    [Fact]
    public void RoundTrip_AllValuesWithDescriptions_ReturnsOriginalValues()
    {
        // Arrange
        var valuesWithDescriptions = new[] { TestEnum.FirstValue, TestEnum.SecondValue };

        foreach (var originalValue in valuesWithDescriptions)
        {
            // Act
            var description = EnumExtensions.GetDescriptionFromName(originalValue);
            var result = EnumExtensions.GetNameFromDescription<TestEnum>(description);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(originalValue, result!.Value);
        }
    }

    #endregion
}
