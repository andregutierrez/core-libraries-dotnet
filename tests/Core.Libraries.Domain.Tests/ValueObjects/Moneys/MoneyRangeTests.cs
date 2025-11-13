using Core.Libraries.Domain.ValueObjects.Moneys;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Moneys;

/// <summary>
/// Testes unitários para a classe <see cref="MoneyRange"/>.
/// </summary>
public class MoneyRangeTests
{
    [Fact]
    public void Constructor_WithValidRange_ShouldCreateInstance()
    {
        // Arrange
        var currency = new Currency("USD");
        var min = new Money(10m, currency);
        var max = new Money(20m, currency);

        // Act
        var range = new MoneyRange(min, max);

        // Assert
        Assert.Equal(min, range.Min);
        Assert.Equal(max, range.Max);
    }

    [Fact]
    public void Constructor_WithDifferentCurrencies_ShouldThrowArgumentException()
    {
        // Arrange
        var min = new Money(10m, new Currency("USD"));
        var max = new Money(20m, new Currency("EUR"));

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new MoneyRange(min, max));
    }

    [Fact]
    public void Constructor_WithMinGreaterThanMax_ShouldThrowArgumentException()
    {
        // Arrange
        var currency = new Currency("USD");
        var min = new Money(20m, currency);
        var max = new Money(10m, currency);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new MoneyRange(min, max));
    }

    [Fact]
    public void Constructor_WithMinEqualToMax_ShouldCreateInstance()
    {
        // Arrange
        var currency = new Currency("USD");
        var money = new Money(10m, currency);

        // Act
        var range = new MoneyRange(money, money);

        // Assert
        Assert.Equal(money, range.Min);
        Assert.Equal(money, range.Max);
    }

    [Fact]
    public void Contains_WithValueInRange_ShouldReturnTrue()
    {
        // Arrange
        var currency = new Currency("USD");
        var range = new MoneyRange(new Money(10m, currency), new Money(20m, currency));
        var value = new Money(15m, currency);

        // Act
        var result = range.Contains(value);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithValueEqualToMin_ShouldReturnTrue()
    {
        // Arrange
        var currency = new Currency("USD");
        var min = new Money(10m, currency);
        var range = new MoneyRange(min, new Money(20m, currency));

        // Act
        var result = range.Contains(min);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithValueEqualToMax_ShouldReturnTrue()
    {
        // Arrange
        var currency = new Currency("USD");
        var max = new Money(20m, currency);
        var range = new MoneyRange(new Money(10m, currency), max);

        // Act
        var result = range.Contains(max);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithValueBelowMin_ShouldReturnFalse()
    {
        // Arrange
        var currency = new Currency("USD");
        var range = new MoneyRange(new Money(10m, currency), new Money(20m, currency));
        var value = new Money(5m, currency);

        // Act
        var result = range.Contains(value);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_WithValueAboveMax_ShouldReturnFalse()
    {
        // Arrange
        var currency = new Currency("USD");
        var range = new MoneyRange(new Money(10m, currency), new Money(20m, currency));
        var value = new Money(25m, currency);

        // Act
        var result = range.Contains(value);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_WithDifferentCurrency_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var currency = new Currency("USD");
        var range = new MoneyRange(new Money(10m, currency), new Money(20m, currency));
        var value = new Money(15m, new Currency("EUR"));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => range.Contains(value));
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var currency = new Currency("USD");
        var range = new MoneyRange(new Money(10m, currency), new Money(20m, currency));

        // Act
        var result = range.ToString();

        // Assert
        Assert.Contains("10.00", result);
        Assert.Contains("20.00", result);
    }
}

