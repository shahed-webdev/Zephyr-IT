using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class SellingPaymentConfiguration : IEntityTypeConfiguration<SellingPayment>
    {
        public void Configure(EntityTypeBuilder<SellingPayment> entity)
        {

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PaidDate)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            entity.Property(e => e.ReceiptSn).HasColumnName("ReceiptSN");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.SellingPayment)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellingPayment_Customer");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.SellingPayment)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellingPayment_Registration");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.SellingPayment)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SellingPayment_Account");

            entity.Property(e => e.AccountTransactionCharge)
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValueSql("(0.00)");

            entity.Property(e => e.PaidAmount)
                    .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.AccountCostPercentage)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.AccountCost)
                .HasComputedColumnSql("([PaidAmount] * ([AccountCostPercentage]/100)) PERSISTED");
        }
    }
}