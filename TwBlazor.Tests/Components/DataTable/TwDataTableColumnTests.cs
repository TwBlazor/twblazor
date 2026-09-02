using TwBlazor.Components;

namespace TwBlazor.Tests.Components.DataTable;

public class TwDataTableColumnTests : TwBlazorTestBase
{
    [Fact]
    public void TwDataTableColumn_SetsName_Correctly()
    {
        // Arrange & Act
        var column = new TwDataTableColumn<TestItem>
        {
            Name = "testColumn"
        };

        // Assert
        Assert.Equal("testColumn", column.Name);
    }

    [Fact]
    public void TwDataTableColumn_SetsTitle_Correctly()
    {
        // Arrange & Act
        var column = new TwDataTableColumn<TestItem>
        {
            Title = "Test Title"
        };

        // Assert
        Assert.Equal("Test Title", column.Title);
    }

    [Fact]
    public void TwDataTableColumn_SetsPropertySelector_Correctly()
    {
        // Arrange
        var testItem = new TestItem { Name = "John", Age = 30 };
        var column = new TwDataTableColumn<TestItem>
        {
            PropertySelector = item => item.Name
        };

        // Act
        var result = column.PropertySelector?.Invoke(testItem);

        // Assert
        Assert.NotNull(column.PropertySelector);
        Assert.Equal("John", result);
    }

    [Fact]
    public void TwDataTableColumn_IsSortable_DefaultsToFalse()
    {
        // Arrange & Act
        var column = new TwDataTableColumn<TestItem>();

        // Assert
        Assert.False(column.IsSortable);
    }

    [Fact]
    public void TwDataTableColumn_IsSortable_CanBeSetToTrue()
    {
        // Arrange & Act
        var column = new TwDataTableColumn<TestItem>
        {
            IsSortable = true
        };

        // Assert
        Assert.True(column.IsSortable);
    }

    [Fact]
    public void TwDataTableColumn_HeaderClass_CanBeSet()
    {
        // Arrange & Act
        var column = new TwDataTableColumn<TestItem>
        {
            HeaderClass = "bg-blue-500"
        };

        // Assert
        Assert.Equal("bg-blue-500", column.HeaderClass);
    }

    [Fact]
    public void TwDataTableColumn_CellClass_CanBeSet()
    {
        // Arrange & Act
        var column = new TwDataTableColumn<TestItem>
        {
            CellClass = "font-bold text-lg"
        };

        // Assert
        Assert.Equal("font-bold text-lg", column.CellClass);
    }

    [Fact]
    public void TwDataTableColumn_CellFormatter_AppliesCustomFormatting()
    {
        // Arrange
        var testItem = new TestItem { Name = "John", Age = 30 };
        var column = new TwDataTableColumn<TestItem>
        {
            CellFormatter = item => $"Age: {item.Age}"
        };

        // Act
        var result = column.CellFormatter?.Invoke(testItem);

        // Assert
        Assert.Equal("Age: 30", result);
    }

    [Fact]
    public void TwDataTableColumn_CanCombineMultipleProperties()
    {
        // Arrange
        var testItem = new TestItem { Name = "John", Age = 30 };
        var column = new TwDataTableColumn<TestItem>
        {
            Name = "fullInfo",
            Title = "Full Information",
            PropertySelector = item => item.Name,
            IsSortable = true,
            HeaderClass = "bg-gray-50",
            CellClass = "font-medium",
            CellFormatter = item => $"{item.Name} ({item.Age})"
        };

        // Act & Assert
        Assert.Equal("fullInfo", column.Name);
        Assert.Equal("Full Information", column.Title);
        Assert.True(column.IsSortable);
        Assert.Equal("bg-gray-50", column.HeaderClass);
        Assert.Equal("font-medium", column.CellClass);
        Assert.Equal("John (30)", column.CellFormatter?.Invoke(testItem));
    }

    private class TestItem
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
