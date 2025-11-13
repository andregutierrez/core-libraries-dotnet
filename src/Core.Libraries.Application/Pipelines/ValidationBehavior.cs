using FluentValidation;
using MediatR;
using Core.LibrariesDomain.Notifications;

namespace Core.LibrariesApplication.Pipelines;

/// <summary>
/// Defines a validation behavior for MediatR requests that do not return a value (void/Unit).
/// </summary>
/// <typeparam name="TRequest">The type of request being validated.</typeparam>
/// <remarks>
/// In case of validation failure, the notifications are added to <see cref="INotificationContext"/> and
/// the request processing is short-circuited by returning <see cref="Unit.Value"/>.
/// </remarks>
public abstract class ValidationBehavior<TRequest> : AbstractValidator<TRequest>, IPipelineBehavior<TRequest, Unit>
    where TRequest : IRequest
{
    private readonly INotificationContext _notificationContext;

    /// <summary>
    /// Initializes the validator with a shared notification context.
    /// </summary>
    /// <param name="notificationContext">The notification context to which validation errors are published.</param>
    protected ValidationBehavior(INotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    /// <summary>
    /// Handles validation for the given request. If valid, proceeds to the next pipeline delegate; otherwise,
    /// adds errors to the notification context and terminates the request.
    /// </summary>
    public async Task<Unit> Handle(TRequest request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
    {
        var retorno = await ValidateAsync(request, cancellationToken);

        if (retorno.IsValid)
        {
            return await next();
        }
        else
        {
            _notificationContext.AddNotifications(retorno);
            return Unit.Value;
        }
    }
}

/// <summary>
/// Defines a validation behavior for MediatR requests that return a typed response.
/// </summary>
/// <typeparam name="TRequest">The type of request being validated.</typeparam>
/// <typeparam name="TResponse">The type of response expected from the request.</typeparam>
/// <remarks>
/// In case of validation failure, the notifications are added to <see cref="INotificationContext"/> and
/// the pipeline returns the default response value.
/// </remarks>
public abstract class ApplicationRequestValidator<TRequest, TResponse> : AbstractValidator<TRequest>, IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly INotificationContext _notificationContext;

    /// <summary>
    /// Initializes the validator with a shared notification context.
    /// </summary>
    /// <param name="notificationContext">The notification context to which validation errors are published.</param>
    protected ApplicationRequestValidator(INotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    /// <summary>
    /// Handles validation for the given request. If valid, continues the pipeline; otherwise,
    /// adds notifications and returns the default value for <typeparamref name="TResponse"/>.
    /// </summary>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var retorno = await ValidateAsync(request, cancellationToken);

        if (retorno.IsValid)
        {
            return await next();
        }
        else
        {
            _notificationContext.AddNotifications(retorno);
            return default!;
        }
    }
}
