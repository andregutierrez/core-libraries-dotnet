using Core.Libraries.Domain.Entities;

namespace Core.Libraries.Domain.Tests.Entities;

/// <summary>
/// Classe de teste concreta para testar EntityList&lt;TEntity&gt;.
/// </summary>
public class TestEntityList : EntityList<string>
{
    // Classe concreta para testar a classe abstrata EntityList
}

/// <summary>
/// Testes unitários para a classe <see cref="EntityList{TEntity}"/>.
/// </summary>
public class EntityListTests
{
    [Fact]
    public void Count_EmptyList_ShouldReturnZero()
    {
        // Arrange
        var list = new TestEntityList();

        // Act
        var count = list.Count;

        // Assert
        Assert.Empty(list);
    }

    [Fact]
    public void Add_WithValidItem_ShouldIncreaseCount()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;

        // Act
        collection.Add("Item1");

        // Assert
        Assert.Single(list);
    }

    [Fact]
    public void Add_WithNullItem_ShouldThrowArgumentNullException()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => collection.Add(null!));
    }

    [Fact]
    public void Add_MultipleItems_ShouldIncreaseCount()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;

        // Act
        collection.Add("Item1");
        collection.Add("Item2");
        collection.Add("Item3");

        // Assert
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void Contains_WithExistingItem_ShouldReturnTrue()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");

        // Act
        var result = collection.Contains("Item1");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithNonExistingItem_ShouldReturnFalse()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");

        // Act
        var result = collection.Contains("Item2");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");

        // Act
        var result = collection.Contains(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Remove_WithExistingItem_ShouldReturnTrue()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");

        // Act
        var result = collection.Remove("Item1");

        // Assert
        Assert.True(result);
        Assert.Empty(list);
    }

    [Fact]
    public void Remove_WithNonExistingItem_ShouldReturnFalse()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");

        // Act
        var result = collection.Remove("Item2");

        // Assert
        Assert.False(result);
        Assert.Single(list);
    }

    [Fact]
    public void Remove_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");

        // Act
        var result = collection.Remove(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Clear_ShouldRemoveAllItems()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");
        collection.Add("Item2");
        collection.Add("Item3");

        // Act
        collection.Clear();

        // Assert
        Assert.Empty(list);
        Assert.Empty(collection);
    }

    [Fact]
    public void CopyTo_ShouldCopyItemsToArray()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");
        collection.Add("Item2");
        collection.Add("Item3");
        var array = new string[5];

        // Act
        collection.CopyTo(array, 1);

        // Assert
        Assert.Null(array[0]);
        Assert.Equal("Item1", array[1]);
        Assert.Equal("Item2", array[2]);
        Assert.Equal("Item3", array[3]);
        Assert.Null(array[4]);
    }

    [Fact]
    public void GetEnumerator_ShouldIterateAllItems()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");
        collection.Add("Item2");
        collection.Add("Item3");

        // Act
        var items = collection.ToList();

        // Assert
        Assert.Equal(3, items.Count);
        Assert.Contains("Item1", items);
        Assert.Contains("Item2", items);
        Assert.Contains("Item3", items);
    }

    [Fact]
    public void IsReadOnly_ShouldReturnFalse()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;

        // Act
        var isReadOnly = collection.IsReadOnly;

        // Assert
        Assert.False(isReadOnly);
    }

    [Fact]
    public void AsReadOnly_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");
        collection.Add("Item2");

        // Act
        var readOnly = list.AsReadOnly();

        // Assert
        Assert.Equal(2, readOnly.Count);
        Assert.Contains("Item1", readOnly);
        Assert.Contains("Item2", readOnly);
    }

    [Fact]
    public void ElementType_ShouldReturnCorrectType()
    {
        // Arrange
        var list = new TestEntityList();

        // Act
        var elementType = list.ElementType;

        // Assert
        Assert.Equal(typeof(string), elementType);
    }

    [Fact]
    public void Expression_ShouldNotBeNull()
    {
        // Arrange
        var list = new TestEntityList();

        // Act
        var expression = list.Expression;

        // Assert
        Assert.NotNull(expression);
    }

    [Fact]
    public void Provider_ShouldNotBeNull()
    {
        // Arrange
        var list = new TestEntityList();

        // Act
        var provider = list.Provider;

        // Assert
        Assert.NotNull(provider);
    }

    [Fact]
    public void AsQueryable_ShouldSupportLinqQueries()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");
        collection.Add("Item2");
        collection.Add("Item3");

        // Act
        var result = list.Where(x => x.Contains("Item")).ToList();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void IQueryable_Where_ShouldFilterItems()
    {
        // Arrange
        var list = new TestEntityList();
        ICollection<string> collection = list;
        collection.Add("Item1");
        collection.Add("Item2");
        collection.Add("Item3");

        // Act
        IQueryable<string> queryable = list;
        var result = queryable.Where(x => x == "Item2").ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Item2", result[0]);
    }
}

