using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class SellingAdjustmentConfiguration : IEntityTypeConfiguration<SellingAdjustment>
    {
        public void Configure(EntityTypeBuilder<SellingAdjustment> entity)
        {

                entity.Property(e => e.AdjustmentStatus)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SellingAdjustment)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellingAdjustment_Product");

                entity.HasOne(d => d.ProductStock)
                    .WithMany(p => p.SellingAdjustment)
                    .HasForeignKey(d => d.ProductStockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellingAdjustment_ProductStock");

                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.SellingAdjustment)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellingAdjustment_Registration");

                entity.HasOne(d => d.Selling)
                    .WithMany(p => p.SellingAdjustment)
                    .HasForeignKey(d => d.SellingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellingAdjustment_Selling");
            }
    }
}