using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class PurchasePaymentListConfiguration : IEntityTypeConfiguration<PurchasePaymentList>
    {
        public void Configure(EntityTypeBuilder<PurchasePaymentList> entity)
        {

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchasePaymentList)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchasePaymentList_Purchase");

                entity.HasOne(d => d.PurchasePayment)
                    .WithMany(p => p.PurchasePaymentList)
                    .HasForeignKey(d => d.PurchasePaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchasePaymentList_PurchasePayment");
         
        }
    }
}