using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class ExpenseFixedConfiguration : IEntityTypeConfiguration<ExpenseFixed>
    {
        public void Configure(EntityTypeBuilder<ExpenseFixed> builder)
        {
            builder.Property(e => e.CostPerDay).
                HasComputedColumnSql("ROUND(([Amount]/[IntervalDays]),2)");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
        }
    }
}