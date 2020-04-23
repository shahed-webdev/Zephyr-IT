using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
    {
        public void Configure(EntityTypeBuilder<ProductStock> entity)
        {

            entity.Property(e => e.ProductCode)
                .IsRequired()
                .HasMaxLength(128);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductStock)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductStock_Product");
        }
    }
}