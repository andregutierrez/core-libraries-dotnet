using Core.Libraries.Domain.ValueObjects.Measurements;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects.Measurements;

/// <summary>
/// Testes unitários para a classe <see cref="Volume"/>.
/// </summary>
public class VolumeTests
{
    [Fact]
    public void Constructor_WithValidLiters_ShouldCreateInstance()
    {
        // Arrange
        var liters = 2.5m;

        // Act
        var volume = new Volume(liters);

        // Assert
        Assert.Equal(2.5m, volume.Liters);
    }

    [Fact]
    public void Constructor_WithNegativeLiters_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Volume(-1m));
    }

    [Fact]
    public void FromMilliliters_ShouldConvertCorrectly()
    {
        // Act
        var volume = Volume.FromMilliliters(1500m);

        // Assert
        Assert.Equal(1.5m, volume.Liters);
    }

    [Fact]
    public void FromCubicMeters_ShouldConvertCorrectly()
    {
        // Act
        var volume = Volume.FromCubicMeters(2m);

        // Assert
        Assert.Equal(2000m, volume.Liters);
    }

    [Fact]
    public void ToMilliliters_ShouldConvertCorrectly()
    {
        // Arrange
        var volume = new Volume(1.5m);

        // Act
        var result = volume.ToMilliliters();

        // Assert
        Assert.Equal(1500m, result);
    }

    [Fact]
    public void ToCubicMeters_ShouldConvertCorrectly()
    {
        // Arrange
        var volume = new Volume(2000m);

        // Act
        var result = volume.ToCubicMeters();

        // Assert
        Assert.Equal(2m, result);
    }

    [Fact]
    public void Add_ShouldReturnSum()
    {
        // Arrange
        var volume1 = new Volume(10m);
        var volume2 = new Volume(5m);

        // Act
        var result = volume1.Add(volume2);

        // Assert
        Assert.Equal(15m, result.Liters);
    }

    [Fact]
    public void Subtract_WithValidResult_ShouldReturnDifference()
    {
        // Arrange
        var volume1 = new Volume(10m);
        var volume2 = new Volume(5m);

        // Act
        var result = volume1.Subtract(volume2);

        // Assert
        Assert.Equal(5m, result.Liters);
    }

    [Fact]
    public void Subtract_WithNegativeResult_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var volume1 = new Volume(5m);
        var volume2 = new Volume(10m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => volume1.Subtract(volume2));
    }

    [Fact]
    public void Multiply_ShouldReturnProduct()
    {
        // Arrange
        var volume = new Volume(10m);

        // Act
        var result = volume.Multiply(2.5m);

        // Assert
        Assert.Equal(25m, result.Liters);
    }

    [Fact]
    public void CompareTo_WithSmallerVolume_ShouldReturnPositive()
    {
        // Arrange
        var volume1 = new Volume(10m);
        var volume2 = new Volume(5m);

        // Act
        var result = volume1.CompareTo(volume2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLargerVolume_ShouldReturnNegative()
    {
        // Arrange
        var volume1 = new Volume(5m);
        var volume2 = new Volume(10m);

        // Act
        var result = volume1.CompareTo(volume2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithEqualVolume_ShouldReturnZero()
    {
        // Arrange
        var volume1 = new Volume(10m);
        var volume2 = new Volume(10m);

        // Act
        var result = volume1.CompareTo(volume2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void OperatorLessThan_ShouldWorkCorrectly()
    {
        // Arrange
        var volume1 = new Volume(5m);
        var volume2 = new Volume(10m);

        // Act & Assert
        Assert.True(volume1 < volume2);
        Assert.False(volume2 < volume1);
    }

    [Fact]
    public void OperatorGreaterThan_ShouldWorkCorrectly()
    {
        // Arrange
        var volume1 = new Volume(10m);
        var volume2 = new Volume(5m);

        // Act & Assert
        Assert.True(volume1 > volume2);
        Assert.False(volume2 > volume1);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var volume = new Volume(2.5m);

        // Act
        var result = volume.ToString();

        // Assert
        Assert.Contains("2.50", result);
        Assert.Contains("L", result);
    }
}

