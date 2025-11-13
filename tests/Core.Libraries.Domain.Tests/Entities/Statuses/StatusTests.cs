using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Statuses;
using Xunit;

namespace Core.Libraries.Domain.Tests.Entities.Statuses;

/// <summary>
/// Classe de teste concreta para testar Status.
/// </summary>
public enum TestStatusType
{
    Pending = 1,
    Active = 2,
    Completed = 3
}

public class TestStatus : Status<TestStatusType>
{
    public TestStatus() { }

    public TestStatus(Guid key, DateTime createdAt, TestStatusType type, string notes)
        : base(key, createdAt, type, notes) { }
}

/// <summary>
/// Testes unitários para a classe <see cref="Status{TType}"/>.
/// </summary>
public class StatusTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var key = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var type = TestStatusType.Active;
        var notes = "Test notes";

        // Act
        var status = new TestStatus(key, createdAt, type, notes);

        // Assert
        Assert.Equal(key, status.Key.Value);
        Assert.Equal(createdAt, status.CreatedAt);
        Assert.Equal(type, status.Type);
        Assert.Equal(notes, status.Notes);
        Assert.True(status.IsCurrent);
    }

    [Fact]
    public void Deactivate_ShouldSetIsCurrentToFalse()
    {
        // Arrange
        var status = new TestStatus(Guid.NewGuid(), DateTime.UtcNow, TestStatusType.Active, "Test");

        // Act
        status.Deactivate();

        // Assert
        Assert.False(status.IsCurrent);
    }

    [Fact]
    public void Type_ShouldBeSettable()
    {
        // Arrange
        var status = new TestStatus(Guid.NewGuid(), DateTime.UtcNow, TestStatusType.Pending, "Test");

        // Act
        status.Type = TestStatusType.Completed;

        // Assert
        Assert.Equal(TestStatusType.Completed, status.Type);
    }

    [Fact]
    public void Notes_ShouldBeSettable()
    {
        // Arrange
        var status = new TestStatus(Guid.NewGuid(), DateTime.UtcNow, TestStatusType.Active, "Original");

        // Act
        status.Notes = "Updated";

        // Assert
        Assert.Equal("Updated", status.Notes);
    }
}

