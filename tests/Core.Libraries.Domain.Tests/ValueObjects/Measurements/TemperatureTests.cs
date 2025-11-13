using Core.Libraries.Domain.ValueObjects.Measurements;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Measurements;

/// <summary>
/// Testes unitários para a classe <see cref="Temperature"/>.
/// </summary>
public class TemperatureTests
{
    [Fact]
    public void Constructor_WithValidKelvin_ShouldCreateInstance()
    {
        // Arrange
        var kelvin = 300m;

        // Act
        var temperature = new Temperature(kelvin);

        // Assert
        Assert.Equal(300m, temperature.Kelvin);
    }

    [Fact]
    public void Constructor_WithNegativeKelvin_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Temperature(-1m));
    }

    [Fact]
    public void FromCelsius_ShouldConvertCorrectly()
    {
        // Act
        var temperature = Temperature.FromCelsius(25m);

        // Assert
        Assert.Equal(298.15m, temperature.Kelvin);
    }

    [Fact]
    public void FromFahrenheit_ShouldConvertCorrectly()
    {
        // Act
        var temperature = Temperature.FromFahrenheit(77m); // ~25°C

        // Assert
        Assert.True(temperature.Kelvin > 298m && temperature.Kelvin < 299m);
    }

    [Fact]
    public void ToCelsius_ShouldConvertCorrectly()
    {
        // Arrange
        var temperature = new Temperature(298.15m);

        // Act
        var result = temperature.ToCelsius();

        // Assert
        Assert.Equal(25m, result);
    }

    [Fact]
    public void ToFahrenheit_ShouldConvertCorrectly()
    {
        // Arrange
        var temperature = new Temperature(298.15m); // 25°C

        // Act
        var result = temperature.ToFahrenheit();

        // Assert
        Assert.Equal(77m, result);
    }

    [Fact]
    public void CompareTo_WithSmallerTemperature_ShouldReturnPositive()
    {
        // Arrange
        var temp1 = new Temperature(300m);
        var temp2 = new Temperature(250m);

        // Act
        var result = temp1.CompareTo(temp2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLargerTemperature_ShouldReturnNegative()
    {
        // Arrange
        var temp1 = new Temperature(250m);
        var temp2 = new Temperature(300m);

        // Act
        var result = temp1.CompareTo(temp2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithEqualTemperature_ShouldReturnZero()
    {
        // Arrange
        var temp1 = new Temperature(300m);
        var temp2 = new Temperature(300m);

        // Act
        var result = temp1.CompareTo(temp2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void OperatorLessThan_ShouldWorkCorrectly()
    {
        // Arrange
        var temp1 = Temperature.FromCelsius(20m);
        var temp2 = Temperature.FromCelsius(25m);

        // Act & Assert
        Assert.True(temp1 < temp2);
        Assert.False(temp2 < temp1);
    }

    [Fact]
    public void OperatorGreaterThan_ShouldWorkCorrectly()
    {
        // Arrange
        var temp1 = Temperature.FromCelsius(25m);
        var temp2 = Temperature.FromCelsius(20m);

        // Act & Assert
        Assert.True(temp1 > temp2);
        Assert.False(temp2 > temp1);
    }

    [Fact]
    public void OperatorLessThanOrEqual_ShouldWorkCorrectly()
    {
        // Arrange
        var temp1 = Temperature.FromCelsius(20m);
        var temp2 = Temperature.FromCelsius(25m);
        var temp3 = Temperature.FromCelsius(20m);

        // Act & Assert
        Assert.True(temp1 <= temp2);
        Assert.True(temp1 <= temp3);
        Assert.False(temp2 <= temp1);
    }

    [Fact]
    public void OperatorGreaterThanOrEqual_ShouldWorkCorrectly()
    {
        // Arrange
        var temp1 = Temperature.FromCelsius(25m);
        var temp2 = Temperature.FromCelsius(20m);
        var temp3 = Temperature.FromCelsius(25m);

        // Act & Assert
        Assert.True(temp1 >= temp2);
        Assert.True(temp1 >= temp3);
        Assert.False(temp2 >= temp1);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var temperature = Temperature.FromCelsius(25m);

        // Act
        var result = temperature.ToString();

        // Assert
        Assert.Contains("25.00", result);
        Assert.Contains("°C", result);
    }
}

