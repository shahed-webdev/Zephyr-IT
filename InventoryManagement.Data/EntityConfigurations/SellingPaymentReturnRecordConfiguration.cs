using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class SellingPaymentReturnRecordConfiguration : IEntityTypeConfiguration<SellingPaymentReturnRecord>
    {
        public void Configure(EntityTypeBuilder<SellingPaymentReturnRecord> entity)
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
                .WithMany(p => p.SellingPaymentReturnRecord)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SellingPaymentReturnRecord_Account");

            entity.HasOne(d => d.Selling)
                .WithMany(p => p.SellingPaymentReturnRecord)
                .HasForeignKey(d => d.SellingId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SellingPaymentReturnRecord_Selling");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.SellingPaymentReturnRecord)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellingPaymentReturnRecord_Registration");
        }
    }
}