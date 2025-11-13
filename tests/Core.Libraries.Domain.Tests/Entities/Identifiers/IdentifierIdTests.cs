using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;

namespace Core.Libraries.Domain.Tests.Entities.Identifiers;

/// <summary>
/// Testes unitários para a classe <see cref="IdentifierId"/>.
/// </summary>
public class IdentifierIdTests
{
    [Fact]
    public void Constructor_WithValue_ShouldSetValue()
    {
        // Arrange
        var value = 123;

        // Act
        var id = new IdentifierId(value);

        // Assert
        Assert.Equal(value, id.Value);
    }

    [Fact]
    public void ImplicitOperator_FromInt_ShouldCreateIdentifierId()
    {
        // Act
        IdentifierId id = 456;

        // Assert
        Assert.Equal(456, id.Value);
    }

    [Fact]
    public void ImplicitOperator_ToInt_ShouldReturnValue()
    {
        // Arrange
        var id = new IdentifierId(789);

        // Act
        int value = id;

        // Assert
        Assert.Equal(789, value);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var id1 = new IdentifierId(123);
        var id2 = new IdentifierId(123);

        // Act & Assert
        Assert.Equal(id1, id2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var id1 = new IdentifierId(123);
        var id2 = new IdentifierId(456);

        // Act & Assert
        Assert.NotEqual(id1, id2);
    }
}

