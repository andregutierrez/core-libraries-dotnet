using Microsoft.EntityFrameworkCore;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesInfra.Data.Postgress.Repositories;
using People.Domain.Persons.Entities;
using People.Domain.Persons.Statuses;
using People.Domain.Persons.Services.Repositories;
using People.Application.Services.Repositories;
using People.Application.DTOs;
using People.Infra.Data.Context;

namespace People.Infra.Data.Repositories;

/// <summary>
/// Repository implementation for managing person entities.
/// Implements both domain repository and application read repository interfaces.
/// </summary>
public class PersonRepository : Repository<Person, EntityId>, IPersonRepository, IPersonReadRepository
{
    private readonly PeopleDbContext _peopleDbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="PersonRepository"/>.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PersonRepository(DbContext dbContext)
        : base(dbContext)
    {
        _peopleDbContext = (PeopleDbContext)dbContext;
    }

    /// <inheritdoc />
    public async Task<PersonDetailDto?> GetDetailByKeyAsync(Guid personKey, CancellationToken cancellationToken = default)
    {
        var result = await (from person in _peopleDbContext.Persons
                            where person.Key.Value == personKey
                            select new PersonDetailDto
                            {
                                PersonKey = person.Key.Value,
                                FirstName = person.Name.FirstName,
                                LastName = person.Name.LastName,
                                MiddleName = person.Name.MiddleName,
                                SocialName = person.Name.SocialName,
                                FullName = person.Name.FullName,
                                DisplayName = person.Name.DisplayName,
                                BirthDate = person.BirthDate != null ? person.BirthDate.Value : null,
                                GenderCode = person.Gender != null ? person.Gender.Code : null,
                                CurrentStatusType = person.CurrentStatus != null ? (int)person.CurrentStatus : null,
                                IsActive = person.IsActive,
                                IsInactive = person.IsInactive,
                                IsMerged = person.IsMerged
                            })
                           .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<PersonSummaryDto> Persons, int TotalCount)> SearchSummariesAsync(
        string? name = null,
        int? statusType = null,
        bool? isActive = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = from person in _peopleDbContext.Persons
                        select person;

        // Apply filters
        if (!string.IsNullOrWhiteSpace(name))
        {
            baseQuery = baseQuery.Where(p =>
                p.Name.FirstName.Contains(name) ||
                p.Name.LastName.Contains(name) ||
                (p.Name.MiddleName != null && p.Name.MiddleName.Contains(name)) ||
                (p.Name.SocialName != null && p.Name.SocialName.Contains(name)));
        }

        if (statusType.HasValue)
        {
            var statusEnum = (PersonStatusType)statusType.Value;
            baseQuery = baseQuery.Where(p => p.CurrentStatus == statusEnum);
        }

        if (isActive.HasValue)
        {
            baseQuery = baseQuery.Where(p => p.IsActive == isActive.Value);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var result = await baseQuery
            .OrderBy(p => p.Name.LastName)
            .ThenBy(p => p.Name.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PersonSummaryDto
            {
                PersonKey = p.Key.Value,
                DisplayName = p.Name.DisplayName,
                CurrentStatusType = p.CurrentStatus != null ? (int)p.CurrentStatus : null,
                IsActive = p.IsActive
            })
            .ToListAsync(cancellationToken);

        return (result, totalCount);
    }
}

