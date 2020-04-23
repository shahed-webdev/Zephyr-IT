using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class ProductCatalogTypeConfiguration : IEntityTypeConfiguration<ProductCatalogType>
    {
        public void Configure(EntityTypeBuilder<ProductCatalogType> entity)
        {

                entity.HasKey(e => e.CatalogTypeId);

                entity.Property(e => e.CatalogType)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
           
        }
    }
}