using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> entity)
        {
            entity.Property(e => e.Due)
                .HasComputedColumnSql("(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]))");

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.VendorAddress).HasMaxLength(500);

            entity.Property(e => e.VendorCompanyName)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.VendorName).HasMaxLength(128);

            entity.Property(e => e.VendorPhone).HasMaxLength(50);

        }
    }
}