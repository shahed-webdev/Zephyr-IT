using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class PurchaseListConfiguration : IEntityTypeConfiguration<PurchaseList>
    {
        public void Configure(EntityTypeBuilder<PurchaseList> entity)
        {
            entity.Property(e => e.Warranty).HasMaxLength(128);
            entity.Property(e => e.Note).HasMaxLength(1000);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.SellingPrice).HasDefaultValueSql("0");
            entity.Property(e => e.PurchasePrice).HasDefaultValueSql("0");

            entity.HasOne(pl => pl.Product)
                .WithMany(p => p.PurchaseList)
                .HasForeignKey(pl => pl.ProductId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_PurchaseList_Product");

            entity.HasOne(pl => pl.Purchase)
                .WithMany(p => p.PurchaseList)
                .HasForeignKey(pl => pl.PurchaseId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_PurchaseList_Purchase");


            entity.Property(e => e.SellingPrice)
                .HasColumnType("decimal(18, 2)");


            entity.Property(e => e.PurchasePrice)
                .HasColumnType("decimal(18, 2)");
        }
    }
}