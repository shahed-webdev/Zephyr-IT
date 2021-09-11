using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
    {
        public void Configure(EntityTypeBuilder<Institution> entity)
        {
            entity.Property(e => e.Address).HasMaxLength(500);

            entity.Property(e => e.City).HasMaxLength(128);

            entity.Property(e => e.DialogTitle).HasMaxLength(256);

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.Established).HasMaxLength(50);

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.InstitutionName)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.LocalArea).HasMaxLength(128);

            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.Property(e => e.PostalCode).HasMaxLength(50);

            entity.Property(e => e.State).HasMaxLength(128);

            entity.Property(e => e.Website).HasMaxLength(50);

            // entity.HasData(new Institution {InstitutionId = 1, InstitutionName = "Institution"});

            entity.Property(e => e.Capital)
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValueSql("(0.00)");

            entity.HasOne(e => e.Account)
                .WithOne(e => e.Institution)
                .HasForeignKey<Institution>(e => e.DefaultAccountId);
        }
    }
}