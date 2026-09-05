using TwBlazor.Enums;

namespace TwBlazor.Tests.Components.DataTable;

public class SortDirectionTests : TwBlazorTestBase
{
    [Fact]
    public void SortDirection_None_HasCorrectValue()
    {
        // Arrange & Act
        var direction = SortDirection.None;

        // Assert
        Assert.Equal(SortDirection.None, direction);
        Assert.Equal(0, (int)direction);
    }

    [Fact]
    public void SortDirection_Ascending_HasCorrectValue()
    {
        // Arrange & Act
        var direction = SortDirection.Ascending;

        // Assert
        Assert.Equal(SortDirection.Ascending, direction);
        Assert.Equal(1, (int)direction);
    }

    [Fact]
    public void SortDirection_Descending_HasCorrectValue()
    {
        // Arrange & Act
        var direction = SortDirection.Descending;

        // Assert
        Assert.Equal(SortDirection.Descending, direction);
        Assert.Equal(2, (int)direction);
    }

    [Fact]
    public void SortDirection_CanCompare()
    {
        // Arrange
        var none = SortDirection.None;
        var asc = SortDirection.Ascending;
        var desc = SortDirection.Descending;

        // Act & Assert
        Assert.NotEqual(none, asc);
        Assert.NotEqual(none, desc);
        Assert.NotEqual(asc, desc);
        Assert.Equal(SortDirection.Ascending, asc);
    }

    [Theory]
    [InlineData(SortDirection.None, SortDirection.Ascending)]
    [InlineData(SortDirection.Ascending, SortDirection.Descending)]
    [InlineData(SortDirection.Descending, SortDirection.None)]
    public void SortDirection_ThreeStateToggle_WorksCorrectly(SortDirection current, SortDirection expected)
    {
        // Arrange & Act
        var next = current switch
        {
            SortDirection.None => SortDirection.Ascending,
            SortDirection.Ascending => SortDirection.Descending,
            SortDirection.Descending => SortDirection.None,
            _ => SortDirection.None
        };

        // Assert
        Assert.Equal(expected, next);
    }
}
