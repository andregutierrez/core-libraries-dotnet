using Core.Libraries.Domain.ValueObjects.Periods;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Periods;

/// <summary>
/// Testes unitários para a classe <see cref="YearPeriod"/>.
/// </summary>
public class YearPeriodTests
{
    [Fact]
    public void Constructor_WithValidYear_ShouldCreateInstance()
    {
        // Act
        var period = new YearPeriod(2025);

        // Assert
        Assert.Equal(2025, period.Year);
        Assert.Equal(new DateOnly(2025, 1, 1), period.Start);
        Assert.Equal(new DateOnly(2025, 12, 31), period.End);
    }

    [Fact]
    public void Constructor_WithInvalidYear_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new YearPeriod(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new YearPeriod(10000));
    }

    [Fact]
    public void FromDate_ShouldCreateCorrectPeriod()
    {
        // Arrange
        var date = new DateOnly(2025, 6, 15);

        // Act
        var period = YearPeriod.FromDate(date);

        // Assert
        Assert.Equal(2025, period.Year);
        Assert.Equal(new DateOnly(2025, 1, 1), period.Start);
        Assert.Equal(new DateOnly(2025, 12, 31), period.End);
    }

    [Fact]
    public void Previous_ShouldReturnPreviousYear()
    {
        // Arrange
        var period = new YearPeriod(2025);

        // Act
        var previous = period.Previous();

        // Assert
        Assert.NotNull(previous);
        Assert.Equal(2024, ((YearPeriod)previous!).Year);
    }

    [Fact]
    public void Next_ShouldReturnNextYear()
    {
        // Arrange
        var period = new YearPeriod(2025);

        // Act
        var next = period.Next();

        // Assert
        Assert.NotNull(next);
        Assert.Equal(2026, ((YearPeriod)next!).Year);
    }

    [Fact]
    public void ToString_ShouldReturnYear()
    {
        // Arrange
        var period = new YearPeriod(2025);

        // Act
        var result = period.ToString();

        // Assert
        Assert.Equal("2025", result);
    }
}

