using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using People.Infra.Data.Context;
using People.Infra.Data.Repositories;
using People.Domain.Addresses.Services.Repositories;
using People.Domain.Contacts.Services.Repositories;
using People.Domain.Localities.Services.Repositories;
using People.Domain.Persons.Services.Repositories;
using People.Application.Services.Repositories;
using People.Application.DTOs;
using Core.LibrariesDomain.Services.Repositories;
using Core.LibrariesInfra.Data.Postgress.UoW;

namespace People.IoC;

/// <summary>
/// Extension methods for configuring dependency injection for the People domain.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all People domain services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddPeopleServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================================================================
        // DATABASE CONTEXT
        // ============================================================================
        // Configures the Entity Framework Core DbContext for the People domain.
        // Uses PostgreSQL as the database provider.
        services.AddDbContext<PeopleDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("PeopleDb")
                ?? throw new InvalidOperationException("Connection string 'PeopleDb' not found in configuration.");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(PeopleDbContext).Assembly.FullName);
            });

            // Enable sensitive data logging in development
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // ============================================================================
        // UNIT OF WORK
        // ============================================================================
        // Manages database transactions and ensures consistent commit/rollback behavior.
        services.AddScoped<IUnitOfWork>(provider =>
        {
            var dbContext = provider.GetRequiredService<PeopleDbContext>();
            return new UnitOfWork(dbContext);
        });

        // ============================================================================
        // DOMAIN REPOSITORIES
        // ============================================================================
        // Repositories for domain operations (write operations, business logic).
        // These repositories work with domain entities and support transactions.
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ILocalityRepository, LocalityRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();

        // ============================================================================
        // APPLICATION READ REPOSITORIES
        // ============================================================================
        // Read-only repositories optimized for queries that return DTOs directly.
        // These use the same concrete implementations as domain repositories but expose
        // a different interface focused on read operations with DTOs.
        services.AddScoped<IAddressReadRepository>(provider =>
            provider.GetRequiredService<AddressRepository>());
        services.AddScoped<IContactReadRepository>(provider =>
            provider.GetRequiredService<ContactRepository>());
        services.AddScoped<ILocalityReadRepository>(provider =>
            provider.GetRequiredService<LocalityRepository>());
        services.AddScoped<IPersonReadRepository>(provider =>
            provider.GetRequiredService<PersonRepository>());

        // ============================================================================
        // MEDIATR - COMMAND & QUERY HANDLERS
        // ============================================================================
        // Registers all MediatR handlers (ICommandHandler and IQueryHandler) from
        // the People.Application assembly. This enables CQRS pattern implementation.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(People.Application.DTOs.AddressDetailDto).Assembly));

        return services;
    }
}

