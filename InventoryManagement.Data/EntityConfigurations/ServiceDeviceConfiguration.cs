using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class ServiceDeviceConfiguration : IEntityTypeConfiguration<ServiceDevice>
    {
        public void Configure(EntityTypeBuilder<ServiceDevice> entity)
        {

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.DeviceCode)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ServiceDevice)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceDevice_Customer");

                entity.HasOne(d => d.ProductCatalog)
                    .WithMany(p => p.ServiceDevice)
                    .HasForeignKey(d => d.ProductCatalogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceDevice_ProductCatalog");

                
    }
    }
}