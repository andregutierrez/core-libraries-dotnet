using Core.Libraries.Domain.ValueObjects.Moneys;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Moneys;

/// <summary>
/// Testes unitários para a classe <see cref="Money"/>.
/// </summary>
public class MoneyTests
{
    [Fact]
    public void Constructor_WithValidAmountAndCurrency_ShouldCreateInstance()
    {
        // Arrange
        var currency = new Currency("USD");
        var amount = 100.50m;

        // Act
        var money = new Money(amount, currency);

        // Assert
        Assert.Equal(100.50m, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Constructor_WithNullCurrency_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Money(100m, null!));
    }

    [Fact]
    public void Constructor_ShouldRoundAmountAccordingToCurrencyPrecision()
    {
        // Arrange
        var currency = new Currency("USD", 2);
        var amount = 100.555m;

        // Act
        var money = new Money(amount, currency);

        // Assert
        Assert.Equal(100.56m, money.Amount);
    }

    [Fact]
    public void Add_WithSameCurrency_ShouldReturnSum()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(100m, currency);
        var money2 = new Money(50m, currency);

        // Act
        var result = money1.Add(money2);

        // Assert
        Assert.Equal(150m, result.Amount);
        Assert.Equal(currency, result.Currency);
    }

    [Fact]
    public void Add_WithDifferentCurrency_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(100m, new Currency("USD"));
        var money2 = new Money(50m, new Currency("EUR"));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => money1.Add(money2));
    }

    [Fact]
    public void Subtract_WithSameCurrency_ShouldReturnDifference()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(100m, currency);
        var money2 = new Money(50m, currency);

        // Act
        var result = money1.Subtract(money2);

        // Assert
        Assert.Equal(50m, result.Amount);
        Assert.Equal(currency, result.Currency);
    }

    [Fact]
    public void Subtract_WithDifferentCurrency_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(100m, new Currency("USD"));
        var money2 = new Money(50m, new Currency("EUR"));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => money1.Subtract(money2));
    }

    [Fact]
    public void Multiply_ShouldReturnProduct()
    {
        // Arrange
        var currency = new Currency("USD");
        var money = new Money(100m, currency);

        // Act
        var result = money.Multiply(2.5m);

        // Assert
        Assert.Equal(250m, result.Amount);
        Assert.Equal(currency, result.Currency);
    }

    [Fact]
    public void Divide_ShouldReturnQuotient()
    {
        // Arrange
        var currency = new Currency("USD");
        var money = new Money(100m, currency);

        // Act
        var result = money.Divide(4m);

        // Assert
        Assert.Equal(25m, result.Amount);
        Assert.Equal(currency, result.Currency);
    }

    [Fact]
    public void CompareTo_WithSmallerAmount_ShouldReturnPositive()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(100m, currency);
        var money2 = new Money(50m, currency);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLargerAmount_ShouldReturnNegative()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(50m, currency);
        var money2 = new Money(100m, currency);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithEqualAmount_ShouldReturnZero()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(100m, currency);
        var money2 = new Money(100m, currency);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WithDifferentCurrency_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(100m, new Currency("USD"));
        var money2 = new Money(100m, new Currency("EUR"));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => money1.CompareTo(money2));
    }

    [Fact]
    public void OperatorLessThan_ShouldWorkCorrectly()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(50m, currency);
        var money2 = new Money(100m, currency);

        // Act & Assert
        Assert.True(money1 < money2);
        Assert.False(money2 < money1);
    }

    [Fact]
    public void OperatorGreaterThan_ShouldWorkCorrectly()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(100m, currency);
        var money2 = new Money(50m, currency);

        // Act & Assert
        Assert.True(money1 > money2);
        Assert.False(money2 > money1);
    }

    [Fact]
    public void OperatorLessThanOrEqual_ShouldWorkCorrectly()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(50m, currency);
        var money2 = new Money(100m, currency);
        var money3 = new Money(50m, currency);

        // Act & Assert
        Assert.True(money1 <= money2);
        Assert.True(money1 <= money3);
        Assert.False(money2 <= money1);
    }

    [Fact]
    public void OperatorGreaterThanOrEqual_ShouldWorkCorrectly()
    {
        // Arrange
        var currency = new Currency("USD");
        var money1 = new Money(100m, currency);
        var money2 = new Money(50m, currency);
        var money3 = new Money(100m, currency);

        // Act & Assert
        Assert.True(money1 >= money2);
        Assert.True(money1 >= money3);
        Assert.False(money2 >= money1);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var currency = new Currency("USD");
        var money = new Money(123.45m, currency);

        // Act
        var result = money.ToString();

        // Assert
        Assert.Contains("USD", result);
        Assert.Contains("123.45", result);
    }
}

