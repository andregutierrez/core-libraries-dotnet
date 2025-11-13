namespace People.Domain.Addresses.Services.Repositories;

using Core.Libraries.Domain.Entities;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Addresses.Entities;

/// <summary>
/// Repository interface for managing address entities.
/// </summary>
public interface IAddressRepository : IRepository<Address, EntityId>
{
}

