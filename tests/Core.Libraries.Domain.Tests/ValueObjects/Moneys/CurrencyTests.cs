using Core.Libraries.Domain.ValueObjects.Moneys;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Moneys;

/// <summary>
/// Testes unitários para a classe <see cref="Currency"/>.
/// </summary>
public class CurrencyTests
{
    [Fact]
    public void Constructor_WithValidCode_ShouldCreateInstance()
    {
        // Arrange
        var code = "USD";

        // Act
        var currency = new Currency(code);

        // Assert
        Assert.Equal("USD", currency.Code);
        Assert.Equal(2, currency.DecimalPlaces);
    }

    [Fact]
    public void Constructor_WithValidCodeAndDecimalPlaces_ShouldCreateInstance()
    {
        // Arrange
        var code = "JPY";
        var decimalPlaces = 0;

        // Act
        var currency = new Currency(code, decimalPlaces);

        // Assert
        Assert.Equal("JPY", currency.Code);
        Assert.Equal(0, currency.DecimalPlaces);
    }

    [Fact]
    public void Constructor_WithLowercaseCode_ShouldNormalizeToUppercase()
    {
        // Arrange
        var code = "usd";

        // Act
        var currency = new Currency(code);

        // Assert
        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public void Constructor_WithNullCode_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency(null!));
    }

    [Fact]
    public void Constructor_WithEmptyCode_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency(""));
        Assert.Throws<ArgumentException>(() => new Currency("   "));
    }

    [Fact]
    public void Constructor_WithInvalidLength_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency("US")); // 2 letras
        Assert.Throws<ArgumentException>(() => new Currency("USDD")); // 4 letras
    }

    [Fact]
    public void Constructor_WithNegativeDecimalPlaces_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency("USD", -1));
    }

    [Fact]
    public void ToString_ShouldReturnCode()
    {
        // Arrange
        var currency = new Currency("USD");

        // Act
        var result = currency.ToString();

        // Assert
        Assert.Equal("USD", result);
    }

    [Fact]
    public void Equals_WithSameCode_ShouldReturnTrue()
    {
        // Arrange
        var currency1 = new Currency("USD");
        var currency2 = new Currency("usd");

        // Act & Assert
        Assert.Equal(currency1, currency2);
    }

    [Fact]
    public void Equals_WithDifferentCode_ShouldReturnFalse()
    {
        // Arrange
        var currency1 = new Currency("USD");
        var currency2 = new Currency("EUR");

        // Act & Assert
        Assert.NotEqual(currency1, currency2);
    }
}

