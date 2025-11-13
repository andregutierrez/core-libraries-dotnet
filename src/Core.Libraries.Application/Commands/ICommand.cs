using MediatR;
using System.Text.Json.Serialization;

namespace Core.Libraries.Application.Commands;

/// <summary>
/// Represents a command that does not return a result upon execution.
/// </summary>
/// <remarks>
/// Use this interface to define fire-and-forget operations that cause a state change,
/// such as creating, updating, or deleting entities.
/// </remarks>
public interface ICommand : IRequest
{
}

/// <summary>
/// Represents a command that returns a result upon execution.
/// </summary>
/// <typeparam name="TResponse">The type of response produced by the command.</typeparam>
/// <remarks>
/// Use this interface when executing commands that both modify application state and return a result,
/// such as creating an entity and returning its identifier or projection.
/// </remarks>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
