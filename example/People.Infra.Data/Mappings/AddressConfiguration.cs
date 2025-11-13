using Core.Libraries.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using People.Domain.Addresses.Entities;
using People.Domain.Addresses.ValueObjects;

namespace People.Infra.Data.Mappings;

/// <summary>
/// Entity Framework Core configuration for the Address entity.
/// </summary>
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        // Primary key
        builder.HasKey(a => a.Id);

        // Alternate key
        builder.Property(a => a.Key)
            .HasConversion(
                k => k.Value,
                v => new AlternateKey(v))
            .HasColumnName("Key")
            .IsRequired();

        // Person relationship (foreign key)
        builder.Property(a => a.PersonId)
            .HasConversion(
                id => id.Value,
                v => new EntityId(v))
            .HasColumnName("PersonId")
            .IsRequired();

        // AddressType (Enumeration) - stored as Code (int)
        builder.Property(a => a.Type)
            .HasConversion(
                t => t.Code,
                c => AddressType.FromCode(c) ?? AddressType.Other)
            .HasColumnName("AddressTypeCode")
            .IsRequired();

        // Locality entity IDs
        builder.Property(a => a.StreetId)
            .HasConversion(
                id => id.Value,
                v => new EntityId(v))
            .HasColumnName("StreetId")
            .IsRequired();

        builder.Property(a => a.CityId)
            .HasConversion(
                id => id.Value,
                v => new EntityId(v))
            .HasColumnName("CityId")
            .IsRequired();

        builder.Property(a => a.NeighborhoodId)
            .HasConversion(
                id => id != null ? id.Value : (int?)null,
                v => v.HasValue ? new EntityId(v.Value) : null)
            .HasColumnName("NeighborhoodId");

        builder.Property(a => a.StateId)
            .HasConversion(
                id => id != null ? id.Value : (int?)null,
                v => v.HasValue ? new EntityId(v.Value) : null)
            .HasColumnName("StateId");

        builder.Property(a => a.CountryId)
            .HasConversion(
                id => id != null ? id.Value : (int?)null,
                v => v.HasValue ? new EntityId(v.Value) : null)
            .HasColumnName("CountryId");

        builder.Property(a => a.PostalCodeId)
            .HasConversion(
                id => id != null ? id.Value : (int?)null,
                v => v.HasValue ? new EntityId(v.Value) : null)
            .HasColumnName("PostalCodeId");

        // Simple properties
        builder.Property(a => a.Number)
            .HasMaxLength(20)
            .HasColumnName("Number");

        builder.Property(a => a.Complement)
            .HasMaxLength(100)
            .HasColumnName("Complement");

        builder.Property(a => a.IsPrimary)
            .HasColumnName("IsPrimary")
            .IsRequired();

        builder.Property(a => a.Notes)
            .HasMaxLength(500)
            .HasColumnName("Notes");

        // Indexes
        builder.HasIndex(a => a.Key.Value)
            .IsUnique()
            .HasDatabaseName("IX_Addresses_Key");

        builder.HasIndex(a => a.PersonId.Value)
            .HasDatabaseName("IX_Addresses_PersonId");

        builder.HasIndex(a => new { a.PersonId.Value, a.IsPrimary })
            .HasDatabaseName("IX_Addresses_PersonId_IsPrimary");
    }
}

