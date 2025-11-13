using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using People.Domain.Localities.Entities;
using People.Domain.Localities.ValueObjects;
using Core.Libraries.Domain.Entities.Identifiers;

namespace People.Infra.Data.Mappings;

/// <summary>
/// Entity Framework Core configuration for the Locality entity.
/// Uses Table Per Hierarchy (TPH) pattern for LocalityCity, LocalityCountry, LocalityState, LocalityNeighborhood, and LocalityStreet.
/// </summary>
public class LocalityConfiguration : IEntityTypeConfiguration<Locality>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Locality> builder)
    {
        builder.ToTable("Localities");

        // Primary key
        builder.HasKey(l => l.Id);

        // Alternate key
        builder.Property(l => l.Key)
            .HasConversion(
                k => k.Value,
                v => new AlternateKey(v))
            .HasColumnName("Key")
            .IsRequired();

        // LocalityType (Enumeration) - stored as Code (int)
        builder.Property(l => l.Type)
            .HasConversion(
                t => t.Code,
                c => LocalityType.FromCode(c) ?? throw new InvalidOperationException($"Invalid LocalityType code: {c}"))
            .HasColumnName("LocalityTypeCode")
            .IsRequired();

        // Discriminator for TPH
        builder.HasDiscriminator<int>("LocalityType")
            .HasValue<LocalityCity>(1) // City
            .HasValue<LocalityCountry>(2) // Country
            .HasValue<LocalityState>(3) // State
            .HasValue<LocalityNeighborhood>(4) // Neighborhood
            .HasValue<LocalityStreet>(5); // Street

        // Common properties
        builder.Property(l => l.Metadata)
            .HasMaxLength(2000)
            .HasColumnName("Metadata");

        builder.Property(l => l.CachedAt)
            .HasColumnName("CachedAt")
            .IsRequired();

        // LocalityCity specific properties
        builder.Entity<LocalityCity>()
            .Property(c => c.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        // LocalityCountry specific properties
        builder.Entity<LocalityCountry>()
            .Property(c => c.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        // LocalityState specific properties
        builder.Entity<LocalityState>()
            .Property(s => s.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        // LocalityNeighborhood specific properties
        builder.Entity<LocalityNeighborhood>()
            .Property(n => n.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        // LocalityStreet specific properties
        builder.Entity<LocalityStreet>()
            .Property(s => s.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        // Indexes
        builder.HasIndex(l => l.Key.Value)
            .IsUnique()
            .HasDatabaseName("IX_Localities_Key");

        builder.HasIndex(l => l.Type.Code)
            .HasDatabaseName("IX_Localities_Type");
    }
}

