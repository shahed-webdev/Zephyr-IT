using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> entity)
        {

            entity.Property(e => e.Due)
                .HasComputedColumnSql("(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid])) PERSISTED");

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.VendorAddress).HasMaxLength(500);

            entity.Property(e => e.VendorCompanyName)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.VendorName).HasMaxLength(128);

            entity.Property(e => e.VendorPhone).HasMaxLength(50);

            entity.Property(e => e.Description).HasMaxLength(1000);


            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalDiscount)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReturnAmount)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Paid)
                .HasColumnType("decimal(18, 2)");
        }
    }
}