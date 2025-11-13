using Core.Libraries.Domain.Entities.DomainEvents;

namespace Core.Libraries.Domain.Tests.Entities.DomainEvents;

/// <summary>
/// Testes unitários para a classe <see cref="DomainEvent"/>.
/// </summary>
public class DomainEventTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var entity = new object();
        var eventData = new { Action = "TestAction" };
        var order = 1L;

        // Act
        var domainEvent = new DomainEvent(entity, eventData, order);

        // Assert
        Assert.Equal(entity, domainEvent.Entity);
        Assert.Equal(eventData, domainEvent.EventData);
        Assert.Equal(order, domainEvent.Order);
    }

    [Fact]
    public void Constructor_WithNullEntity_ShouldThrowArgumentNullException()
    {
        // Arrange
        object? entity = null;
        var eventData = new { Action = "TestAction" };
        var order = 1L;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEvent(entity!, eventData, order));
    }

    [Fact]
    public void Constructor_WithNullEventData_ShouldThrowArgumentNullException()
    {
        // Arrange
        var entity = new object();
        object? eventData = null;
        var order = 1L;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DomainEvent(entity, eventData!, order));
    }

    [Fact]
    public void Create_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var entity = new object();
        var eventData = new { Action = "TestAction" };

        // Act
        var domainEvent = DomainEvent.Create(entity, eventData);

        // Assert
        Assert.Equal(entity, domainEvent.Entity);
        Assert.Equal(eventData, domainEvent.EventData);
        Assert.True(domainEvent.Order > 0);
    }

    [Fact]
    public void Create_WithNullEntity_ShouldThrowArgumentNullException()
    {
        // Arrange
        object? entity = null;
        var eventData = new { Action = "TestAction" };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => DomainEvent.Create(entity!, eventData));
    }

    [Fact]
    public void Create_WithNullEventData_ShouldThrowArgumentNullException()
    {
        // Arrange
        var entity = new object();
        object? eventData = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => DomainEvent.Create(entity, eventData!));
    }

    [Fact]
    public void Create_MultipleEvents_ShouldIncrementOrder()
    {
        // Arrange
        var entity = new object();
        var eventData1 = new { Action = "Action1" };
        var eventData2 = new { Action = "Action2" };
        var eventData3 = new { Action = "Action3" };

        // Act
        var event1 = DomainEvent.Create(entity, eventData1);
        var event2 = DomainEvent.Create(entity, eventData2);
        var event3 = DomainEvent.Create(entity, eventData3);

        // Assert
        Assert.True(event1.Order < event2.Order);
        Assert.True(event2.Order < event3.Order);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueOrders()
    {
        // Arrange
        var entity = new object();
        var events = new List<DomainEvent>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            events.Add(DomainEvent.Create(entity, new { Index = i }));
        }

        // Assert
        var orders = events.Select(e => e.Order).ToList();
        Assert.Equal(orders.Count, orders.Distinct().Count());
    }

    [Fact]
    public void Entity_ShouldBeImmutable()
    {
        // Arrange
        var entity = new object();
        var eventData = new { Action = "TestAction" };
        var domainEvent = DomainEvent.Create(entity, eventData);

        // Act
        var entityProperty = domainEvent.Entity;

        // Assert
        Assert.Equal(entity, entityProperty);
    }

    [Fact]
    public void EventData_ShouldBeImmutable()
    {
        // Arrange
        var entity = new object();
        var eventData = new { Action = "TestAction" };
        var domainEvent = DomainEvent.Create(entity, eventData);

        // Act
        var eventDataProperty = domainEvent.EventData;

        // Assert
        Assert.Equal(eventData, eventDataProperty);
    }

    [Fact]
    public void Order_ShouldBeImmutable()
    {
        // Arrange
        var entity = new object();
        var eventData = new { Action = "TestAction" };
        var domainEvent = DomainEvent.Create(entity, eventData);
        var originalOrder = domainEvent.Order;

        // Act
        var orderProperty = domainEvent.Order;

        // Assert
        Assert.Equal(originalOrder, orderProperty);
    }

}

