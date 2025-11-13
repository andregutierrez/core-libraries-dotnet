namespace People.Application.UseCases.Persons.Queries.GetPersonByKey;

using Core.LibrariesApplication.Queries;
using People.Application.DTOs;
using People.Application.Services.Repositories;

/// <summary>
/// Handler for the GetPersonByKeyQuery.
/// </summary>
public class GetPersonByKeyQueryHandler : IQueryHandler<GetPersonByKeyQuery, GetPersonByKeyQueryResponse?>
{
    private readonly IPersonReadRepository _personReadRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="GetPersonByKeyQueryHandler"/>.
    /// </summary>
    /// <param name="personReadRepository">The person read repository.</param>
    public GetPersonByKeyQueryHandler(IPersonReadRepository personReadRepository)
    {
        _personReadRepository = personReadRepository ?? throw new ArgumentNullException(nameof(personReadRepository));
    }

    /// <summary>
    /// Handles the GetPersonByKeyQuery.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed person information, or null if not found.</returns>
    public async Task<GetPersonByKeyQueryResponse?> Handle(
        GetPersonByKeyQuery request,
        CancellationToken cancellationToken)
    {
        var person = await _personReadRepository.GetDetailByKeyAsync(request.PersonKey, cancellationToken);

        if (person == null)
            return null;

        return new GetPersonByKeyQueryResponse
        {
            Person = person
        };
    }
}

