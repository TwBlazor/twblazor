using TwBlazor.Models;

namespace TwBlazor.Tests.Models;

public class TwDialogParametersTests
{
    [Fact]
    public void Indexer_SetsAndGetsValue()
    {
        // Arrange
        var parameters = new TwDialogParameters
        {
            // Act
            ["Message"] = "Hello"
        };

        // Assert
        Assert.Equal("Hello", parameters["Message"]);
    }

    [Fact]
    public void CollectionInitializer_AddsValues()
    {
        // Arrange & Act
        var parameters = new TwDialogParameters
        {
            ["Message"] = "Hello",
            ["Count"] = 5
        };

        // Assert
        Assert.Equal(2, parameters.Count);
        Assert.Equal("Hello", parameters["Message"]);
        Assert.Equal(5, parameters["Count"]);
    }

    [Fact]
    public void Add_ThrowsArgumentException_WhenParameterNameIsEmpty()
    {
        // Arrange
        TwDialogParameters parameters = [];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => parameters.Add(string.Empty, "value"));
    }

    [Fact]
    public void Add_OverwritesExistingValue()
    {
        // Arrange
        var parameters = new TwDialogParameters { ["Message"] = "First" };

        // Act
        parameters.Add("Message", "Second");

        // Assert
        Assert.Equal("Second", parameters["Message"]);
        Assert.Equal(1, parameters.Count);
    }

    [Fact]
    public void Add_IsCaseInsensitive()
    {
        // Arrange
        var parameters = new TwDialogParameters { ["Message"] = "First" };

        // Act
        parameters.Add("message", "Second");

        // Assert
        Assert.Equal(1, parameters.Count);
        Assert.Equal("Second", parameters["Message"]);
    }

    [Fact]
    public void Get_ReturnsTypedValue_WhenPresent()
    {
        // Arrange
        var parameters = new TwDialogParameters { ["Count"] = 42 };

        // Act
        var value = parameters.Get<int>("Count");

        // Assert
        Assert.Equal(42, value);
    }

    [Fact]
    public void Get_ReturnsDefault_WhenNotPresent()
    {
        // Arrange
        TwDialogParameters parameters = [];

        // Act
        var value = parameters.Get<int>("Missing");

        // Assert
        Assert.Equal(0, value);
    }

    [Fact]
    public void Get_ReturnsDefault_WhenTypeMismatch()
    {
        // Arrange
        var parameters = new TwDialogParameters { ["Message"] = "Hello" };

        // Act
        var value = parameters.Get<int>("Message");

        // Assert
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryGet_ReturnsTrueAndValue_WhenPresentAndAssignable()
    {
        // Arrange
        var parameters = new TwDialogParameters { ["Message"] = "Hello" };

        // Act
        var found = parameters.TryGet<string>("Message", out var value);

        // Assert
        Assert.True(found);
        Assert.Equal("Hello", value);
    }

    [Fact]
    public void TryGet_ReturnsFalse_WhenNotPresent()
    {
        // Arrange
        TwDialogParameters parameters = [];

        // Act
        var found = parameters.TryGet<string>("Missing", out var value);

        // Assert
        Assert.False(found);
        Assert.Null(value);
    }

    [Fact]
    public void TryGet_ReturnsFalse_WhenTypeMismatch()
    {
        // Arrange
        var parameters = new TwDialogParameters { ["Count"] = 42 };

        // Act
        var found = parameters.TryGet<string>("Count", out var value);

        // Assert
        Assert.False(found);
        Assert.Null(value);
    }

    [Fact]
    public void Enumeration_YieldsAllAddedParameters()
    {
        // Arrange
        var parameters = new TwDialogParameters
        {
            ["A"] = 1,
            ["B"] = 2
        };

        // Act
        var keys = parameters.Select(p => p.Key).OrderBy(k => k).ToList();

        // Assert
        Assert.Equal(["A", "B"], keys);
    }

    [Fact]
    public void Count_ReflectsNumberOfParameters()
    {
        // Arrange
        TwDialogParameters parameters = [];

        // Assert
        Assert.Equal(0, parameters.Count);

        // Act
        parameters.Add("A", 1);
        parameters.Add("B", 2);

        // Assert
        Assert.Equal(2, parameters.Count);
    }

    [Fact]
    public void Default_IsEmpty()
    {
        // Act & Assert
        Assert.Equal(0, TwDialogParameters._default.Count);
    }
}
