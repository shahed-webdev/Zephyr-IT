using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.Property(e => e.CustomerAddress).HasMaxLength(500);

            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(128);
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            entity.Property(e => e.Designation)
                .HasMaxLength(255);
            entity.Property(e => e.Due)
                .HasComputedColumnSql("(([TotalAmount]+[ReturnAmount]+[AccountTransactionCharge])-([TotalDiscount]+[Paid]+[PurchaseAdjustedAmount])) PERSISTED");
            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.OrganizationName)
                .HasMaxLength(128);

            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.TotalDiscount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.ReturnAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.Paid)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.PurchaseAdjustedAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.DueLimit)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.AccountTransactionCharge)
                .HasColumnType("decimal(18, 2)")
                .HasDefaultValueSql("(0.00)");
        }
    }
}