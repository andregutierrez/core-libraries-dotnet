using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Libraries.Domain.Entities.Identifiers;
using People.Domain.Persons.Entities.Identifiers;
using Core.Libraries.Domain.Entities;

namespace People.Infra.Data.Mappings;

/// <summary>
/// Entity Framework Core configuration for the PersonIdentifier entity.
/// </summary>
public class PersonIdentifierConfiguration : IEntityTypeConfiguration<PersonIdentifier>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PersonIdentifier> builder)
    {
        builder.ToTable("PersonIdentifiers");

        // Primary key
        builder.HasKey(i => i.Id);

        // Alternate key
        builder.Property(i => i.Key)
            .HasConversion(
                k => k.Value,
                v => new AlternateKey(v))
            .HasColumnName("Key")
            .IsRequired();

        // Person relationship (foreign key)
        builder.Property<int>("PersonId")
            .HasColumnName("PersonId")
            .IsRequired();

        // IdentifierType (Enumeration) - stored as Code (int)
        builder.Property(i => i.Type)
            .HasConversion(
                t => t.Code,
                c => IdentifierType.FromCode(c) ?? IdentifierType.OpenAIPlatform)
            .HasColumnName("IdentifierTypeCode")
            .IsRequired();

        // ExternalId
        builder.Property(i => i.ExternalId)
            .HasColumnName("ExternalId")
            .HasMaxLength(200)
            .IsRequired();

        // Indexes
        builder.HasIndex(i => i.Key.Value)
            .IsUnique()
            .HasDatabaseName("IX_PersonIdentifiers_Key");
    }
}

