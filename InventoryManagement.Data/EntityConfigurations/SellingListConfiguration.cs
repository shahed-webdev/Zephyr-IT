using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class SellingListConfiguration : IEntityTypeConfiguration<SellingList>
    {
        public void Configure(EntityTypeBuilder<SellingList> entity)
        {

                entity.HasOne(d => d.ProductStock)
                    .WithMany(p => p.SellingList)
                    .HasForeignKey(d => d.ProductStockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellingList_ProductStock");

                entity.HasOne(d => d.Selling)
                    .WithMany(p => p.SellingList)
                    .HasForeignKey(d => d.SellingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellingList_Selling");
           
        }
    }
}