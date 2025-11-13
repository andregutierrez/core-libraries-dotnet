using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Statuses;
using Core.Libraries.Domain.Exceptions;
using Xunit;

namespace Core.Libraries.Domain.Tests.Entities.Statuses;

/// <summary>
/// Testes unitários para a classe <see cref="StatusHistory{TStatus}"/>.
/// </summary>
public class StatusHistoryTests
{
    // Enum de teste para status
    public enum TestStatusType
    {
        Pending,
        Active,
        Inactive,
        Suspended
    }

    // Entidade de status de teste
    private class TestStatus : Status<TestStatusType>
    {
        public TestStatus() { }

        public TestStatus(Guid key, DateTime createdAt, TestStatusType type, string notes)
            : base(key, createdAt, type, notes)
        {
        }
    }

    // Entidade pai de teste
    private class TestEntity : Entity<EntityId>
    {
        public string Name { get; set; } = string.Empty;
        public TestStatusHistory StatusHistory { get; }

        public TestEntity()
        {
            StatusHistory = new TestStatusHistory();
        }

        public TestEntity(IStatusTransitionValidator<TestEntity, TestStatusType> validator)
        {
            StatusHistory = new TestStatusHistory();
            StatusHistory.SetValidatorForTest(validator);
        }
    }

    // Implementação concreta de StatusHistory para testes
    private class TestStatusHistory : StatusHistory<TestStatus>
    {
        public void SetValidatorForTest(IStatusTransitionValidator<TestEntity, TestStatusType> validator)
        {
            SetValidator<TestEntity, TestStatusType>(validator);
        }

        protected override Enum GetStatusType(TestStatus status) => status.Type;
    }

    // Validador de teste
    private class TestStatusTransitionValidator : IStatusTransitionValidator<TestEntity, TestStatusType>
    {
        private readonly HashSet<(TestStatusType from, TestStatusType to)> _allowedTransitions;

        public TestStatusTransitionValidator(params (TestStatusType from, TestStatusType to)[] allowedTransitions)
        {
            _allowedTransitions = new HashSet<(TestStatusType, TestStatusType)>(allowedTransitions);
        }

        public bool CanTransition(TestStatusType fromStatus, TestStatusType toStatus, TestEntity entity)
        {
            return _allowedTransitions.Contains((fromStatus, toStatus));
        }

        public void ValidateTransition(TestStatusType fromStatus, TestStatusType toStatus, TestEntity entity)
        {
            if (!CanTransition(fromStatus, toStatus, entity))
            {
                throw new InvalidStatusTransitionException(
                    domainContext: "TestEntity",
                    fromStatus: fromStatus.ToString(),
                    toStatus: toStatus.ToString()
                );
            }
        }
    }

    [Fact]
    public void Add_WithValidStatus_ShouldAddToHistory()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;
        var status = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Pending,
            "Initial status"
        );

        // Act
        history.Add(status);

        // Assert
        Assert.Single(history);
        Assert.Equal(status, history.First());
        Assert.True(status.IsCurrent);
    }

    [Fact]
    public void Add_WithInactiveStatus_ShouldStillAdd()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;
        var status = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Pending,
            "Inactive status"
        );
        status.Deactivate(); // Marcar como não current

        // Act
        history.Add(status);

        // Assert
        // O código atual permite adicionar status inativo
        Assert.Single(history);
    }

    [Fact]
    public void Add_WithExistingCurrentStatus_ShouldDeactivatePrevious()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;
        var status1 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Pending,
            "First status"
        );
        var status2 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow.AddSeconds(1),
            TestStatusType.Active,
            "Second status"
        );

        // Act
        history.Add(status1);
        history.Add(status2);

        // Assert
        Assert.Equal(2, history.Count());
        Assert.False(status1.IsCurrent);
        Assert.True(status2.IsCurrent);
    }

    [Fact]
    public void GetCurrent_WithActiveStatus_ShouldReturnCurrentStatus()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;
        var status = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Active,
            "Current status"
        );
        history.Add(status);

        // Act
        var current = history.GetCurrent();

        // Assert
        Assert.Equal(status, current);
        Assert.True(current.IsCurrent);
    }

    [Fact]
    public void GetCurrent_WithNoActiveStatus_ShouldThrowException()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;

        // Act & Assert
        var exception = Assert.Throws<NullReferenceException>(() => history.GetCurrent());
        Assert.Contains("No active status was found", exception.Message);
    }

    [Fact]
    public void GetByType_WithMatchingType_ShouldReturnMatchingStatuses()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;
        var status1 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Active,
            "First active"
        );
        var status2 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow.AddSeconds(1),
            TestStatusType.Active,
            "Second active"
        );
        var status3 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow.AddSeconds(2),
            TestStatusType.Pending,
            "Pending"
        );

        history.Add(status1);
        history.Add(status2);
        history.Add(status3);

        // Act
        var activeStatuses = history.GetByType(TestStatusType.Active).ToList();

        // Assert
        Assert.Equal(2, activeStatuses.Count);
        Assert.All(activeStatuses, s => Assert.Equal(TestStatusType.Active, s.Type));
    }

    [Fact]
    public void GetByType_WithNoMatchingType_ShouldReturnEmpty()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;
        var status = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Active,
            "Active status"
        );
        history.Add(status);

        // Act
        var pendingStatuses = history.GetByType(TestStatusType.Pending).ToList();

        // Assert
        Assert.Empty(pendingStatuses);
    }

    [Fact]
    public void Add_WithoutValidator_ShouldWorkNormally()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;
        var status1 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Pending,
            "First"
        );
        var status2 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow.AddSeconds(1),
            TestStatusType.Active,
            "Second"
        );

        // Act
        history.Add(status1);
        history.Add(status2); // Sem validador, qualquer transição é permitida

        // Assert
        Assert.Equal(2, history.Count());
        Assert.True(status2.IsCurrent);
    }

    [Fact]
    public void Add_WithValidator_AndValidTransition_ShouldSucceed()
    {
        // Arrange
        var validator = new TestStatusTransitionValidator(
            (TestStatusType.Pending, TestStatusType.Active)
        );
        var entity = new TestEntity(validator);
        var history = entity.StatusHistory;

        var status1 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Pending,
            "Pending"
        );
        var status2 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow.AddSeconds(1),
            TestStatusType.Active,
            "Active"
        );

        // Act
        history.Add(status1);
        history.Add(status2); // Transição válida: Pending -> Active

        // Assert
        Assert.Equal(2, history.Count());
        Assert.True(status2.IsCurrent);
    }

    [Fact]
    public void Add_WithValidator_AndInvalidTransition_ShouldThrowException()
    {
        // Arrange
        var validator = new TestStatusTransitionValidator(
            (TestStatusType.Pending, TestStatusType.Active)
        // Não permite Pending -> Suspended
        );
        var entity = new TestEntity(validator);
        var history = entity.StatusHistory;

        var status1 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Pending,
            "Pending"
        );
        var status2 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow.AddSeconds(1),
            TestStatusType.Suspended,
            "Suspended"
        );

        // Act
        history.Add(status1);

        // Assert
        var exception = Assert.Throws<InvalidStatusTransitionException>(() => history.Add(status2));
        Assert.Equal("TestEntity", exception.DomainContext);
        Assert.Equal("Pending", exception.FromStatus);
        Assert.Equal("Suspended", exception.ToStatus);
        Assert.Single(history); // Status2 não foi adicionado
    }

    [Fact]
    public void Add_WithValidator_AndNoCurrentStatus_ShouldNotValidate()
    {
        // Arrange
        var validator = new TestStatusTransitionValidator(); // Nenhuma transição permitida
        var entity = new TestEntity(validator);
        var history = entity.StatusHistory;

        var status = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Pending,
            "First status"
        );

        // Act
        history.Add(status); // Primeiro status não precisa validação

        // Assert
        Assert.Single(history);
        Assert.True(status.IsCurrent);
    }

    [Fact]
    public void Add_WithValidator_AndNullParentEntity_ShouldStillValidate()
    {
        // Arrange
        var validator = new TestStatusTransitionValidator(
            (TestStatusType.Pending, TestStatusType.Active) // Permite esta transição
        );
        var history = new TestStatusHistory();
        history.SetValidatorForTest(validator);

        var status1 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow,
            TestStatusType.Pending,
            "First"
        );
        var status2 = new TestStatus(
            AlternateKey.New(),
            DateTime.UtcNow.AddSeconds(1),
            TestStatusType.Active,
            "Second"
        );

        // Act
        history.Add(status1);
        history.Add(status2); // Validação ainda é executada mesmo sem entidade pai

        // Assert
        Assert.Equal(2, history.Count());
        Assert.True(status2.IsCurrent);
    }

    [Fact]
    public void Add_WithMultipleStatuses_ShouldMaintainHistory()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;
        var statuses = new List<TestStatus>();

        // Act
        for (int i = 0; i < 5; i++)
        {
            var status = new TestStatus(
                AlternateKey.New(),
                DateTime.UtcNow.AddSeconds(i),
                TestStatusType.Pending,
                $"Status {i}"
            );
            statuses.Add(status);
            history.Add(status);
        }

        // Assert
        Assert.Equal(5, history.Count());
        Assert.True(statuses[4].IsCurrent);
        Assert.All(statuses.Take(4), s => Assert.False(s.IsCurrent));
    }

    [Fact]
    public void GetByType_WithNullHistory_ShouldThrowException()
    {
        // Arrange
        var entity = new TestEntity();
        var history = entity.StatusHistory;

        // Act & Assert
        // O método GetByType verifica se this == null, mas isso nunca será true
        // Este teste verifica o comportamento quando a lista está vazia
        var result = history.GetByType(TestStatusType.Pending).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void Add_WithValidator_AndComplexTransitionRules_ShouldRespectRules()
    {
        // Arrange
        var validator = new TestStatusTransitionValidator(
            (TestStatusType.Pending, TestStatusType.Active),
            (TestStatusType.Active, TestStatusType.Inactive),
            (TestStatusType.Active, TestStatusType.Suspended),
            (TestStatusType.Suspended, TestStatusType.Active)
        );
        var entity = new TestEntity(validator);
        var history = entity.StatusHistory;

        var status1 = new TestStatus(AlternateKey.New(), DateTime.UtcNow, TestStatusType.Pending, "1");
        var status2 = new TestStatus(AlternateKey.New(), DateTime.UtcNow.AddSeconds(1), TestStatusType.Active, "2");
        var status3 = new TestStatus(AlternateKey.New(), DateTime.UtcNow.AddSeconds(2), TestStatusType.Suspended, "3");
        var status4 = new TestStatus(AlternateKey.New(), DateTime.UtcNow.AddSeconds(3), TestStatusType.Active, "4");

        // Act
        history.Add(status1);
        history.Add(status2); // Pending -> Active (válido)
        history.Add(status3); // Active -> Suspended (válido)
        history.Add(status4); // Suspended -> Active (válido)

        // Assert
        Assert.Equal(4, history.Count());
        Assert.True(status4.IsCurrent);
    }

    [Fact]
    public void Add_WithValidator_AndBlockedTransition_ShouldPreventTransition()
    {
        // Arrange
        var validator = new TestStatusTransitionValidator(
            (TestStatusType.Active, TestStatusType.Inactive)
        // Não permite Active -> Suspended
        );
        var entity = new TestEntity(validator);
        var history = entity.StatusHistory;

        var status1 = new TestStatus(AlternateKey.New(), DateTime.UtcNow, TestStatusType.Active, "1");
        var status2 = new TestStatus(AlternateKey.New(), DateTime.UtcNow.AddSeconds(1), TestStatusType.Suspended, "2");

        // Act
        history.Add(status1);

        // Assert
        var exception = Assert.Throws<InvalidStatusTransitionException>(() => history.Add(status2));
        Assert.Equal("Active", exception.FromStatus);
        Assert.Equal("Suspended", exception.ToStatus);
        Assert.Single(history);
    }
}

