using Core.Libraries.Domain.ValueObjects.Measurements;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Measurements;

/// <summary>
/// Testes unitários para a classe <see cref="Length"/>.
/// </summary>
public class LengthTests
{
    [Fact]
    public void Constructor_WithValidMeters_ShouldCreateInstance()
    {
        // Arrange
        var meters = 10.5m;

        // Act
        var length = new Length(meters);

        // Assert
        Assert.Equal(10.5m, length.Meters);
    }

    [Fact]
    public void Constructor_WithNegativeMeters_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Length(-1m));
    }

    [Fact]
    public void FromKilometers_ShouldConvertCorrectly()
    {
        // Act
        var length = Length.FromKilometers(2.5m);

        // Assert
        Assert.Equal(2500m, length.Meters);
    }

    [Fact]
    public void FromCentimeters_ShouldConvertCorrectly()
    {
        // Act
        var length = Length.FromCentimeters(150m);

        // Assert
        Assert.Equal(1.5m, length.Meters);
    }

    [Fact]
    public void ToKilometers_ShouldConvertCorrectly()
    {
        // Arrange
        var length = new Length(2500m);

        // Act
        var result = length.ToKilometers();

        // Assert
        Assert.Equal(2.5m, result);
    }

    [Fact]
    public void ToCentimeters_ShouldConvertCorrectly()
    {
        // Arrange
        var length = new Length(1.5m);

        // Act
        var result = length.ToCentimeters();

        // Assert
        Assert.Equal(150m, result);
    }

    [Fact]
    public void Add_ShouldReturnSum()
    {
        // Arrange
        var length1 = new Length(10m);
        var length2 = new Length(5m);

        // Act
        var result = length1.Add(length2);

        // Assert
        Assert.Equal(15m, result.Meters);
    }

    [Fact]
    public void Subtract_WithValidResult_ShouldReturnDifference()
    {
        // Arrange
        var length1 = new Length(10m);
        var length2 = new Length(5m);

        // Act
        var result = length1.Subtract(length2);

        // Assert
        Assert.Equal(5m, result.Meters);
    }

    [Fact]
    public void Subtract_WithNegativeResult_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var length1 = new Length(5m);
        var length2 = new Length(10m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => length1.Subtract(length2));
    }

    [Fact]
    public void Multiply_ShouldReturnProduct()
    {
        // Arrange
        var length = new Length(10m);

        // Act
        var result = length.Multiply(2.5m);

        // Assert
        Assert.Equal(25m, result.Meters);
    }

    [Fact]
    public void CompareTo_WithSmallerLength_ShouldReturnPositive()
    {
        // Arrange
        var length1 = new Length(10m);
        var length2 = new Length(5m);

        // Act
        var result = length1.CompareTo(length2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLargerLength_ShouldReturnNegative()
    {
        // Arrange
        var length1 = new Length(5m);
        var length2 = new Length(10m);

        // Act
        var result = length1.CompareTo(length2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithEqualLength_ShouldReturnZero()
    {
        // Arrange
        var length1 = new Length(10m);
        var length2 = new Length(10m);

        // Act
        var result = length1.CompareTo(length2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void OperatorLessThan_ShouldWorkCorrectly()
    {
        // Arrange
        var length1 = new Length(5m);
        var length2 = new Length(10m);

        // Act & Assert
        Assert.True(length1 < length2);
        Assert.False(length2 < length1);
    }

    [Fact]
    public void OperatorGreaterThan_ShouldWorkCorrectly()
    {
        // Arrange
        var length1 = new Length(10m);
        var length2 = new Length(5m);

        // Act & Assert
        Assert.True(length1 > length2);
        Assert.False(length2 > length1);
    }

    [Fact]
    public void OperatorLessThanOrEqual_ShouldWorkCorrectly()
    {
        // Arrange
        var length1 = new Length(5m);
        var length2 = new Length(10m);
        var length3 = new Length(5m);

        // Act & Assert
        Assert.True(length1 <= length2);
        Assert.True(length1 <= length3);
        Assert.False(length2 <= length1);
    }

    [Fact]
    public void OperatorGreaterThanOrEqual_ShouldWorkCorrectly()
    {
        // Arrange
        var length1 = new Length(10m);
        var length2 = new Length(5m);
        var length3 = new Length(10m);

        // Act & Assert
        Assert.True(length1 >= length2);
        Assert.True(length1 >= length3);
        Assert.False(length2 >= length1);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var length = new Length(10.5m);

        // Act
        var result = length.ToString();

        // Assert
        Assert.Contains("10.50", result);
        Assert.Contains("m", result);
    }
}

