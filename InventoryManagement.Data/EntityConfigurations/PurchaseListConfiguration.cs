using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class PurchaseListConfiguration : IEntityTypeConfiguration<PurchaseList>
    {
        public void Configure(EntityTypeBuilder<PurchaseList> entity)
        {
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
        }
    }
}