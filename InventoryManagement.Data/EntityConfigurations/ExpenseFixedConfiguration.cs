using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class ExpenseFixedConfiguration : IEntityTypeConfiguration<ExpenseFixed>
    {
        public void Configure(EntityTypeBuilder<ExpenseFixed> builder)
        {
            builder.Property(e => e.CostPerDay)
              .HasComputedColumnSql("([Amount]/[IntervalDays]) PERSISTED");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)");

            builder.Property(e => e.CostPerDay)
                .HasColumnType("decimal(18, 2)");
        }
    }
}