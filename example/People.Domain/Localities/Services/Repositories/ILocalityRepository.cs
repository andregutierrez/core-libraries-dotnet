namespace People.Domain.Localities.Services.Repositories;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Localities.Entities;

/// <summary>
/// Repository interface for managing locality cache entities.
/// </summary>
public interface ILocalityRepository : IRepository<Locality, EntityId>
{
}

