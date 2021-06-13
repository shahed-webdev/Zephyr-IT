using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class ProductDamagedConfiguration : IEntityTypeConfiguration<ProductDamaged>
    {
        public void Configure(EntityTypeBuilder<ProductDamaged> builder)
        {
            builder.Property(e => e.Note)
                .HasMaxLength(1024);

            builder.Property(e => e.InsertDateUtc)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            builder.Property(e => e.DamagedDate)
                .HasColumnType("date");

            builder.HasOne(e => e.ProductStock)
                .WithOne(e => e.ProductDamaged)
                .HasForeignKey<ProductDamaged>(e => e.ProductStockId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductDamaged_ProductStock");

        }
    }
}