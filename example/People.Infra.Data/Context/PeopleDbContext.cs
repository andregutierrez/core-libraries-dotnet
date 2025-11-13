using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MediatR;
using People.Domain.Addresses.Entities;
using People.Domain.Contacts.Entities;
using People.Domain.Localities.Entities;
using People.Domain.Persons.Entities;
using Core.Libraries.Infra.Data.Context;

namespace People.Infra.Data.Context;

/// <summary>
/// Entity Framework Core database context for the People domain.
/// </summary>
public class PeopleDbContext : CoreDbContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="PeopleDbContext"/>.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="options">The database context options.</param>
    /// <param name="mediator">Optional mediator for domain events.</param>
    public PeopleDbContext(
        ILogger<PeopleDbContext> logger,
        DbContextOptions<PeopleDbContext> options,
        IMediator? mediator = null)
        : base(logger, options, mediator)
    {
    }

    /// <summary>
    /// Gets the DbSet for addresses.
    /// </summary>
    public DbSet<Address> Addresses => Set<Address>();

    /// <summary>
    /// Gets the DbSet for contacts.
    /// </summary>
    public DbSet<Contact> Contacts => Set<Contact>();

    /// <summary>
    /// Gets the DbSet for localities.
    /// </summary>
    public DbSet<Locality> Localities => Set<Locality>();

    /// <summary>
    /// Gets the DbSet for persons.
    /// </summary>
    public DbSet<Person> Persons => Set<Person>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PeopleDbContext).Assembly);
    }
}

