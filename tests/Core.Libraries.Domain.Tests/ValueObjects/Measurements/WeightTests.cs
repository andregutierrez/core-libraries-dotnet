using Core.Libraries.Domain.ValueObjects.Measurements;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Measurements;

/// <summary>
/// Testes unitários para a classe <see cref="Weight"/>.
/// </summary>
public class WeightTests
{
    [Fact]
    public void Constructor_WithValidKilograms_ShouldCreateInstance()
    {
        // Arrange
        var kilograms = 75.5m;

        // Act
        var weight = new Weight(kilograms);

        // Assert
        Assert.Equal(75.5m, weight.Kilograms);
    }

    [Fact]
    public void Constructor_WithNegativeKilograms_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Weight(-1m));
    }

    [Fact]
    public void FromGrams_ShouldConvertCorrectly()
    {
        // Act
        var weight = Weight.FromGrams(1500m);

        // Assert
        Assert.Equal(1.5m, weight.Kilograms);
    }

    [Fact]
    public void FromPounds_ShouldConvertCorrectly()
    {
        // Act
        var weight = Weight.FromPounds(2.20462m); // ~1 kg

        // Assert
        Assert.True(weight.Kilograms > 0.99m && weight.Kilograms < 1.01m);
    }

    [Fact]
    public void ToGrams_ShouldConvertCorrectly()
    {
        // Arrange
        var weight = new Weight(1.5m);

        // Act
        var result = weight.ToGrams();

        // Assert
        Assert.Equal(1500m, result);
    }

    [Fact]
    public void ToPounds_ShouldConvertCorrectly()
    {
        // Arrange
        var weight = new Weight(1m);

        // Act
        var result = weight.ToPounds();

        // Assert
        Assert.True(result > 2.2m && result < 2.21m);
    }

    [Fact]
    public void Add_ShouldReturnSum()
    {
        // Arrange
        var weight1 = new Weight(10m);
        var weight2 = new Weight(5m);

        // Act
        var result = weight1.Add(weight2);

        // Assert
        Assert.Equal(15m, result.Kilograms);
    }

    [Fact]
    public void Subtract_WithValidResult_ShouldReturnDifference()
    {
        // Arrange
        var weight1 = new Weight(10m);
        var weight2 = new Weight(5m);

        // Act
        var result = weight1.Subtract(weight2);

        // Assert
        Assert.Equal(5m, result.Kilograms);
    }

    [Fact]
    public void Subtract_WithNegativeResult_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var weight1 = new Weight(5m);
        var weight2 = new Weight(10m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => weight1.Subtract(weight2));
    }

    [Fact]
    public void Multiply_ShouldReturnProduct()
    {
        // Arrange
        var weight = new Weight(10m);

        // Act
        var result = weight.Multiply(2.5m);

        // Assert
        Assert.Equal(25m, result.Kilograms);
    }

    [Fact]
    public void CompareTo_WithSmallerWeight_ShouldReturnPositive()
    {
        // Arrange
        var weight1 = new Weight(10m);
        var weight2 = new Weight(5m);

        // Act
        var result = weight1.CompareTo(weight2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLargerWeight_ShouldReturnNegative()
    {
        // Arrange
        var weight1 = new Weight(5m);
        var weight2 = new Weight(10m);

        // Act
        var result = weight1.CompareTo(weight2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithEqualWeight_ShouldReturnZero()
    {
        // Arrange
        var weight1 = new Weight(10m);
        var weight2 = new Weight(10m);

        // Act
        var result = weight1.CompareTo(weight2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void OperatorLessThan_ShouldWorkCorrectly()
    {
        // Arrange
        var weight1 = new Weight(5m);
        var weight2 = new Weight(10m);

        // Act & Assert
        Assert.True(weight1 < weight2);
        Assert.False(weight2 < weight1);
    }

    [Fact]
    public void OperatorGreaterThan_ShouldWorkCorrectly()
    {
        // Arrange
        var weight1 = new Weight(10m);
        var weight2 = new Weight(5m);

        // Act & Assert
        Assert.True(weight1 > weight2);
        Assert.False(weight2 > weight1);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var weight = new Weight(75.5m);

        // Act
        var result = weight.ToString();

        // Assert
        Assert.Contains("75.50", result);
        Assert.Contains("kg", result);
    }
}

