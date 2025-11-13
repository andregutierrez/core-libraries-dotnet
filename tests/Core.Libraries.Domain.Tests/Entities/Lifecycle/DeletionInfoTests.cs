using Core.Libraries.Domain.Entities.Lifecycle;
using Xunit;

namespace Core.Libraries.Domain.Tests.Entities.Lifecycle;

/// <summary>
/// Testes unitários para a classe <see cref="DeletionInfo"/>.
/// </summary>
public class DeletionInfoTests
{
    [Fact]
    public void Constructor_ShouldInitializeAsNotDeleted()
    {
        // Act
        var deletionInfo = new DeletionInfo();

        // Assert
        Assert.False(deletionInfo.IsDeleted);
        Assert.Null(deletionInfo.DeletedAt);
    }

    [Fact]
    public void MarkAsDeleted_ShouldSetIsDeletedAndDeletedAt()
    {
        // Arrange
        var deletionInfo = new DeletionInfo();

        // Act
        deletionInfo.MarkAsDeleted();

        // Assert
        Assert.True(deletionInfo.IsDeleted);
        Assert.NotNull(deletionInfo.DeletedAt);
    }

    [Fact]
    public void MarkAsDeleted_MultipleTimes_ShouldNotChangeTimestamp()
    {
        // Arrange
        var deletionInfo = new DeletionInfo();
        deletionInfo.MarkAsDeleted();
        var firstDeletedAt = deletionInfo.DeletedAt;

        // Act
        System.Threading.Thread.Sleep(10);
        deletionInfo.MarkAsDeleted();

        // Assert
        Assert.Equal(firstDeletedAt, deletionInfo.DeletedAt);
    }

    [Fact]
    public void Restore_ShouldClearDeletionInfo()
    {
        // Arrange
        var deletionInfo = new DeletionInfo();
        deletionInfo.MarkAsDeleted();

        // Act
        deletionInfo.Restore();

        // Assert
        Assert.False(deletionInfo.IsDeleted);
        Assert.Null(deletionInfo.DeletedAt);
    }

    [Fact]
    public void Restore_WhenNotDeleted_ShouldNotThrow()
    {
        // Arrange
        var deletionInfo = new DeletionInfo();

        // Act & Assert
        deletionInfo.Restore(); // Não deve lançar exceção
        Assert.False(deletionInfo.IsDeleted);
    }
}

/// <summary>
/// Testes unitários para a classe genérica <see cref="DeletionInfo{TUserId}"/>.
/// </summary>
public class DeletionInfoGenericTests
{
    [Fact]
    public void MarkAsDeleted_WithUserId_ShouldSetDeletedBy()
    {
        // Arrange
        var deletionInfo = new DeletionInfo<string>();
        var deletedBy = "user123";

        // Act
        deletionInfo.MarkAsDeleted(deletedBy);

        // Assert
        Assert.True(deletionInfo.IsDeleted);
        Assert.Equal(deletedBy, deletionInfo.DeletedBy);
        Assert.NotNull(deletionInfo.DeletedAt);
    }

    [Fact]
    public void MarkAsDeleted_WithoutUserId_ShouldStillMarkAsDeleted()
    {
        // Arrange
        var deletionInfo = new DeletionInfo<string>();

        // Act
        deletionInfo.MarkAsDeleted();

        // Assert
        Assert.True(deletionInfo.IsDeleted);
        Assert.Null(deletionInfo.DeletedBy);
    }

    [Fact]
    public void Restore_WithUserId_ShouldClearDeletionInfo()
    {
        // Arrange
        var deletionInfo = new DeletionInfo<string>();
        deletionInfo.MarkAsDeleted("user123");

        // Act
        deletionInfo.Restore();

        // Assert
        Assert.False(deletionInfo.IsDeleted);
        Assert.Null(deletionInfo.DeletedAt);
        Assert.Null(deletionInfo.DeletedBy);
    }

    [Fact]
    public void DeletedAt_ShouldReturnDeletedAt()
    {
        // Arrange
        var deletionInfo = new DeletionInfo<string>();
        deletionInfo.MarkAsDeleted("user123");

        // Act
        var deletedAt = deletionInfo.DeletedAt;

        // Assert
        Assert.NotNull(deletedAt);
    }

    [Fact]
    public void DeletedBy_ShouldReturnUserId()
    {
        // Arrange
        var deletionInfo = new DeletionInfo<string>();
        var userId = "user123";
        deletionInfo.MarkAsDeleted(userId);

        // Act
        var deletedBy = deletionInfo.DeletedBy;

        // Assert
        Assert.Equal(userId, deletedBy);
    }
}

