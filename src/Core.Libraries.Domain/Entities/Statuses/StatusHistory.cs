using Core.Libraries.Domain.Exceptions;

namespace Core.Libraries.Domain.Entities.Statuses;

/// <summary>
/// Gerencia o histórico de status de uma entidade, garantindo que apenas um status ativo exista por vez.
/// </summary>
/// <typeparam name="TStatus">
/// Tipo da entidade de status, derivada de <see cref="Status{TType}"/>.
/// </typeparam>
/// <remarks>
/// <para>
/// Esta classe gerencia o histórico de status de uma entidade, permitindo rastrear todas as mudanças
/// de status ao longo do tempo. Apenas um status pode estar ativo (IsCurrent = true) por vez.
/// </para>
/// <para>
/// Classes derivadas devem implementar o método abstrato <see cref="GetStatusType"/> para permitir
/// validação de transições baseadas apenas nos tipos de status.
/// </para>
/// <para>
/// Para habilitar validação de transições, configure um validador através do método
/// <see cref="SetValidator"/> ou no construtor da classe derivada.
/// </para>
/// </remarks>
public abstract class StatusHistory<TStatus> : EntityList<TStatus>, IStatusHistory<TStatus>
    where TStatus : IStatusBase
{
    private const string LIST_NOT_LOADED = "The status list has not been initialized.";
    private const string ACTIVE_STATUS_NOT_FOUND = "No active status was found.";
    private IStatusTransitionValidator<object, Enum>? _validator;

    /// <summary>
    /// Construtor padrão. A lista interna deve ser inicializada antes do uso.
    /// </summary>
    protected StatusHistory() { }

    /// <summary>
    /// Extrai o tipo enum do status.
    /// </summary>
    /// <param name="status">A entidade de status.</param>
    /// <returns>O valor enum do status.</returns>
    /// <remarks>
    /// <para>
    /// Este método é usado para extrair o valor enum do status, permitindo que o validador
    /// trabalhe com tipos enum tipados ao invés de strings.
    /// </para>
    /// <para>
    /// Implementações devem retornar o valor da propriedade <see cref="IStatus{TType}.Type"/>.
    /// </para>
    /// </remarks>
    protected abstract Enum GetStatusType(TStatus status);

    /// <summary>
    /// Configura o validador de transição de status para este histórico.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade pai.</typeparam>
    /// <typeparam name="TStatusEnum">O tipo enum do status.</typeparam>
    /// <param name="validator">O validador a ser usado.</param>
    /// <remarks>
    /// <para>
    /// Este método permite configurar um validador de transição após a criação do histórico.
    /// Útil quando o validador não pode ser injetado no construtor.
    /// </para>
    /// <para>
    /// O validador será usado automaticamente no método <see cref="Add"/> para validar
    /// transições antes de adicionar novos status.
    /// </para>
    /// </remarks>
    protected void SetValidator<TEntity, TStatusEnum>(IStatusTransitionValidator<TEntity, TStatusEnum> validator)
        where TStatusEnum : Enum
    {
        _validator = new StatusTransitionValidatorWrapper<TEntity, TStatusEnum>(validator);
    }

    /// <summary>
    /// Registra um novo status no histórico:
    /// 1. Valida a transição se um validador estiver configurado
    /// 2. Verifica se o novo status está marcado como current
    /// 3. Desativa todos os status existentes
    /// 4. Adiciona o novo status à lista
    /// </summary>
    /// <param name="entity">A nova entidade de status a ser registrada.</param>
    /// <exception cref="NullReferenceException">
    /// Lançada se a lista interna não estiver inicializada.
    /// </exception>
    /// <exception cref="CannotSetInactiveStatusAsCurrentException">
    /// Lançada se o status não estiver marcado como current antes de ser definido como status ativo.
    /// </exception>
    /// <exception cref="InvalidStatusTransitionException">
    /// Lançada se a transição de status não for permitida pelo validador configurado.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Se um validador estiver configurado através de <see cref="SetValidator"/>,
    /// a transição será validada antes de adicionar o novo status. Se a validação falhar,
    /// uma <see cref="InvalidStatusTransitionException"/> será lançada.
    /// </para>
    /// <para>
    /// Se nenhum validador estiver configurado, o comportamento é o mesmo do sistema anterior,
    /// garantindo backward compatibility.
    /// </para>
    /// </remarks>
    public void Add(TStatus entity)
    {
        // Validar transição se validador estiver configurado
        if (_validator != null)
        {
            var currentStatus = GetCurrentOrDefault();
            if (currentStatus != null)
            {
                var fromStatus = GetStatusType(currentStatus);
                var toStatus = GetStatusType(entity);

                // Validação sem necessidade da entidade pai
                _validator.ValidateTransition(fromStatus, toStatus, null!);
            }
        }

        // Desativar status atuais
        foreach (var status in this.Where(s => s.IsCurrent))
            status.Deactivate();

        _list.Add(entity);
    }

    /// <summary>
    /// Obtém o status atual ou null se não houver nenhum.
    /// </summary>
    /// <returns>O status atual ou null.</returns>
    private TStatus? GetCurrentOrDefault()
    {
        return this.FirstOrDefault(s => s.IsCurrent);
    }

    /// <summary>
    /// Obtém a entidade de status atualmente ativa.
    /// </summary>
    /// <returns>A entidade com <c>IsCurrent == true</c>.</returns>
    /// <exception cref="NullReferenceException">
    /// Lançada se a lista não estiver inicializada ou nenhum status ativo for encontrado.
    /// </exception>
    public TStatus GetCurrent()
    {
        return this
            .SingleOrDefault(s => s.IsCurrent)
            ?? throw new NullReferenceException(ACTIVE_STATUS_NOT_FOUND);
    }

    /// <summary>
    /// Obtém todos os registros de status que correspondem ao tipo especificado.
    /// </summary>
    /// <param name="type">O valor enum para filtrar.</param>
    /// <returns>Uma sequência de entidades de status que correspondem ao tipo especificado.</returns>
    /// <exception cref="NullReferenceException">
    /// Lançada se a lista não estiver inicializada.
    /// </exception>
    public IEnumerable<TStatus> GetByType<TType>(TType type)
    {
        if (this == null)
            throw new NullReferenceException(LIST_NOT_LOADED);

        var statuses = this.OfType<IStatus<TType>>().ToList();
        return statuses
            .Where(status => status != null && Equals(status.Type, type))
            .Cast<TStatus>();
    }

    /// <summary>
    /// Wrapper interno para converter validadores genéricos específicos em validador genérico.
    /// </summary>
    private class StatusTransitionValidatorWrapper<TEntity, TStatusEnum> : IStatusTransitionValidator<object, Enum>
        where TStatusEnum : Enum
    {
        private readonly IStatusTransitionValidator<TEntity, TStatusEnum> _innerValidator;

        public StatusTransitionValidatorWrapper(IStatusTransitionValidator<TEntity, TStatusEnum> innerValidator)
        {
            _innerValidator = innerValidator;
        }

        public bool CanTransition(Enum fromStatus, Enum toStatus, object entity)
        {
            if (fromStatus is TStatusEnum from &&
                toStatus is TStatusEnum to)
            {
                var typedEntity = entity is TEntity te ? te : default(TEntity)!;
                return _innerValidator.CanTransition(from, to, typedEntity);
            }
            return false;
        }

        public void ValidateTransition(Enum fromStatus, Enum toStatus, object entity)
        {
            if (fromStatus is TStatusEnum from && toStatus is TStatusEnum to)
            {
                // Usar default(TEntity) quando entity for null
                var typedEntity = entity is TEntity te ? te : default(TEntity)!;
                _innerValidator.ValidateTransition(from, to, typedEntity);
            }
            else
            {
                throw new InvalidStatusTransitionException(
                    domainContext: typeof(TEntity).Name,
                    fromStatus: fromStatus.ToString(),
                    toStatus: toStatus.ToString()
                );
            }
        }
    }
}