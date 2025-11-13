using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using People.Domain.Persons.Entities;
using People.Domain.Persons.ValueObjects;
using Core.Libraries.Domain.Entities.Identifiers;
using System.Linq.Expressions;
using People.Domain.Persons.Entities.Identifiers;
using People.Domain.Persons.Entities.Statuses;
using Core.Libraries.Domain.Entities;

namespace People.Infra.Data.Mappings;

/// <summary>
/// Entity Framework Core configuration for the Person entity.
/// </summary>
public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");

        // Primary key
        builder.HasKey(p => p.Id);

        // Alternate key
        builder.Property(p => p.Key)
            .HasConversion(
                k => k.Value,
                v => new AlternateKey(v))
            .HasColumnName("Key")
            .IsRequired();

        // PersonName (Value Object) - Owned Entity
        builder.OwnsOne(p => p.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            name.Property(n => n.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();

            name.Property(n => n.MiddleName)
                .HasColumnName("MiddleName")
                .HasMaxLength(100);

            name.Property(n => n.SocialName)
                .HasColumnName("SocialName")
                .HasMaxLength(200);
        });

        // BirthDate (Value Object) - stored as DateOnly
        builder.Property(p => p.BirthDate)
            .HasConversion(
                bd => bd != null ? bd.Value : (DateOnly?)null,
                d => d.HasValue ? new BirthDate(d.Value) : null)
            .HasColumnName("BirthDate");

        // Gender (Enumeration) - stored as Code (int)
        builder.Property(p => p.Gender)
            .HasConversion(
                g => g != null ? g.Code : (int?)null,
                c => c.HasValue ? Gender.FromCode(c.Value) : null)
            .HasColumnName("GenderCode");

        // IdentifiersList - Collection navigation property with backing field
        builder.HasMany<PersonIdentifier>("_identifiers")
            .WithOne()
            .HasForeignKey("PersonId")
            .OnDelete(DeleteBehavior.Cascade);

        // PersonStatusHistory - Collection navigation property with backing field
        builder.HasMany<PersonStatus>("_statusHistory")
            .WithOne()
            .HasForeignKey("PersonId")
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(p => p.Key.Value)
            .IsUnique()
            .HasDatabaseName("IX_Persons_Key");
    }
}

