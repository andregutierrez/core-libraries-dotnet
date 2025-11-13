using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;

namespace Core.Libraries.Domain.Tests.Entities.Identifiers;

/// <summary>
/// Testes unitários para a classe <see cref="IdentifiersList{TIdentifier}"/>.
/// </summary>
public class IdentifiersListTests
{
    [Fact]
    public void Add_WithIdentifier_ShouldAddToCollection()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");

        // Act
        list.Add(identifier);

        // Assert
        Assert.Single(list);
        Assert.Contains(identifier, list);
    }

    [Fact]
    public void Count_AfterAdding_ShouldReturnCorrectCount()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key1 = Guid.NewGuid();
        var key2 = Guid.NewGuid();
        var identifier1 = new TestIdentifier(key1, IdentifierType.OpenAIPlatform, "value1");
        var identifier2 = new TestIdentifier(key2, IdentifierType.OpenAIPlatform, "value2");

        // Act
        list.Add(identifier1);
        list.Add(identifier2);

        // Assert
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void GetEnumerator_ShouldIterateThroughIdentifiers()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key1 = Guid.NewGuid();
        var key2 = Guid.NewGuid();
        var identifier1 = new TestIdentifier(key1, IdentifierType.OpenAIPlatform, "value1");
        var identifier2 = new TestIdentifier(key2, IdentifierType.OpenAIPlatform, "value2");
        list.Add(identifier1);
        list.Add(identifier2);

        // Act
        var identifiers = list.ToList();

        // Assert
        Assert.Equal(2, identifiers.Count);
        Assert.Contains(identifier1, identifiers);
        Assert.Contains(identifier2, identifiers);
    }

    [Fact]
    public void AsReadOnly_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var readOnly = list.AsReadOnly();

        // Assert
        Assert.Single(readOnly);
        Assert.Contains(identifier, readOnly);
    }

    [Fact]
    public void Contains_WithExistingIdentifier_ShouldReturnTrue()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var contains = list.Contains(identifier);

        // Assert
        Assert.True(contains);
    }

    [Fact]
    public void Contains_WithNonExistingIdentifier_ShouldReturnFalse()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var id1 = new IdentifierId(1);
        var id2 = new IdentifierId(2);
        var key1 = Guid.NewGuid();
        var key2 = Guid.NewGuid();
        var identifier1 = new TestIdentifier(id1, key1, IdentifierType.OpenAIPlatform, "value1");
        var identifier2 = new TestIdentifier(id2, key2, IdentifierType.OpenAIPlatform, "value2");
        list.Add(identifier1);

        // Act
        var contains = list.Contains(identifier2);

        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void GetByType_WithExistingType_ShouldReturnIdentifier()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var result = list.GetByType(IdentifierType.OpenAIPlatform);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(identifier, result);
    }

    [Fact]
    public void GetByType_WithNonExistingType_ShouldReturnNull()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Criar um tipo inexistente usando reflexão para evitar exceção
        var nonExistentType = (IdentifierType)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(IdentifierType));
        var codeField = typeof(IdentifierType).GetField("<Code>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var nameField = typeof(IdentifierType).GetField("<Name>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        codeField?.SetValue(nonExistentType, 999);
        nameField?.SetValue(nonExistentType, "Non-Existent Type");

        // Act
        var result = list.GetByType(nonExistentType);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void HasType_WithExistingType_ShouldReturnTrue()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var result = list.HasType(IdentifierType.OpenAIPlatform);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasType_WithNonExistingType_ShouldReturnFalse()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Criar um tipo inexistente usando reflexão para evitar exceção
        var nonExistentType = (IdentifierType)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(IdentifierType));
        var codeField = typeof(IdentifierType).GetField("<Code>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var nameField = typeof(IdentifierType).GetField("<Name>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        codeField?.SetValue(nonExistentType, 999);
        nameField?.SetValue(nonExistentType, "Non-Existent Type");

        // Act
        var result = list.HasType(nonExistentType);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemoveByType_WithExistingType_ShouldRemoveAndReturnTrue()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var result = list.RemoveByType(IdentifierType.OpenAIPlatform);

        // Assert
        Assert.True(result);
        Assert.Empty(list);
        Assert.False(list.HasType(IdentifierType.OpenAIPlatform));
    }

    [Fact]
    public void RemoveByType_WithNonExistingType_ShouldReturnFalse()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Criar um tipo inexistente usando reflexão para evitar exceção
        var nonExistentType = (IdentifierType)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(IdentifierType));
        var codeField = typeof(IdentifierType).GetField("<Code>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var nameField = typeof(IdentifierType).GetField("<Name>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        codeField?.SetValue(nonExistentType, 999);
        nameField?.SetValue(nonExistentType, "Non-Existent Type");

        // Act
        var result = list.RemoveByType(nonExistentType);

        // Assert
        Assert.False(result);
        Assert.Single(list);
    }

    [Fact]
    public void GetByKey_WithExistingKey_ShouldReturnIdentifier()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = AlternateKey.New();
        var identifier = new TestIdentifier(key.Value, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var result = list.GetByKey(key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(identifier, result);
    }

    [Fact]
    public void GetByKey_WithNonExistingKey_ShouldReturnNull()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key1 = AlternateKey.New();
        var key2 = AlternateKey.New();
        var identifier = new TestIdentifier(key1.Value, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var result = list.GetByKey(key2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void RemoveByKey_WithExistingKey_ShouldRemoveAndReturnTrue()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = AlternateKey.New();
        var identifier = new TestIdentifier(key.Value, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var result = list.RemoveByKey(key);

        // Assert
        Assert.True(result);
        Assert.Empty(list);
        Assert.Null(list.GetByKey(key));
    }

    [Fact]
    public void RemoveByKey_WithNonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key1 = AlternateKey.New();
        var key2 = AlternateKey.New();
        var identifier = new TestIdentifier(key1.Value, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Act
        var result = list.RemoveByKey(key2);

        // Assert
        Assert.False(result);
        Assert.Single(list);
        Assert.NotNull(list.GetByKey(key1));
    }

    [Fact]
    public void RemoveByKey_WithMultipleIdentifiers_ShouldRemoveOnlySpecified()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key1 = AlternateKey.New();
        var key2 = AlternateKey.New();
        var identifier1 = new TestIdentifier(key1.Value, IdentifierType.OpenAIPlatform, "value1");
        var identifier2 = new TestIdentifier(key2.Value, IdentifierType.OpenAIPlatform, "value2");
        list.Add(identifier1);
        list.Add(identifier2);

        // Act
        var result = list.RemoveByKey(key1);

        // Assert
        Assert.True(result);
        Assert.Single(list);
        Assert.Null(list.GetByKey(key1));
        Assert.NotNull(list.GetByKey(key2));
        Assert.Equal(identifier2, list.GetByKey(key2));
    }

    [Fact]
    public void GetAllByType_WithMultipleIdentifiersOfSameType_ShouldReturnAll()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key1 = Guid.NewGuid();
        var key2 = Guid.NewGuid();
        var identifier1 = new TestIdentifier(key1, IdentifierType.OpenAIPlatform, "value1");
        var identifier2 = new TestIdentifier(key2, IdentifierType.OpenAIPlatform, "value2");
        list.Add(identifier1);
        list.Add(identifier2);

        // Act
        var result = list.GetAllByType(IdentifierType.OpenAIPlatform).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(identifier1, result);
        Assert.Contains(identifier2, result);
    }

    [Fact]
    public void GetAllByType_WithNoIdentifiersOfType_ShouldReturnEmpty()
    {
        // Arrange
        var list = new IdentifiersList<TestIdentifier>();
        var key = Guid.NewGuid();
        var identifier = new TestIdentifier(key, IdentifierType.OpenAIPlatform, "test-value");
        list.Add(identifier);

        // Criar um tipo inexistente usando reflexão para evitar exceção
        var nonExistentType = (IdentifierType)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(IdentifierType));
        var codeField = typeof(IdentifierType).GetField("<Code>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var nameField = typeof(IdentifierType).GetField("<Name>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        codeField?.SetValue(nonExistentType, 999);
        nameField?.SetValue(nonExistentType, "Non-Existent Type");

        // Act
        var result = list.GetAllByType(nonExistentType).ToList();

        // Assert
        Assert.Empty(result);
    }
}

