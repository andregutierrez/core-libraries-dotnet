using Core.Libraries.Domain.ValueObjects.Periods;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Periods;

/// <summary>
/// Testes unitários para a classe base <see cref="Period"/>.
/// </summary>
public class PeriodTests
{
    [Fact]
    public void Constructor_WithValidDates_ShouldCreateInstance()
    {
        // Arrange
        var start = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 12, 31);

        // Act
        var period = new CustomPeriod("Test", start, end);

        // Assert
        Assert.Equal(start, period.Start);
        Assert.Equal(end, period.End);
    }

    [Fact]
    public void Constructor_WithEndBeforeStart_ShouldThrowArgumentException()
    {
        // Arrange
        var start = new DateOnly(2025, 12, 31);
        var end = new DateOnly(2025, 1, 1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CustomPeriod("Test", start, end));
    }

    [Fact]
    public void Contains_WithDateInRange_ShouldReturnTrue()
    {
        // Arrange
        var period = new CustomPeriod("Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31));
        var date = new DateOnly(2025, 6, 15);

        // Act
        var result = period.Contains(date);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithDateEqualToStart_ShouldReturnTrue()
    {
        // Arrange
        var start = new DateOnly(2025, 1, 1);
        var period = new CustomPeriod("Test", start, new DateOnly(2025, 12, 31));

        // Act
        var result = period.Contains(start);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithDateEqualToEnd_ShouldReturnTrue()
    {
        // Arrange
        var end = new DateOnly(2025, 12, 31);
        var period = new CustomPeriod("Test", new DateOnly(2025, 1, 1), end);

        // Act
        var result = period.Contains(end);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithDateBeforeStart_ShouldReturnFalse()
    {
        // Arrange
        var period = new CustomPeriod("Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31));
        var date = new DateOnly(2024, 12, 31);

        // Act
        var result = period.Contains(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_WithDateAfterEnd_ShouldReturnFalse()
    {
        // Arrange
        var period = new CustomPeriod("Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31));
        var date = new DateOnly(2026, 1, 1);

        // Act
        var result = period.Contains(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Days_ShouldReturnCorrectNumberOfDays()
    {
        // Arrange
        var period = new CustomPeriod("Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));

        // Act
        var days = period.Days;

        // Assert
        Assert.Equal(31, days);
    }

    [Fact]
    public void OverlapsWith_WithOverlappingPeriod_ShouldReturnTrue()
    {
        // Arrange
        var period1 = new CustomPeriod("Test1", new DateOnly(2025, 1, 1), new DateOnly(2025, 6, 30));
        var period2 = new CustomPeriod("Test2", new DateOnly(2025, 4, 1), new DateOnly(2025, 9, 30));

        // Act
        var result = period1.OverlapsWith(period2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OverlapsWith_WithNonOverlappingPeriod_ShouldReturnFalse()
    {
        // Arrange
        var period1 = new CustomPeriod("Test1", new DateOnly(2025, 1, 1), new DateOnly(2025, 3, 31));
        var period2 = new CustomPeriod("Test2", new DateOnly(2025, 4, 1), new DateOnly(2025, 6, 30));

        // Act
        var result = period1.OverlapsWith(period2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OverlapsWith_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var period = new CustomPeriod("Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31));

        // Act
        var result = period.OverlapsWith(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsConsecutiveWith_WithConsecutivePeriod_ShouldReturnTrue()
    {
        // Arrange
        var period1 = new CustomPeriod("Test1", new DateOnly(2025, 1, 1), new DateOnly(2025, 3, 31));
        var period2 = new CustomPeriod("Test2", new DateOnly(2025, 4, 1), new DateOnly(2025, 6, 30));

        // Act
        var result = period1.IsConsecutiveWith(period2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsConsecutiveWith_WithNonConsecutivePeriod_ShouldReturnFalse()
    {
        // Arrange
        var period1 = new CustomPeriod("Test1", new DateOnly(2025, 1, 1), new DateOnly(2025, 3, 31));
        var period2 = new CustomPeriod("Test2", new DateOnly(2025, 4, 2), new DateOnly(2025, 6, 30));

        // Act
        var result = period1.IsConsecutiveWith(period2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsConsecutiveWith_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var period = new CustomPeriod("Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31));

        // Act
        var result = period.IsConsecutiveWith(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CompareTo_WithEarlierStart_ShouldReturnPositive()
    {
        // Arrange
        var period1 = new CustomPeriod("Test1", new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));
        var period2 = new CustomPeriod("Test2", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));

        // Act
        var result = period1.CompareTo(period2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLaterStart_ShouldReturnNegative()
    {
        // Arrange
        var period1 = new CustomPeriod("Test1", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));
        var period2 = new CustomPeriod("Test2", new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));

        // Act
        var result = period1.CompareTo(period2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithSameStart_ShouldReturnZero()
    {
        // Arrange
        var start = new DateOnly(2025, 1, 1);
        var period1 = new CustomPeriod("Test1", start, new DateOnly(2025, 1, 31));
        var period2 = new CustomPeriod("Test2", start, new DateOnly(2025, 6, 30));

        // Act
        var result = period1.CompareTo(period2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void OperatorLessThan_ShouldWorkCorrectly()
    {
        // Arrange
        var period1 = new CustomPeriod("Test1", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));
        var period2 = new CustomPeriod("Test2", new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));

        // Act & Assert
        Assert.True(period1 < period2);
        Assert.False(period2 < period1);
    }

    [Fact]
    public void OperatorGreaterThan_ShouldWorkCorrectly()
    {
        // Arrange
        var period1 = new CustomPeriod("Test1", new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));
        var period2 = new CustomPeriod("Test2", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));

        // Act & Assert
        Assert.True(period1 > period2);
        Assert.False(period2 > period1);
    }
}

