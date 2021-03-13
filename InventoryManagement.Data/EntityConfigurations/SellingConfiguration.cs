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
                .HasComputedColumnSql("(case when [SellingTotalPrice]=(0) then (0) else ([SellingDiscountAmount]*(100))/[SellingTotalPrice] end) PERSISTED");

            entity.Property(e => e.SellingDueAmount)
                .HasComputedColumnSql("(([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount])) PERSISTED");

            entity.Property(e => e.SellingPaymentStatus)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("(case when (([SellingTotalPrice]+[ServiceCharge]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED");

            entity.Property(e => e.SellingSn).HasColumnName("SellingSN");

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

            entity.Property(e=> e.ServiceCharge)
                .HasColumnType("decimal(18, 2)");       
            
            entity.Property(e=> e.ServiceCost)
                .HasColumnType("decimal(18, 2)");  
            
            entity.Property(e=> e.ServiceProfit)
                .HasComputedColumnSql("([ServiceCharge]-[ServiceCost]) PERSISTED");  
            
            entity.Property(e=> e.SellingProfit)
                .HasComputedColumnSql("([BuyingTotalPrice]-([SellingTotalPrice]+[SellingDiscountAmount]+[Expense]+[SellingAccountCost])) PERSISTED");

            entity.Property(e => e.ServiceChargeDescription)
                .HasMaxLength(1024); 

            entity.Property(e => e.ExpenseDescription)
                .HasMaxLength(1024);

            entity.Property(e => e.Expense)
                .HasColumnType("decimal(18, 2)");


            entity.Property(e => e.BuyingTotalPrice)
                .HasColumnType("decimal(18, 2)");  
            
            entity.Property(e => e.SellingAccountCost)
                .HasColumnType("decimal(18, 2)");
        }
    }
}