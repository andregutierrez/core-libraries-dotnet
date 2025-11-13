namespace People.Domain.Persons.Services.Repositories;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Persons.Entities;

/// <summary>
/// Repository interface for managing person entities.
/// </summary>
public interface IPersonRepository : IRepository<Person, EntityId>
{
}

