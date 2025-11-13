using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;

namespace Core.Libraries.Domain.Tests.Entities.Identifiers;

/// <summary>
/// Classe de teste concreta para testar Identifier.
/// </summary>
public class TestIdentifier : Identifier
{
    public string ExternalValue { get; set; } = string.Empty;

    public TestIdentifier() { }

    public TestIdentifier(Guid key, IdentifierType type, string externalValue)
        : base(key, type)
    {
        ExternalValue = externalValue;
    }

    public TestIdentifier(IdentifierId id, Guid key, IdentifierType type, string externalValue)
        : base(id, key, type)
    {
        ExternalValue = externalValue;
    }
}

/// <summary>
/// Testes unitários para a classe <see cref="Identifier"/>.
/// </summary>
public class IdentifierTests
{
    [Fact]
    public void Constructor_WithKeyAndType_ShouldSetProperties()
    {
        // Arrange
        var key = Guid.NewGuid();
        var type = IdentifierType.OpenAIPlatform;

        // Act
        var identifier = new TestIdentifier(key, type, "test-value");

        // Assert
        Assert.Equal(key, identifier.Key.Value);
        Assert.Equal(type, identifier.Type);
        Assert.Equal("test-value", identifier.ExternalValue);
    }

    [Fact]
    public void Constructor_WithIdKeyAndType_ShouldSetAllProperties()
    {
        // Arrange
        var id = new IdentifierId(123);
        var key = Guid.NewGuid();
        var type = IdentifierType.OpenAIPlatform;

        // Act
        var identifier = new TestIdentifier(id, key, type, "test-value");

        // Assert
        Assert.Equal(id, identifier.Id);
        Assert.Equal(key, identifier.Key.Value);
        Assert.Equal(type, identifier.Type);
        Assert.Equal("test-value", identifier.ExternalValue);
    }

    [Fact]
    public void Key_ShouldImplementIHasAlternateKey()
    {
        // Arrange
        var key = Guid.NewGuid();
        var type = IdentifierType.OpenAIPlatform;
        var identifier = new TestIdentifier(key, type, "test-value");

        // Act & Assert
        var hasAlternateKey = identifier as IHasAlternateKey;
        Assert.NotNull(hasAlternateKey);
        Assert.Equal(key, hasAlternateKey.Key.Value);
    }

    [Fact]
    public void Key_ShouldImplementIIdentifier()
    {
        // Arrange
        var key = Guid.NewGuid();
        var type = IdentifierType.OpenAIPlatform;
        var identifier = new TestIdentifier(key, type, "test-value");

        // Act & Assert
        var iIdentifier = identifier as IIdentifier;
        Assert.NotNull(iIdentifier);
        Assert.Equal(key, iIdentifier.Key.Value);
        Assert.Equal(type, iIdentifier.Type);
    }
}

