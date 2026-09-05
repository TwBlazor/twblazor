using TwBlazor.Models;

namespace TwBlazor.Tests.Models;

public class TwDialogResultTests
{
    [Fact]
    public void Ok_Parameterless_ReturnsUncanceledResultWithNullData()
    {
        // Act
        var result = TwDialogResult.Ok();

        // Assert
        Assert.False(result.Canceled);
        Assert.Null(result.Data);
        Assert.Equal(typeof(object), result.DataType);
    }

    [Fact]
    public void Ok_WithData_ReturnsUncanceledResultWithDataAndType()
    {
        // Act
        var result = TwDialogResult.Ok("hello");

        // Assert
        Assert.False(result.Canceled);
        Assert.Equal("hello", result.Data);
        Assert.Equal(typeof(string), result.DataType);
    }

    [Fact]
    public void Ok_WithDataAndDialogType_UsesProvidedDialogType()
    {
        // Act
        var result = TwDialogResult.Ok(42, typeof(TwDialogResultTests));

        // Assert
        Assert.False(result.Canceled);
        Assert.Equal(42, result.Data);
        Assert.Equal(typeof(TwDialogResultTests), result.DataType);
    }

    [Fact]
    public void Cancel_ReturnsCanceledResultWithNoData()
    {
        // Act
        var result = TwDialogResult.Cancel();

        // Assert
        Assert.True(result.Canceled);
        Assert.Null(result.Data);
        Assert.Null(result.DataType);
    }
}
