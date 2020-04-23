using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class PurchaseAdjustmentConfiguration : IEntityTypeConfiguration<PurchaseAdjustment>
    {
        public void Configure(EntityTypeBuilder<PurchaseAdjustment> entity)
        {

                entity.Property(e => e.AdjustmentStatus)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PurchaseAdjustment)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseAdjustment_Product");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchaseAdjustment)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseAdjustment_Purchase");
        
        }
    }
}