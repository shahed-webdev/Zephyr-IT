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

                entity.Property(e => e.SellingDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SellingDiscountPercentage).HasComputedColumnSql("(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end)");

                entity.Property(e => e.SellingDueAmount).HasComputedColumnSql("(round(([SellingTotalPrice]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))");

                entity.Property(e => e.SellingPaymentStatus)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComputedColumnSql("(case when (([SellingTotalPrice]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end)");

                entity.Property(e => e.SellingSn).HasColumnName("SellingSN");

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
            
        }
    }
}