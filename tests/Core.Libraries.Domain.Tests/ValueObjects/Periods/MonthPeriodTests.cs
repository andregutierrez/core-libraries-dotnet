using Core.Libraries.Domain.ValueObjects.Periods;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Periods;

/// <summary>
/// Testes unitários para a classe <see cref="MonthPeriod"/>.
/// </summary>
public class MonthPeriodTests
{
    [Fact]
    public void Constructor_WithValidMonthAndYear_ShouldCreateInstance()
    {
        // Act
        var period = new MonthPeriod(6, 2025);

        // Assert
        Assert.Equal(6, period.Month);
        Assert.Equal(2025, period.Year);
        Assert.Equal(new DateOnly(2025, 6, 1), period.Start);
        Assert.Equal(new DateOnly(2025, 6, 30), period.End);
    }

    [Fact]
    public void Constructor_WithInvalidMonth_ShouldThrowException()
    {
        // Act & Assert
        // DateOnly lança ArgumentOutOfRangeException para valores inválidos
        Assert.ThrowsAny<Exception>(() => new MonthPeriod(0, 2025));
        Assert.ThrowsAny<Exception>(() => new MonthPeriod(13, 2025));
    }

    [Fact]
    public void Constructor_WithInvalidYear_ShouldThrowException()
    {
        // Act & Assert
        // DateOnly lança ArgumentOutOfRangeException para valores inválidos
        Assert.ThrowsAny<Exception>(() => new MonthPeriod(6, 0));
        Assert.ThrowsAny<Exception>(() => new MonthPeriod(6, 10000));
    }

    [Fact]
    public void FromDate_ShouldCreateCorrectPeriod()
    {
        // Arrange
        var date = new DateOnly(2025, 6, 15);

        // Act
        var period = MonthPeriod.FromDate(date);

        // Assert
        Assert.Equal(6, period.Month);
        Assert.Equal(2025, period.Year);
    }

    [Fact]
    public void Previous_FromJanuary_ShouldReturnDecemberOfPreviousYear()
    {
        // Arrange
        var period = new MonthPeriod(1, 2025);

        // Act
        var previous = period.Previous();

        // Assert
        Assert.NotNull(previous);
        var prevMonth = (MonthPeriod)previous!;
        Assert.Equal(12, prevMonth.Month);
        Assert.Equal(2024, prevMonth.Year);
    }

    [Fact]
    public void Previous_FromOtherMonth_ShouldReturnPreviousMonth()
    {
        // Arrange
        var period = new MonthPeriod(6, 2025);

        // Act
        var previous = period.Previous();

        // Assert
        Assert.NotNull(previous);
        var prevMonth = (MonthPeriod)previous!;
        Assert.Equal(5, prevMonth.Month);
        Assert.Equal(2025, prevMonth.Year);
    }

    [Fact]
    public void Next_FromDecember_ShouldReturnJanuaryOfNextYear()
    {
        // Arrange
        var period = new MonthPeriod(12, 2025);

        // Act
        var next = period.Next();

        // Assert
        Assert.NotNull(next);
        var nextMonth = (MonthPeriod)next!;
        Assert.Equal(1, nextMonth.Month);
        Assert.Equal(2026, nextMonth.Year);
    }

    [Fact]
    public void Next_FromOtherMonth_ShouldReturnNextMonth()
    {
        // Arrange
        var period = new MonthPeriod(6, 2025);

        // Act
        var next = period.Next();

        // Assert
        Assert.NotNull(next);
        var nextMonth = (MonthPeriod)next!;
        Assert.Equal(7, nextMonth.Month);
        Assert.Equal(2025, nextMonth.Year);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var period = new MonthPeriod(6, 2025);

        // Act
        var result = period.ToString();

        // Assert
        Assert.Equal("2025-06", result);
    }
}

