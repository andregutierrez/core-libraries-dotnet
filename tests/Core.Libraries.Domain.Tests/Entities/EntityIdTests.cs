using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Tests.Entities;

/// <summary>
/// Testes unitários para a classe <see cref="EntityId"/>.
/// </summary>
public class EntityIdTests
{
    [Fact]
    public void Constructor_WithValidValue_ShouldCreateInstance()
    {
        // Arrange
        var value = 123;

        // Act
        var entityId = new EntityId(value);

        // Assert
        Assert.Equal(value, entityId.Value);
    }

    [Fact]
    public void Constructor_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new EntityId(value));
    }

    [Fact]
    public void Constructor_WithZero_ShouldCreateInstance()
    {
        // Arrange
        var value = 0;

        // Act
        var entityId = new EntityId(value);

        // Assert
        Assert.Equal(value, entityId.Value);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var value = 123;
        var entityId1 = new EntityId(value);
        var entityId2 = new EntityId(value);

        // Act
        var result = entityId1.Equals(entityId2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var entityId1 = new EntityId(123);
        var entityId2 = new EntityId(456);

        // Act
        var result = entityId1.Equals(entityId2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CompareTo_WithSmallerValue_ShouldReturnPositive()
    {
        // Arrange
        var entityId1 = new EntityId(100);
        var entityId2 = new EntityId(50);

        // Act
        var result = entityId1.CompareTo(entityId2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLargerValue_ShouldReturnNegative()
    {
        // Arrange
        var entityId1 = new EntityId(50);
        var entityId2 = new EntityId(100);

        // Act
        var result = entityId1.CompareTo(entityId2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithSameValue_ShouldReturnZero()
    {
        // Arrange
        var value = 123;
        var entityId1 = new EntityId(value);
        var entityId2 = new EntityId(value);

        // Act
        var result = entityId1.CompareTo(entityId2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ToString_ShouldReturnValueAsString()
    {
        // Arrange
        var value = 123;
        var entityId = new EntityId(value);

        // Act
        var result = entityId.ToString();

        // Assert
        Assert.Equal(value.ToString(), result);
    }
}

