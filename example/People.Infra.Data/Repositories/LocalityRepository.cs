using Microsoft.EntityFrameworkCore;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesInfra.Data.Postgress.Repositories;
using People.Domain.Localities.Entities;
using People.Domain.Localities.Services.Repositories;
using People.Application.Services.Repositories;
using People.Application.DTOs;
using People.Infra.Data.Context;

namespace People.Infra.Data.Repositories;

/// <summary>
/// Repository implementation for managing locality cache entities.
/// Implements both domain repository and application read repository interfaces.
/// </summary>
public class LocalityRepository : Repository<Locality, EntityId>, ILocalityRepository, ILocalityReadRepository
{
    private readonly PeopleDbContext _peopleDbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="LocalityRepository"/>.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public LocalityRepository(DbContext dbContext)
        : base(dbContext)
    {
        _peopleDbContext = (PeopleDbContext)dbContext;
    }

    /// <inheritdoc />
    public async Task<LocalityReferenceDto?> GetByKeyAsync(Guid localityKey, CancellationToken cancellationToken = default)
    {
        var locality = await (from l in _peopleDbContext.Localities
                              where l.Key.Value == localityKey
                              select l)
                            .FirstOrDefaultAsync(cancellationToken);

        if (locality == null)
            return null;

        return new LocalityReferenceDto
        {
            LocalityKey = locality.Key.Value,
            Name = GetLocalityName(locality)
        };
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<LocalityReferenceDto>> GetByTypeAsync(int localityTypeCode, CancellationToken cancellationToken = default)
    {
        var localities = await (from l in _peopleDbContext.Localities
                                where l.Type.Code == localityTypeCode
                                select l)
                               .ToListAsync(cancellationToken);

        return localities.Select(l => new LocalityReferenceDto
        {
            LocalityKey = l.Key.Value,
            Name = GetLocalityName(l)
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<LocalityReferenceDto> Localities, int TotalCount)> SearchAsync(
        string? name = null,
        int? localityTypeCode = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = from locality in _peopleDbContext.Localities
                        select locality;

        if (!string.IsNullOrWhiteSpace(name))
        {
            // Search in Name property of derived types using pattern matching
            baseQuery = baseQuery.Where(l =>
                (l is LocalityCity city && city.Name.Contains(name)) ||
                (l is LocalityCountry country && country.Name.Contains(name)) ||
                (l is LocalityState state && state.Name.Contains(name)) ||
                (l is LocalityNeighborhood neighborhood && neighborhood.Name.Contains(name)) ||
                (l is LocalityStreet street && street.Name.Contains(name)));
        }

        if (localityTypeCode.HasValue)
        {
            baseQuery = baseQuery.Where(l => l.Type.Code == localityTypeCode.Value);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var localities = await baseQuery
            .OrderBy(l => l.Id.Value)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var result = localities.Select(l => new LocalityReferenceDto
        {
            LocalityKey = l.Key.Value,
            Name = GetLocalityName(l)
        }).ToList();

        return (result, totalCount);
    }

    /// <summary>
    /// Gets the name from a locality entity using reflection if needed.
    /// </summary>
    private static string GetLocalityName(Locality locality)
    {
        // Try to get Name property via reflection
        var nameProperty = locality.GetType().GetProperty("Name");
        if (nameProperty != null && nameProperty.GetValue(locality) is string name)
        {
            return name;
        }

        // Fallback to ToString
        return locality.ToString() ?? string.Empty;
    }
}

