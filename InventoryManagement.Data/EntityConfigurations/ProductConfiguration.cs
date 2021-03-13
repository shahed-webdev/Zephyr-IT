using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.Warranty).HasMaxLength(128);
            entity.Property(e => e.Note).HasMaxLength(1000);

            entity.Property(e => e.SellingPrice).HasDefaultValueSql("0");

            entity.HasOne(d => d.ProductCatalog)
                .WithMany(p => p.Product)
                .HasForeignKey(d => d.ProductCatalogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCatalog");


            entity.Property(e => e.SellingPrice)
                .HasColumnType("decimal(18, 2)");

        }
    }
}