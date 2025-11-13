using Core.Libraries.Domain.ValueObjects.Moneys;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Moneys;

/// <summary>
/// Testes unitários para a classe <see cref="ExchangeRate"/>.
/// </summary>
public class ExchangeRateTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var from = new Currency("USD");
        var to = new Currency("EUR");
        var rate = 0.85m;

        // Act
        var exchangeRate = new ExchangeRate(from, to, rate);

        // Assert
        Assert.Equal(from, exchangeRate.From);
        Assert.Equal(to, exchangeRate.To);
        Assert.Equal(0.85m, exchangeRate.Rate);
    }

    [Fact]
    public void Constructor_WithNullFrom_ShouldThrowArgumentNullException()
    {
        // Arrange
        var to = new Currency("EUR");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(null!, to, 0.85m));
    }

    [Fact]
    public void Constructor_WithNullTo_ShouldThrowArgumentNullException()
    {
        // Arrange
        var from = new Currency("USD");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(from, null!, 0.85m));
    }

    [Fact]
    public void Constructor_WithZeroRate_ShouldThrowArgumentException()
    {
        // Arrange
        var from = new Currency("USD");
        var to = new Currency("EUR");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ExchangeRate(from, to, 0m));
    }

    [Fact]
    public void Constructor_WithNegativeRate_ShouldThrowArgumentException()
    {
        // Arrange
        var from = new Currency("USD");
        var to = new Currency("EUR");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ExchangeRate(from, to, -1m));
    }

    [Fact]
    public void Convert_WithMatchingCurrency_ShouldReturnConvertedMoney()
    {
        // Arrange
        var from = new Currency("USD");
        var to = new Currency("EUR");
        var rate = 0.85m;
        var exchangeRate = new ExchangeRate(from, to, rate);
        var sourceMoney = new Money(100m, from);

        // Act
        var result = exchangeRate.Convert(sourceMoney);

        // Assert
        Assert.Equal(85m, result.Amount);
        Assert.Equal(to, result.Currency);
    }

    [Fact]
    public void Convert_WithMismatchedCurrency_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var from = new Currency("USD");
        var to = new Currency("EUR");
        var rate = 0.85m;
        var exchangeRate = new ExchangeRate(from, to, rate);
        var sourceMoney = new Money(100m, new Currency("BRL"));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => exchangeRate.Convert(sourceMoney));
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var from = new Currency("USD");
        var to = new Currency("EUR");
        var rate = 0.85m;
        var exchangeRate = new ExchangeRate(from, to, rate);

        // Act
        var result = exchangeRate.ToString();

        // Assert
        Assert.Contains("1 USD", result);
        Assert.Contains("0.850000 EUR", result);
    }
}

