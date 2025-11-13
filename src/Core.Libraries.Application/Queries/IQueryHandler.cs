using MediatR;

namespace Core.LibrariesApplication.Queries;

/// <summary>
/// Represents a handler for a query that returns a response.
/// </summary>
/// <typeparam name="TRequest">The type of the query request.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the query.</typeparam>
/// <remarks>
/// This interface should be implemented by any service that executes a read-only operation
/// (e.g., fetching data from a repository or API) as part of the application's query pipeline.
/// It inherits from <see cref="IRequestHandler{TRequest, TResponse}"/> from MediatR.
/// </remarks>
public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
}
