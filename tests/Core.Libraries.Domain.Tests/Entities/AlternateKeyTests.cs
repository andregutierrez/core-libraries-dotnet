using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Tests.Entities;

/// <summary>
/// Testes unitários para a classe <see cref="AlternateKey"/>.
/// </summary>
public class AlternateKeyTests
{
    [Fact]
    public void Constructor_WithValidGuid_ShouldCreateInstance()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var alternateKey = new AlternateKey(guid);

        // Assert
        Assert.Equal(guid, alternateKey.Value);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new AlternateKey(emptyGuid));
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void New_ShouldCreateInstanceWithNewGuid()
    {
        // Act
        var alternateKey = AlternateKey.New();

        // Assert
        Assert.NotEqual(Guid.Empty, alternateKey.Value);
    }

    [Fact]
    public void New_ShouldCreateUniqueInstances()
    {
        // Act
        var key1 = AlternateKey.New();
        var key2 = AlternateKey.New();

        // Assert
        Assert.NotEqual(key1.Value, key2.Value);
    }

    [Fact]
    public void ImplicitOperator_FromGuid_ShouldCreateInstance()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        AlternateKey alternateKey = guid;

        // Assert
        Assert.Equal(guid, alternateKey.Value);
    }

    [Fact]
    public void ImplicitOperator_FromEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => { AlternateKey _ = emptyGuid; });
    }

    [Fact]
    public void ImplicitOperator_ToGuid_ShouldReturnValue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var alternateKey = new AlternateKey(guid);

        // Act
        Guid result = alternateKey;

        // Assert
        Assert.Equal(guid, result);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var key1 = new AlternateKey(guid);
        var key2 = new AlternateKey(guid);

        // Act
        var result = key1.Equals(key2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var key1 = AlternateKey.New();
        var key2 = AlternateKey.New();

        // Act
        var result = key1.Equals(key2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var key = AlternateKey.New();

        // Act
        var result = key.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var key1 = new AlternateKey(guid);
        var key2 = new AlternateKey(guid);

        // Act
        var result = key1 == key2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorNotEquals_WithDifferentValue_ShouldReturnTrue()
    {
        // Arrange
        var key1 = AlternateKey.New();
        var key2 = AlternateKey.New();

        // Act
        var result = key1 != key2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ToString_ShouldReturnGuidAsString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var alternateKey = new AlternateKey(guid);

        // Act
        var result = alternateKey.ToString();

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void GetHashCode_WithSameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var key1 = new AlternateKey(guid);
        var key2 = new AlternateKey(guid);

        // Act
        var hashCode1 = key1.GetHashCode();
        var hashCode2 = key2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }
}

