using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class PurchasePaymentReturnRecordConfiguration : IEntityTypeConfiguration<PurchasePaymentReturnRecord>
    {
        public void Configure(EntityTypeBuilder<PurchasePaymentReturnRecord> entity)
        {
            entity.Property(e => e.PrevReturnAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.CurrentReturnAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.InsertDateUtc)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            entity.Property(e => e.NetReturnAmount)
                .HasComputedColumnSql("([CurrentReturnAmount]-[PrevReturnAmount]) PERSISTED")
                .HasColumnType("decimal(18, 2)");
            entity.HasOne(d => d.Account)
                .WithMany(p => p.PurchasePaymentReturnRecord)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PurchasePaymentReturnRecord_Account");

            entity.HasOne(d => d.Purchase)
                .WithMany(p => p.PurchasePaymentReturnRecord)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PurchasePaymentReturnRecord_Purchase");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.PurchasePaymentReturnRecord)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePaymentReturnRecord_Registration");
        }
    }
}