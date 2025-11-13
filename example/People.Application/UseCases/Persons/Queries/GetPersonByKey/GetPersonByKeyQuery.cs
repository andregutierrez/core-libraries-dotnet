namespace People.Application.UseCases.Persons.Queries.GetPersonByKey;

using Core.LibrariesApplication.Queries;
using People.Application.DTOs;

/// <summary>
/// Query to get a person by its alternate key.
/// </summary>
public class GetPersonByKeyQuery : IQuery<GetPersonByKeyQueryResponse?>
{
    /// <summary>
    /// Gets the person's alternate key.
    /// </summary>
    public Guid PersonKey { get; init; }
}

/// <summary>
/// Response for the GetPersonByKeyQuery.
/// </summary>
public class GetPersonByKeyQueryResponse
{
    /// <summary>
    /// Gets the detailed person information.
    /// </summary>
    public PersonDetailDto Person { get; init; } = null!;
}

