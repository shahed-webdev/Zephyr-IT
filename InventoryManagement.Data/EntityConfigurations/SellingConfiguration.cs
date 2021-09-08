using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class SellingConfiguration : IEntityTypeConfiguration<Selling>
    {
        public void Configure(EntityTypeBuilder<Selling> entity)
        {

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.LastUpdateDate)
                .HasColumnType("date");

            entity.Property(e => e.PromisedPaymentDate)
                .HasColumnType("date");

            entity.Property(e => e.SellingDate)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.SellingDiscountPercentage)
                .HasColumnType("decimal(18, 2)")
                .HasComputedColumnSql("(case when [SellingTotalPrice]=(0.00) then (0.00) else ([SellingDiscountAmount]*(100.00))/[SellingTotalPrice] end) PERSISTED");

            entity.Property(e => e.SellingDueAmount)
                .HasComputedColumnSql("(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount]+[AccountTransactionCharge])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount])) PERSISTED")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SellingPaymentStatus)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount]+[AccountTransactionCharge])-([SellingDiscountAmount]+[SellingPaidAmount]+[PurchaseAdjustedAmount]))<=(0.00) then 'Paid' else 'Due' end) PERSISTED");

            entity.Property(e => e.SellingSn)
                .HasColumnName("SellingSN");

            entity.Property(e => e.SellingTotalPrice)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SellingDiscountAmount)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SellingPaidAmount)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SellingReturnAmount)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Selling)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Selling_Customer");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.Selling)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Selling_Registration");

            entity.Property(e => e.ServiceCharge)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.ServiceCost)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.ServiceChargeDescription)
                .HasMaxLength(1024);

            entity.Property(e => e.ExpenseTotal)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.AccountTransactionCharge)
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValueSql("(0.00)");

            entity.Property(e => e.BuyingTotalPrice)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.SellingAccountCost)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.ServiceProfit)
                .HasComputedColumnSql("([ServiceCharge]-[ServiceCost]) PERSISTED")
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.SellingProfit)
                .HasColumnType("decimal(18, 2)")
                .HasComputedColumnSql("([SellingTotalPrice]-[BuyingTotalPrice]) PERSISTED");

            entity.Property(e => e.SellingNetProfit)
                .HasColumnType("decimal(18, 2)")
                .HasComputedColumnSql("(([SellingTotalPrice]+[AccountTransactionCharge])-([BuyingTotalPrice]+[SellingAccountCost]+[ExpenseTotal])) PERSISTED");

            entity.Property(e => e.GrandProfit)
                .HasColumnType("decimal(18, 2)")
                .HasComputedColumnSql("((([SellingTotalPrice]+[AccountTransactionCharge])-([BuyingTotalPrice]+[SellingDiscountAmount]+[SellingAccountCost]+[ExpenseTotal]))+([ServiceCharge]-[ServiceCost])) PERSISTED");

            entity.Property(e => e.PurchaseAdjustedAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.PurchaseDescription)
                .HasMaxLength(1024);

            entity.HasOne(e => e.Purchase)
                .WithOne(e => e.Selling)
                .HasForeignKey<Selling>(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Selling_Purchase");
        }
    }
}