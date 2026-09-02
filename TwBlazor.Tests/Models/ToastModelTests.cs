using TwBlazor.Enums;
using TwBlazor.Models;

namespace TwBlazor.Tests.Models;

public class ToastModelTests
{
    [Fact]
    public void ToastModel_HasDefaultValues()
    {
        // Act
        var toast = new ToastModel();

        // Assert
        Assert.NotEqual(Guid.Empty, toast.Id);
        Assert.Equal(string.Empty, toast.Title);
        Assert.Equal(string.Empty, toast.Message);
        Assert.Null(toast.Color);
        Assert.Null(toast.Icon);
        Assert.Equal(5000, toast.Duration);
        Assert.False(toast.IsBurnt);
    }

    [Fact]
    public void ToastModel_GeneratesUniqueIds()
    {
        // Act
        var toast1 = new ToastModel();
        var toast2 = new ToastModel();

        // Assert
        Assert.NotEqual(toast1.Id, toast2.Id);
    }

    [Fact]
    public void ToastModel_IsBurnt_ReturnsFalse_WhenDurationNotExceeded()
    {
        // Arrange
        var toast = new ToastModel
        {
            Duration = 5000,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.False(toast.IsBurnt);
    }

    [Fact]
    public void ToastModel_IsBurnt_ReturnsTrue_WhenDurationExceeded()
    {
        // Arrange
        var toast = new ToastModel
        {
            Duration = 100,
            CreatedAt = DateTime.UtcNow.AddSeconds(-1)
        };

        // Act & Assert
        Assert.True(toast.IsBurnt);
    }

    [Fact]
    public void ToastModel_IsBurnt_ReturnsFalse_WhenDurationIsZero()
    {
        // Arrange
        var toast = new ToastModel
        {
            Duration = 0,
            CreatedAt = DateTime.UtcNow.AddHours(-1)
        };

        // Act & Assert
        Assert.False(toast.IsBurnt);
    }

    [Theory]
    [InlineData(5, "just now")]
    [InlineData(30, "just now")]
    [InlineData(59, "just now")]
    public void ToastModel_ElapsedTimeText_ReturnsJustNow_WhenUnderOneMinute(int secondsAgo, string expected)
    {
        // Arrange
        var toast = new ToastModel
        {
            CreatedAt = DateTime.UtcNow.AddSeconds(-secondsAgo)
        };

        // Act
        var result = toast.ElapsedTimeText;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(60, "1m ago")]
    [InlineData(120, "2m ago")]
    [InlineData(3540, "59m ago")]
    public void ToastModel_ElapsedTimeText_ReturnsMinutes_WhenUnderOneHour(int secondsAgo, string expected)
    {
        // Arrange
        var toast = new ToastModel
        {
            CreatedAt = DateTime.UtcNow.AddSeconds(-secondsAgo)
        };

        // Act
        var result = toast.ElapsedTimeText;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(3600, "1h ago")]
    [InlineData(7200, "2h ago")]
    [InlineData(82800, "23h ago")]
    public void ToastModel_ElapsedTimeText_ReturnsHours_WhenUnderOneDay(int secondsAgo, string expected)
    {
        // Arrange
        var toast = new ToastModel
        {
            CreatedAt = DateTime.UtcNow.AddSeconds(-secondsAgo)
        };

        // Act
        var result = toast.ElapsedTimeText;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(86400, "1d ago")]
    [InlineData(172800, "2d ago")]
    [InlineData(604800, "7d ago")]
    public void ToastModel_ElapsedTimeText_ReturnsDays_WhenOverOneDay(int secondsAgo, string expected)
    {
        // Arrange
        var toast = new ToastModel
        {
            CreatedAt = DateTime.UtcNow.AddSeconds(-secondsAgo)
        };

        // Act
        var result = toast.ElapsedTimeText;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToastModel_CanSetAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        // Act
        var toast = new ToastModel
        {
            Id = id,
            Title = "Test Title",
            Message = "Test Message",
            Color = Color.Primary,
            Icon = Icon.Info_Circle,
            Duration = 3000,
            CreatedAt = createdAt
        };

        // Assert
        Assert.Equal(id, toast.Id);
        Assert.Equal("Test Title", toast.Title);
        Assert.Equal("Test Message", toast.Message);
        Assert.Equal(Color.Primary, toast.Color);
        Assert.Equal(Icon.Info_Circle, toast.Icon);
        Assert.Equal(3000, toast.Duration);
        Assert.Equal(createdAt, toast.CreatedAt);
    }

    [Fact]
    public void ToastModel_IsHoveredAndIsFocused_DefaultToFalse()
    {
        // Act
        var toast = new ToastModel();

        // Assert
        Assert.False(toast.IsHovered);
        Assert.False(toast.IsFocused);
        Assert.False(toast.IsPaused);
    }

    [Fact]
    public void PauseForHover_SetsIsHovered_AndIsPaused()
    {
        // Arrange
        var toast = new ToastModel();

        // Act
        toast.PauseForHover();

        // Assert
        Assert.True(toast.IsHovered);
        Assert.True(toast.IsPaused);
    }

    [Fact]
    public void ResumeForHover_ClearsIsHovered_AndIsPaused_WhenNotFocused()
    {
        // Arrange
        var toast = new ToastModel();
        toast.PauseForHover();

        // Act
        toast.ResumeForHover();

        // Assert
        Assert.False(toast.IsHovered);
        Assert.False(toast.IsPaused);
    }

    [Fact]
    public void ResumeForHover_KeepsIsPaused_WhenStillFocused()
    {
        // Arrange - hover and focus overlap, e.g. tabbing onto the close button while the pointer
        // is still over the toast.
        var toast = new ToastModel();
        toast.PauseForHover();
        toast.PauseForFocus();

        // Act
        toast.ResumeForHover();

        // Assert
        Assert.False(toast.IsHovered);
        Assert.True(toast.IsFocused);
        Assert.True(toast.IsPaused);
    }

    [Fact]
    public void PauseForFocus_SetsIsFocused_AndIsPaused()
    {
        // Arrange
        var toast = new ToastModel();

        // Act
        toast.PauseForFocus();

        // Assert
        Assert.True(toast.IsFocused);
        Assert.True(toast.IsPaused);
    }

    [Fact]
    public void ResumeForFocus_ClearsIsFocused_AndIsPaused_WhenNotHovered()
    {
        // Arrange
        var toast = new ToastModel();
        toast.PauseForFocus();

        // Act
        toast.ResumeForFocus();

        // Assert
        Assert.False(toast.IsFocused);
        Assert.False(toast.IsPaused);
    }

    [Fact]
    public void ResumeForFocus_KeepsIsPaused_WhenStillHovered()
    {
        // Arrange
        var toast = new ToastModel();
        toast.PauseForFocus();
        toast.PauseForHover();

        // Act
        toast.ResumeForFocus();

        // Assert
        Assert.False(toast.IsFocused);
        Assert.True(toast.IsHovered);
        Assert.True(toast.IsPaused);
    }

    [Fact]
    public void IsBurnt_IsAlwaysFalse_WhilePaused_EvenIfDurationExceeded()
    {
        // Arrange
        var toast = new ToastModel
        {
            Duration = 100,
            CreatedAt = DateTime.UtcNow.AddSeconds(-1)
        };

        // Act
        toast.PauseForHover();

        // Assert
        Assert.True(toast.IsPaused);
        Assert.False(toast.IsBurnt);
    }

    [Fact]
    public async Task IsBurnt_ExcludesTimeSpentPaused_FromElapsedDuration()
    {
        // Arrange - a toast with a short duration that spends most of its life paused should not
        // be burnt until it has accumulated `Duration` worth of *unpaused* time.
        var toast = new ToastModel { Duration = 150 };

        toast.PauseForHover();
        await Task.Delay(200, TestContext.Current.CancellationToken); // time passes entirely while paused

        toast.ResumeForHover();

        // Assert - almost no unpaused time has elapsed yet, so it should not be burnt.
        Assert.False(toast.IsBurnt);

        // Act - let enough *unpaused* time elapse to exceed Duration.
        await Task.Delay(200, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(toast.IsBurnt);
    }
}
