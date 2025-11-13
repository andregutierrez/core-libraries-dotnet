using Core.Libraries.Domain.ValueObjects;
using Xunit;

namespace Core.Libraries.Domain.Tests.ValueObjects;

/// <summary>
/// Classe de teste concreta para testar Enumeration.
/// </summary>
public class TestEnumeration : Enumeration
{
    public static readonly TestEnumeration Value1 = new(1, "Value1");
    public static readonly TestEnumeration Value2 = new(2, "Value2");
    public static readonly TestEnumeration Value3 = new(3, "Value3");

    private TestEnumeration(int code, string name) : base(code, name) { }
}

/// <summary>
/// Testes unitários para a classe <see cref="Enumeration"/>.
/// </summary>
public class EnumerationTests
{
    [Fact]
    public void Constructor_WithIdAndName_ShouldSetProperties()
    {
        // Arrange & Act
        var enumeration = TestEnumeration.Value1;

        // Assert
        Assert.Equal(1, enumeration.Code);
        Assert.Equal("Value1", enumeration.Name);
    }

    [Fact]
    public void GetAll_ShouldReturnAllStaticInstances()
    {
        // Act
        var all = Enumeration.GetAll<TestEnumeration>().ToList();

        // Assert
        Assert.Equal(3, all.Count);
        Assert.Contains(TestEnumeration.Value1, all);
        Assert.Contains(TestEnumeration.Value2, all);
        Assert.Contains(TestEnumeration.Value3, all);
    }

    [Fact]
    public void ToString_ShouldReturnName()
    {
        // Arrange
        var enumeration = TestEnumeration.Value2;

        // Act
        var result = enumeration.ToString();

        // Assert
        Assert.Equal("Value2", result);
    }

    [Fact]
    public void Equals_WithSameIdAndType_ShouldReturnTrue()
    {
        // Arrange
        var enum1 = TestEnumeration.Value1;
        var enum2 = TestEnumeration.Value1;

        // Act & Assert
        Assert.True(enum1.Equals(enum2));
        Assert.True(enum1.Equals((object)enum2));
    }

    [Fact]
    public void Equals_WithDifferentId_ShouldReturnFalse()
    {
        // Arrange
        var enum1 = TestEnumeration.Value1;
        var enum2 = TestEnumeration.Value2;

        // Act & Assert
        Assert.False(enum1.Equals(enum2));
        Assert.False(enum1.Equals((object)enum2));
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var enum1 = TestEnumeration.Value1;
        var other = new { Id = 1, Name = "Value1" };

        // Act & Assert
        Assert.False(enum1.Equals(other));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var enumeration = TestEnumeration.Value1;

        // Act & Assert
        Assert.False(enumeration.Equals(null));
        Assert.False(enumeration.Equals((object?)null));
    }

    [Fact]
    public void GetHashCode_ShouldReturnIdHashCode()
    {
        // Arrange
        var enum1 = TestEnumeration.Value1;
        var enum2 = TestEnumeration.Value1;

        // Act
        var hash1 = enum1.GetHashCode();
        var hash2 = enum2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
        Assert.Equal(enum1.Code.GetHashCode(), hash1);
    }

    [Fact]
    public void CompareTo_WithSmallerId_ShouldReturnPositive()
    {
        // Arrange
        var enum1 = TestEnumeration.Value2; // Id = 2
        var enum2 = TestEnumeration.Value1; // Id = 1

        // Act
        var result = enum1.CompareTo(enum2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithLargerId_ShouldReturnNegative()
    {
        // Arrange
        var enum1 = TestEnumeration.Value1; // Id = 1
        var enum2 = TestEnumeration.Value2; // Id = 2

        // Act
        var result = enum1.CompareTo(enum2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_WithSameId_ShouldReturnZero()
    {
        // Arrange
        var enum1 = TestEnumeration.Value1;
        var enum2 = TestEnumeration.Value1;

        // Act
        var result = enum1.CompareTo(enum2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WithNull_ShouldReturnPositive()
    {
        // Arrange
        var enumeration = TestEnumeration.Value1;

        // Act
        var result = enumeration.CompareTo(null);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_WithDifferentType_ShouldThrowArgumentException()
    {
        // Arrange
        var enumeration = TestEnumeration.Value1;
        var other = new { Id = 1 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => enumeration.CompareTo(other));
    }
}

