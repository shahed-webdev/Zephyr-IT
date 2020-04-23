using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class ProductCatalogConfiguration : IEntityTypeConfiguration<ProductCatalog>
    {
        public void Configure(EntityTypeBuilder<ProductCatalog> entity)
        {

            entity.Property(e => e.CatalogName)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CatalogType)
                .WithMany(p => p.ProductCatalog)
                .HasForeignKey(d => d.CatalogTypeId)
                .HasConstraintName("FK_ProductCatalog_ProductCatalogType");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductCatalog_ProductCatalog");
        }
    }
}