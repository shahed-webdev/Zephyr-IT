using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class SellingPaymentListConfiguration : IEntityTypeConfiguration<SellingPaymentList>
    {
        public void Configure(EntityTypeBuilder<SellingPaymentList> entity)
        {

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Selling)
                .WithMany(p => p.SellingPaymentList)
                .HasForeignKey(d => d.SellingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellingPaymentList_Selling");

            entity.HasOne(d => d.SellingPayment)
                .WithMany(p => p.SellingPaymentList)
                .HasForeignKey(d => d.SellingPaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellingPaymentList_SellingPayment");

            entity.Property(e => e.SellingPaidAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.AccountTransactionCharge)
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValueSql("(0.00)");



        }
    }
}