namespace People.Domain.Contacts.Services.Repositories;

using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Contacts.Entities;

/// <summary>
/// Repository interface for managing contact entities.
/// </summary>
public interface IContactRepository : IRepository<Contact, EntityId>
{
}

