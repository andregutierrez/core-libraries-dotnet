using MediatR;

namespace Core.LibrariesApplication.Queries;

/// <summary>
/// Represents a query that retrieves data without modifying application state.
/// </summary>
/// <typeparam name="TResponse">The type of data returned by the query.</typeparam>
/// <remarks>
/// This interface is used to define read-only operations in a CQRS-based architecture,
/// separating them from commands which perform state changes.
/// </remarks>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
