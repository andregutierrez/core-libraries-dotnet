using Core.Libraries.Domain.Entities.Lifecycle;
using Xunit;

namespace Core.Libraries.Domain.Tests.Entities.Lifecycle;

/// <summary>
/// Testes unitários para a classe <see cref="AuditInfo"/>.
/// </summary>
public class AuditInfoTests
{
    [Fact]
    public void Create_ShouldSetCreatedAt()
    {
        // Act
        var auditInfo = AuditInfo.Create();

        // Assert
        Assert.NotEqual(default(DateTime), auditInfo.CreatedAt);
        Assert.Null(auditInfo.UpdatedAt);
    }

    [Fact]
    public void MarkAsModified_ShouldSetUpdatedAt()
    {
        // Arrange
        var auditInfo = AuditInfo.Create();
        var beforeUpdate = auditInfo.CreatedAt;

        // Act
        auditInfo.MarkAsModified();

        // Assert
        Assert.NotNull(auditInfo.UpdatedAt);
        Assert.True(auditInfo.UpdatedAt >= beforeUpdate);
    }

    [Fact]
    public void MarkAsModified_MultipleTimes_ShouldUpdateTimestamp()
    {
        // Arrange
        var auditInfo = AuditInfo.Create();
        auditInfo.MarkAsModified();
        var firstUpdate = auditInfo.UpdatedAt;

        // Act
        System.Threading.Thread.Sleep(10); // Pequeno delay para garantir timestamp diferente
        auditInfo.MarkAsModified();

        // Assert
        Assert.True(auditInfo.UpdatedAt > firstUpdate);
    }
}

/// <summary>
/// Testes unitários para a classe genérica <see cref="AuditInfo{TUserId}"/>.
/// </summary>
public class AuditInfoGenericTests
{
    [Fact]
    public void Constructor_WithUserId_ShouldSetCreatedBy()
    {
        // Arrange
        var userId = "user123";
        var createdAt = DateTime.UtcNow;

        // Act
        var auditInfo = new AuditInfo<string>(createdAt, userId);

        // Assert
        Assert.Equal(userId, auditInfo.CreatedBy);
        Assert.Null(auditInfo.UpdatedBy);
        Assert.Equal(createdAt, auditInfo.CreatedAt);
    }

    [Fact]
    public void MarkAsModified_WithUserId_ShouldSetUpdatedBy()
    {
        // Arrange
        var auditInfo = new AuditInfo<string>(DateTime.UtcNow, "user123");
        var updatedBy = "user456";

        // Act
        auditInfo.MarkAsModified(updatedBy);

        // Assert
        Assert.Equal(updatedBy, auditInfo.UpdatedBy);
        Assert.NotNull(auditInfo.UpdatedAt);
    }

    [Fact]
    public void Constructor_WithIntUserId_ShouldWork()
    {
        // Arrange
        var userId = 123;
        var createdAt = DateTime.UtcNow;

        // Act
        var auditInfo = new AuditInfo<int>(createdAt, userId);

        // Assert
        Assert.Equal(123, auditInfo.CreatedBy);
        Assert.Equal(createdAt, auditInfo.CreatedAt);
    }
}
