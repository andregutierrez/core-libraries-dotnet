using Core.Libraries.Domain.Entities.Statuses;
using Xunit;

namespace Core.Libraries.Domain.Tests.Entities.Statuses;

/// <summary>
/// Testes unitários para a classe <see cref="StatusId"/>.
/// </summary>
public class StatusIdTests
{
    [Fact]
    public void Constructor_WithValidId_ShouldCreateInstance()
    {
        // Arrange
        var id = 123;

        // Act
        var statusId = new StatusId(id);

        // Assert
        Assert.Equal(123, statusId.Value);
    }

    [Fact]
    public void ImplicitConversion_FromInt_ShouldCreateStatusId()
    {
        // Act
        StatusId statusId = 456;

        // Assert
        Assert.Equal(456, statusId.Value);
    }

    [Fact]
    public void ImplicitConversion_ToInt_ShouldReturnValue()
    {
        // Arrange
        var statusId = new StatusId(789);

        // Act
        int value = statusId;

        // Assert
        Assert.Equal(789, value);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var statusId1 = new StatusId(123);
        var statusId2 = new StatusId(123);

        // Act & Assert
        Assert.Equal(statusId1, statusId2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var statusId1 = new StatusId(123);
        var statusId2 = new StatusId(456);

        // Act & Assert
        Assert.NotEqual(statusId1, statusId2);
    }
}

