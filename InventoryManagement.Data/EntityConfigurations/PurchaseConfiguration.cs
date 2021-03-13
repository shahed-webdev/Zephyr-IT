using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> entity)
        {

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MemoNumber).HasMaxLength(50);

            entity.Property(e => e.PurchaseDate)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PurchaseDiscountPercentage)
                .HasComputedColumnSql("(case when [PurchaseTotalPrice]=(0) then (0) else ([PurchaseDiscountAmount]*(100))/[PurchaseTotalPrice] end) PERSISTED");

            entity.Property(e => e.PurchaseDueAmount)
                .HasComputedColumnSql("(([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount])) PERSISTED");

            entity.Property(e => e.PurchasePaymentStatus)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql( "(case when (([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED");

            entity.Property(e => e.PurchaseSn).HasColumnName("PurchaseSN");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.Purchase)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_Registration");

            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.Purchase)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_Vendor");

            entity.Property(e => e.PurchaseTotalPrice)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PurchaseDiscountAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.PurchasePaidAmount)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PurchaseReturnAmount)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PurchaseDueAmount)
                .HasColumnType("decimal(18, 2)");

        }
    }
}