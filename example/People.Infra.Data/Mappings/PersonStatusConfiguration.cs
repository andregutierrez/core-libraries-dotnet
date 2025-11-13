using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using People.Domain.Persons.Statuses;
using Core.Libraries.Domain.Entities.Identifiers;

namespace People.Infra.Data.Mappings;

/// <summary>
/// Entity Framework Core configuration for the PersonStatus entity.
/// </summary>
public class PersonStatusConfiguration : IEntityTypeConfiguration<PersonStatus>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PersonStatus> builder)
    {
        builder.ToTable("PersonStatusHistory");

        // Primary key
        builder.HasKey(s => s.Id);

        // Alternate key
        builder.Property(s => s.Key)
            .HasConversion(
                k => k.Value,
                v => new AlternateKey(v))
            .HasColumnName("Key")
            .IsRequired();

        // Person relationship (foreign key)
        builder.Property<int>("PersonId")
            .HasColumnName("PersonId")
            .IsRequired();

        // CreatedAt mapping (from Status base class)
        builder.Property<DateTime>("CreatedAt")
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property<bool>("IsCurrent")
            .HasColumnName("IsCurrent")
            .IsRequired();

        // PersonStatusType (Enum) - stored as int
        // Note: PersonStatus inherits Type from Status<PersonStatusType> base class
        builder.Property<PersonStatusType>("Type")
            .HasConversion<int>()
            .HasColumnName("StatusTypeCode")
            .IsRequired();

        // Notes
        builder.Property(s => s.Notes)
            .HasColumnName("Notes")
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(s => s.Key.Value)
            .IsUnique()
            .HasDatabaseName("IX_PersonStatusHistory_Key");
    }
}

