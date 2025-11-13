using Core.Libraries.Domain.Entities.Identifiers;

namespace Core.Libraries.Domain.Tests.Entities.Identifiers;

/// <summary>
/// Testes unitários para a classe <see cref="IdentifierType"/>.
/// </summary>
public class IdentifierTypeTests
{
    [Fact]
    public void OpenAIPlatform_ShouldHaveCorrectCodeAndName()
    {
        // Act
        var type = IdentifierType.OpenAIPlatform;

        // Assert
        Assert.Equal(1, type.Code);
        Assert.Equal("OpenAI Platform", type.Name);
    }

    [Fact]
    public void ToString_ShouldReturnName()
    {
        // Arrange
        var type = IdentifierType.OpenAIPlatform;

        // Act
        var result = type.ToString();

        // Assert
        Assert.Equal("OpenAI Platform", result);
    }

    [Fact]
    public void List_ShouldReturnAllTypes()
    {
        // Act
        var types = IdentifierType.List().ToList();

        // Assert
        Assert.NotEmpty(types);
        Assert.Contains(IdentifierType.OpenAIPlatform, types);
    }

    [Fact]
    public void FromCode_WithValidCode_ShouldReturnType()
    {
        // Act
        var type = IdentifierType.FromCode(1);

        // Assert
        Assert.Equal(IdentifierType.OpenAIPlatform, type);
    }

    [Fact]
    public void FromCode_WithInvalidCode_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => IdentifierType.FromCode(999));
    }

    [Fact]
    public void FromName_WithValidName_ShouldReturnType()
    {
        // Act
        var type = IdentifierType.FromName("OpenAI Platform");

        // Assert
        Assert.Equal(IdentifierType.OpenAIPlatform, type);
    }

    [Fact]
    public void FromName_WithValidNameCaseInsensitive_ShouldReturnType()
    {
        // Act
        var type = IdentifierType.FromName("openai platform");

        // Assert
        Assert.Equal(IdentifierType.OpenAIPlatform, type);
    }

    [Fact]
    public void FromName_WithInvalidName_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => IdentifierType.FromName("Invalid Type"));
    }

    [Fact]
    public void Equals_WithSameCode_ShouldReturnTrue()
    {
        // Arrange
        var type1 = IdentifierType.OpenAIPlatform;
        var type2 = IdentifierType.OpenAIPlatform;

        // Act & Assert
        Assert.True(type1.Equals(type2));
        Assert.True(type1 == type2);
    }

    [Fact]
    public void GetHashCode_WithSameCode_ShouldReturnSameHash()
    {
        // Arrange
        var type1 = IdentifierType.OpenAIPlatform;
        var type2 = IdentifierType.OpenAIPlatform;

        // Act
        var hash1 = type1.GetHashCode();
        var hash2 = type2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToInt()
    {
        // Arrange
        var type = IdentifierType.OpenAIPlatform;

        // Act
        int code = type;

        // Assert
        Assert.Equal(1, code);
    }
}

