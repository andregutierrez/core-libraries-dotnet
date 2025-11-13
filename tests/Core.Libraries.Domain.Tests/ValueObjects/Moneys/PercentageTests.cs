using Core.Libraries.Domain.ValueObjects.Moneys;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Moneys;

/// <summary>
/// Testes unitários para a classe <see cref="Percentage"/>.
/// </summary>
public class PercentageTests
{
    [Fact]
    public void Constructor_WithValidValue_ShouldCreateInstance()
    {
        // Arrange
        var value = 0.25m;

        // Act
        var percentage = new Percentage(value);

        // Assert
        Assert.Equal(0.25m, percentage.Value);
    }

    [Fact]
    public void Constructor_WithZero_ShouldCreateInstance()
    {
        // Act
        var percentage = new Percentage(0m);

        // Assert
        Assert.Equal(0m, percentage.Value);
    }

    [Fact]
    public void Constructor_WithOne_ShouldCreateInstance()
    {
        // Act
        var percentage = new Percentage(1m);

        // Assert
        Assert.Equal(1m, percentage.Value);
    }

    [Fact]
    public void Constructor_WithNegativeValue_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Percentage(-0.1m));
    }

    [Fact]
    public void Constructor_WithValueGreaterThanOne_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Percentage(1.1m));
    }

    [Fact]
    public void ApplyTo_ShouldReturnCalculatedAmount()
    {
        // Arrange
        var percentage = new Percentage(0.25m); // 25%
        var currency = new Currency("USD");
        var money = new Money(100m, currency);

        // Act
        var result = percentage.ApplyTo(money);

        // Assert
        Assert.Equal(25m, result.Amount);
        Assert.Equal(currency, result.Currency);
    }

    [Fact]
    public void ApplyTo_WithZeroPercentage_ShouldReturnZero()
    {
        // Arrange
        var percentage = new Percentage(0m);
        var currency = new Currency("USD");
        var money = new Money(100m, currency);

        // Act
        var result = percentage.ApplyTo(money);

        // Assert
        Assert.Equal(0m, result.Amount);
    }

    [Fact]
    public void ApplyTo_WithOneHundredPercent_ShouldReturnFullAmount()
    {
        // Arrange
        var percentage = new Percentage(1m); // 100%
        var currency = new Currency("USD");
        var money = new Money(100m, currency);

        // Act
        var result = percentage.ApplyTo(money);

        // Assert
        Assert.Equal(100m, result.Amount);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var percentage = new Percentage(0.25m);

        // Act
        var result = percentage.ToString();

        // Assert
        Assert.Contains("%", result);
    }
}

