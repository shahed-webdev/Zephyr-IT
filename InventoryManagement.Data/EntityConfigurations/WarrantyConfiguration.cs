using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class WarrantyConfiguration : IEntityTypeConfiguration<Warranty>
    {
        public void Configure(EntityTypeBuilder<Warranty> builder)
        {
            builder.Property(e => e.AcceptanceDate)
                .HasColumnType("date");

            builder.Property(e => e.DeliveryDate)
                .HasColumnType("date");

            builder.Property(e => e.InsertDateUtc)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            builder.Property(e => e.AcceptanceDescription)
                .HasMaxLength(1024);

            builder.Property(e => e.DeliveryDescription)
                .HasMaxLength(1024);

            builder.Property(e => e.ChangedProductCode)
                .HasMaxLength(50);

            builder.Property(e => e.ChangedProductName)
                .HasMaxLength(128);

            builder.HasOne(e => e.Selling)
                .WithMany(e => e.Warranty)
                .HasForeignKey(e => e.SellingId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Warranty_Selling");

            builder.HasOne(e => e.ProductStock)
                .WithMany(e => e.Warranty)
                .HasForeignKey(e => e.ProductStockId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Warranty_ProductStock");

            builder.HasOne(e => e.ProductCatalog)
                .WithMany(e => e.Warranty)
                .HasForeignKey(e => e.ChangedProductCatalogId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Warranty_ProductCatalog");

        }
    }
}