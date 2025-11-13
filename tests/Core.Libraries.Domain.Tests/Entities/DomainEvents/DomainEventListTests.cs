using Core.Libraries.Domain.Entities.DomainEvents;

namespace Core.Libraries.Domain.Tests.Entities.DomainEvents;

/// <summary>
/// Testes unitários para a classe <see cref="DomainEventList"/>.
/// </summary>
public class DomainEventListTests
{
    [Fact]
    public void Constructor_ShouldCreateEmptyList()
    {
        // Act
        var list = new DomainEventList();

        // Assert
        Assert.Empty(list);
        Assert.Empty(list.GetAll());
    }

    [Fact]
    public void RegisterEvent_WithValidParameters_ShouldAddEvent()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();
        var eventData = new { Action = "TestAction" };

        // Act
        list.RegisterEvent(entity, eventData);

        // Assert
        Assert.Single(list);
    }

    [Fact]
    public void RegisterEvent_WithNullEntity_ShouldThrowArgumentNullException()
    {
        // Arrange
        var list = new DomainEventList();
        object? entity = null;
        var eventData = new { Action = "TestAction" };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.RegisterEvent(entity!, eventData));
    }

    [Fact]
    public void RegisterEvent_WithNullEventData_ShouldThrowArgumentNullException()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();
        object? eventData = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.RegisterEvent(entity, eventData!));
    }

    [Fact]
    public void RegisterEvent_MultipleEvents_ShouldAddAllEvents()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();

        // Act
        list.RegisterEvent(entity, new { Action = "Event1" });
        list.RegisterEvent(entity, new { Action = "Event2" });
        list.RegisterEvent(entity, new { Action = "Event3" });

        // Assert
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void RegisterEvent_ShouldSetEntityReference()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();
        var eventData = new { Action = "TestAction" };

        // Act
        list.RegisterEvent(entity, eventData);

        // Assert
        var domainEvent = list.GetAll().First();
        Assert.Equal(entity, domainEvent.Entity);
    }

    [Fact]
    public void RegisterEvent_ShouldSetEventData()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();
        var eventData = new { Action = "TestAction", Value = 42 };

        // Act
        list.RegisterEvent(entity, eventData);

        // Assert
        var domainEvent = list.GetAll().First();
        Assert.Equal(eventData, domainEvent.EventData);
    }

    [Fact]
    public void RegisterEvent_ShouldIncrementOrder()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();

        // Act
        list.RegisterEvent(entity, new { Action = "Event1" });
        list.RegisterEvent(entity, new { Action = "Event2" });
        list.RegisterEvent(entity, new { Action = "Event3" });

        // Assert
        var events = list.GetAll().ToList();
        Assert.True(events[0].Order < events[1].Order);
        Assert.True(events[1].Order < events[2].Order);
    }

    [Fact]
    public void GetAll_EmptyList_ShouldReturnEmptyEnumerable()
    {
        // Arrange
        var list = new DomainEventList();

        // Act
        var events = list.GetAll();

        // Assert
        Assert.Empty(events);
    }

    [Fact]
    public void GetAll_WithEvents_ShouldReturnAllEvents()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();
        list.RegisterEvent(entity, new { Action = "Event1" });
        list.RegisterEvent(entity, new { Action = "Event2" });
        list.RegisterEvent(entity, new { Action = "Event3" });

        // Act
        var events = list.GetAll().ToList();

        // Assert
        Assert.Equal(3, events.Count);
        Assert.All(events, e => Assert.NotNull(e));
    }

    [Fact]
    public void GetAll_ShouldReturnEventsInOrder()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();
        list.RegisterEvent(entity, new { Action = "Event1" });
        list.RegisterEvent(entity, new { Action = "Event2" });
        list.RegisterEvent(entity, new { Action = "Event3" });

        // Act
        var events = list.GetAll().ToList();

        // Assert
        Assert.True(events[0].Order < events[1].Order);
        Assert.True(events[1].Order < events[2].Order);
    }

    [Fact]
    public void Clear_ShouldRemoveAllEvents()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();
        list.RegisterEvent(entity, new { Action = "Event1" });
        list.RegisterEvent(entity, new { Action = "Event2" });
        list.RegisterEvent(entity, new { Action = "Event3" });

        // Act
        list.Clear();

        // Assert
        Assert.Empty(list);
        Assert.Empty(list.GetAll());
    }

    [Fact]
    public void Clear_EmptyList_ShouldNotThrow()
    {
        // Arrange
        var list = new DomainEventList();

        // Act & Assert
        list.Clear();
        Assert.Empty(list);
    }

    [Fact]
    public void Count_AfterAddingEvents_ShouldReturnCorrectCount()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();

        // Act
        list.RegisterEvent(entity, new { Action = "Event1" });
        Assert.Single(list);

        list.RegisterEvent(entity, new { Action = "Event2" });
        Assert.Equal(2, list.Count);

        list.RegisterEvent(entity, new { Action = "Event3" });
        Assert.Equal(3, list.Count);

        // Assert
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void Count_AfterClear_ShouldReturnZero()
    {
        // Arrange
        var list = new DomainEventList();
        var entity = new object();
        list.RegisterEvent(entity, new { Action = "Event1" });
        list.RegisterEvent(entity, new { Action = "Event2" });

        // Act
        list.Clear();

        // Assert
        Assert.Empty(list);
    }

    [Fact]
    public void GetAll_ShouldNotReturnNull()
    {
        // Arrange
        var list = new DomainEventList();

        // Act
        var events = list.GetAll();

        // Assert
        Assert.NotNull(events);
    }

    [Fact]
    public void RegisterEvent_WithDifferentEntities_ShouldStoreCorrectEntity()
    {
        // Arrange
        var list = new DomainEventList();
        var entity1 = new object();
        var entity2 = new object();

        // Act
        list.RegisterEvent(entity1, new { Action = "Event1" });
        list.RegisterEvent(entity2, new { Action = "Event2" });

        // Assert
        var events = list.GetAll().ToList();
        Assert.Equal(entity1, events[0].Entity);
        Assert.Equal(entity2, events[1].Entity);
    }
}

