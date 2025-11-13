using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.DomainEvents;

namespace Core.Libraries.Domain.Tests.Entities;

/// <summary>
/// Classe de teste concreta para testar Aggregate&lt;TEntityId&gt;.
/// </summary>
public class TestAggregate : Aggregate<EntityId>
{
    public string Name { get; set; } = string.Empty;

    public TestAggregate() { }

    public TestAggregate(EntityId id, string name) : base(id)
    {
        Name = name;
    }

    public void DoSomething()
    {
        RegisterEvent(new { Action = "SomethingDone", Name });
    }
}

/// <summary>
/// Testes unitários para a classe <see cref="Aggregate{TEntityId}"/>.
/// </summary>
public class AggregateTests
{
    [Fact]
    public void Constructor_WithId_ShouldSetId()
    {
        // Arrange
        var id = new EntityId(123);

        // Act
        var aggregate = new TestAggregate(id, "Test");

        // Assert
        Assert.Equal(id, aggregate.Id);
    }

    [Fact]
    public void Constructor_WithoutId_ShouldSetDefaultId()
    {
        // Act
        var aggregate = new TestAggregate();

        // Assert
        Assert.Equal(default(EntityId), aggregate.Id);
    }

    [Fact]
    public void RegisterEvent_WithValidEventData_ShouldAddEvent()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");
        var eventData = new { Action = "TestAction" };

        // Act
        aggregate.RegisterEvent(eventData);

        // Assert
        var events = ((IHasDomainEvents)aggregate).Events;
        Assert.Equal(1, events.Count); // IDomainEventList não é diretamente enumerável, então usamos Count
    }

    [Fact]
    public void RegisterEvent_WithNullEventData_ShouldThrowArgumentNullException()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => aggregate.RegisterEvent(null!));
    }

    [Fact]
    public void RegisterEvent_MultipleEvents_ShouldAddAllEvents()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");

        // Act
        aggregate.RegisterEvent(new { Action = "Event1" });
        aggregate.RegisterEvent(new { Action = "Event2" });
        aggregate.RegisterEvent(new { Action = "Event3" });

        // Assert
        var events = ((IHasDomainEvents)aggregate).Events;
        Assert.Equal(3, events.Count);
    }

    [Fact]
    public void Events_ShouldReturnDomainEventList()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");

        // Act
        var events = ((IHasDomainEvents)aggregate).Events;

        // Assert
        Assert.NotNull(events);
        Assert.Equal(0, events.Count);
    }

    [Fact]
    public void Events_GetAll_ShouldReturnAllEvents()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");
        aggregate.RegisterEvent(new { Action = "Event1" });
        aggregate.RegisterEvent(new { Action = "Event2" });

        // Act
        var events = ((IHasDomainEvents)aggregate).Events;
        var allEvents = events.GetAll().ToList();

        // Assert
        Assert.Equal(2, allEvents.Count);
        Assert.All(allEvents, e => Assert.NotNull(e));
    }

    [Fact]
    public void Events_Clear_ShouldRemoveAllEvents()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");
        aggregate.RegisterEvent(new { Action = "Event1" });
        aggregate.RegisterEvent(new { Action = "Event2" });

        // Act
        var events = ((IHasDomainEvents)aggregate).Events;
        events.Clear();

        // Assert
        Assert.Equal(0, events.Count);
        Assert.Empty(events.GetAll());
    }

    [Fact]
    public void RegisterEvent_ShouldSetEntityReference()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");
        var eventData = new { Action = "TestAction" };

        // Act
        aggregate.RegisterEvent(eventData);

        // Assert
        var events = ((IHasDomainEvents)aggregate).Events;
        var domainEvent = events.GetAll().First();
        Assert.Equal(aggregate, domainEvent.Entity);
    }

    [Fact]
    public void RegisterEvent_ShouldSetEventData()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");
        var eventData = new { Action = "TestAction", Value = 42 };

        // Act
        aggregate.RegisterEvent(eventData);

        // Assert
        var events = ((IHasDomainEvents)aggregate).Events;
        var domainEvent = events.GetAll().First();
        Assert.Equal(eventData, domainEvent.EventData);
    }

    [Fact]
    public void RegisterEvent_ShouldIncrementOrder()
    {
        // Arrange
        var aggregate = new TestAggregate(new EntityId(123), "Test");

        // Act
        aggregate.RegisterEvent(new { Action = "Event1" });
        aggregate.RegisterEvent(new { Action = "Event2" });
        aggregate.RegisterEvent(new { Action = "Event3" });

        // Assert
        var events = ((IHasDomainEvents)aggregate).Events;
        var allEvents = events.GetAll().ToList();
        Assert.True(allEvents[0].Order < allEvents[1].Order);
        Assert.True(allEvents[1].Order < allEvents[2].Order);
    }
}

