using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Tests.Entities;

/// <summary>
/// Classe de teste concreta para testar Entity&lt;TKey&gt;.
/// </summary>
public class TestEntity : Entity<EntityId>
{
    public string Name { get; set; } = string.Empty;

    public TestEntity() { }

    public TestEntity(EntityId id, string name) : base(id)
    {
        Name = name;
    }
}

/// <summary>
/// Testes unitários para a classe <see cref="Entity{TKey}"/>.
/// </summary>
public class EntityTests
{
    [Fact]
    public void Constructor_WithId_ShouldSetId()
    {
        // Arrange
        var id = new EntityId(123);

        // Act
        var entity = new TestEntity(id, "Test");

        // Assert
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Constructor_WithoutId_ShouldSetDefaultId()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        Assert.Equal(default(EntityId), entity.Id);
    }

    [Fact]
    public void Id_ShouldReturnSetId()
    {
        // Arrange
        var id = new EntityId(456);
        var entity = new TestEntity(id, "Test");

        // Act
        var result = entity.Id;

        // Assert
        Assert.Equal(id, result);
    }

    [Fact]
    public void Equals_WithSameIdAndType_ShouldReturnTrue()
    {
        // Arrange
        var id = new EntityId(123);
        var entity1 = new TestEntity(id, "Test1");
        var entity2 = new TestEntity(id, "Test2");

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentId_ShouldReturnFalse()
    {
        // Arrange
        var entity1 = new TestEntity(new EntityId(123), "Test1");
        var entity2 = new TestEntity(new EntityId(456), "Test2");

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithTransientEntities_ShouldReturnFalse()
    {
        // Arrange
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var entity = new TestEntity(new EntityId(123), "Test");

        // Act
        var result = entity.Equals((object?)null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithSameReference_ShouldReturnTrue()
    {
        // Arrange
        var entity = new TestEntity(new EntityId(123), "Test");

        // Act
        var result = entity.Equals(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetHashCode_WithPersistedEntity_ShouldReturnIdHashCode()
    {
        // Arrange
        var id = new EntityId(123);
        var entity = new TestEntity(id, "Test");

        // Act
        var hashCode = entity.GetHashCode();

        // Assert
        Assert.Equal(id.GetHashCode(), hashCode);
    }

    [Fact]
    public void GetHashCode_WithTransientEntity_ShouldReturnBaseHashCode()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        var hashCode = entity.GetHashCode();

        // Assert
        Assert.NotEqual(0, hashCode);
    }

    [Fact]
    public void CompareTo_WithSmallerId_ShouldReturnPositive()
    {
        // Arrange
        var entity1 = new TestEntity(new EntityId(100), "Test1");
        var entity2 = new TestEntity(new EntityId(50), "Test2");

        // Act
        var result = entity1.CompareTo(entity2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLargerId_ShouldReturnNegative()
    {
        // Arrange
        var entity1 = new TestEntity(new EntityId(50), "Test1");
        var entity2 = new TestEntity(new EntityId(100), "Test2");

        // Act
        var result = entity1.CompareTo(entity2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithSameId_ShouldReturnZero()
    {
        // Arrange
        var id = new EntityId(123);
        var entity1 = new TestEntity(id, "Test1");
        var entity2 = new TestEntity(id, "Test2");

        // Act
        var result = entity1.CompareTo(entity2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WithNull_ShouldReturnPositive()
    {
        // Arrange
        var entity = new TestEntity(new EntityId(123), "Test");

        // Act
        var result = entity.CompareTo(null);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithTransientEntities_ShouldReturnZero()
    {
        // Arrange
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        // Act
        var result = entity1.CompareTo(entity2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_TransientWithPersisted_ShouldReturnNegative()
    {
        // Arrange
        var transient = new TestEntity();
        var persisted = new TestEntity(new EntityId(123), "Test");

        // Act
        var result = transient.CompareTo(persisted);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_PersistedWithTransient_ShouldReturnPositive()
    {
        // Arrange
        var persisted = new TestEntity(new EntityId(123), "Test");
        var transient = new TestEntity();

        // Act
        var result = persisted.CompareTo(transient);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var id = new EntityId(123);
        var entity = new TestEntity(id, "Test");

        // Act
        var result = entity.ToString();

        // Assert
        Assert.Equal($"TestEntity [Id={id}]", result);
    }
}

