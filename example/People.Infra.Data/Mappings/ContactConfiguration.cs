using Core.Libraries.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using People.Domain.Contacts.Entities;
using People.Domain.Contacts.ValueObjects;

namespace People.Infra.Data.Mappings;

/// <summary>
/// Entity Framework Core configuration for the Contact entity.
/// Uses Table Per Hierarchy (TPH) pattern for EmailContact, PhoneContact, and SocialMediaContact.
/// </summary>
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        // Primary key
        builder.HasKey(c => c.Id);

        // Alternate key
        builder.Property(c => c.Key)
            .HasConversion(
                k => k.Value,
                v => new AlternateKey(v))
            .HasColumnName("Key")
            .IsRequired();

        // Person relationship (foreign key)
        builder.Property(c => c.PersonId)
            .HasConversion(
                id => id.Value,
                v => new EntityId(v))
            .HasColumnName("PersonId")
            .IsRequired();

        // ContactType (Enumeration) - stored as Code (int)
        builder.Property(c => c.Type)
            .HasConversion(
                t => t.Code,
                c => ContactType.FromCode(c) ?? ContactType.Phone)
            .HasColumnName("ContactTypeCode")
            .IsRequired();

        // Discriminator for TPH
        builder.HasDiscriminator<int>("ContactType")
            .HasValue<EmailContact>(1) // Email
            .HasValue<PhoneContact>(2) // Phone, Mobile, WhatsApp
            .HasValue<SocialMediaContact>(5); // SocialMedia

        // Simple properties
        builder.Property(c => c.IsPrimary)
            .HasColumnName("IsPrimary")
            .IsRequired();

        builder.Property(c => c.Notes)
            .HasMaxLength(500)
            .HasColumnName("Notes");

        //// EmailContact specific properties
        //builder.Entity<EmailContact>()
        //    .OwnsOne(e => e.Email, email =>
        //    {
        //        email.Property(e => e.Value)
        //            .HasColumnName("Email")
        //            .HasMaxLength(255)
        //            .IsRequired();
        //    });

        //// PhoneContact specific properties
        //builder.Entity<PhoneContact>()
        //    .OwnsOne(p => p.Phone, phone =>
        //    {
        //        phone.Property(p => p.Value)
        //            .HasColumnName("PhoneNumber")
        //            .HasMaxLength(20)
        //            .IsRequired();

        //        phone.Property(p => p.Formatted)
        //            .HasColumnName("PhoneFormatted")
        //            .HasMaxLength(50)
        //            .IsRequired();
        //    });

        //// SocialMediaContact specific properties
        //builder.Entity<SocialMediaContact>()
        //    .OwnsOne(s => s.SocialMedia, social =>
        //    {
        //        social.Property(s => s.Platform)
        //            .HasConversion(
        //                p => p.Code,
        //                c => SocialMediaPlatform.FromCode(c) ?? throw new InvalidOperationException($"Invalid SocialMediaPlatform code: {c}"))
        //            .HasColumnName("SocialMediaPlatformCode")
        //            .IsRequired();

        //        social.Property(s => s.Username)
        //            .HasColumnName("SocialMediaUsername")
        //            .HasMaxLength(100)
        //            .IsRequired();

        //        social.Property(s => s.ProfileUrl)
        //            .HasColumnName("SocialMediaProfileUrl")
        //            .HasMaxLength(500);
        //    });

        // Indexes
        builder.HasIndex(c => c.Key.Value)
            .IsUnique()
            .HasDatabaseName("IX_Contacts_Key");

        builder.HasIndex(c => c.PersonId.Value)
            .HasDatabaseName("IX_Contacts_PersonId");

        builder.HasIndex(c => new { c.PersonId.Value, c.Type.Code, c.IsPrimary })
            .HasDatabaseName("IX_Contacts_PersonId_Type_IsPrimary");
    }
}

