namespace People.Domain.Addresses.Services.Repositories;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Addresses.Entities;

/// <summary>
/// Repository interface for managing address entities.
/// </summary>
public interface IAddressRepository : IRepository<Address, EntityId>
{
}

