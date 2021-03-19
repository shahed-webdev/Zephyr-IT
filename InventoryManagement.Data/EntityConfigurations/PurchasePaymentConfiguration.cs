using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class PurchasePaymentConfiguration : IEntityTypeConfiguration<PurchasePayment>
    {
        public void Configure(EntityTypeBuilder<PurchasePayment> entity)
        {

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PaidDate)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            entity.Property(e => e.ReceiptSn).HasColumnName("ReceiptSN");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.PurchasePayment)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePayment_Registration");

            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.PurchasePayment)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePayment_Vendor");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.PurchasePayment)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PurchasePayment_Account");
        }
    }
}