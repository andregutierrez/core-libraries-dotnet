using MediatR;

namespace Core.LibrariesApplication.Commands;

/// <summary>
/// Represents a handler for a command that returns a response.
/// </summary>
/// <typeparam name="TRequest">The type of the command request.</typeparam>
/// <typeparam name="TResponse">The type of the response returned after handling the command.</typeparam>
/// <remarks>
/// This interface should be implemented by any application service or use case handler that
/// executes a command and produces a result. It inherits from <see cref="IRequestHandler{TRequest, TResponse}"/>.
/// </remarks>
public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{ }

/// <summary>
/// Represents a handler for a command that does not return a response (fire-and-forget or unit).
/// </summary>
/// <typeparam name="TRequest">The type of the command request.</typeparam>
/// <remarks>
/// Use this interface when the command execution does not produce a result and only has side effects.
/// It inherits from <see cref="IRequestHandler{TRequest}"/> and typically maps to <see cref="Unit"/> as a response.
/// </remarks>
public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : ICommand
{ }
