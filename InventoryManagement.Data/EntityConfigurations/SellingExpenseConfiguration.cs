using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class SellingExpenseConfiguration : IEntityTypeConfiguration<SellingExpense>
    {
        public void Configure(EntityTypeBuilder<SellingExpense> builder)
        {
            builder.Property(e => e.ExpenseDescription)
                .HasMaxLength(1024);

            builder.Property(e => e.Expense)
                .HasColumnType("decimal(18, 2)");

            builder.Property(e => e.InsertDateUtc)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            builder.HasOne(e => e.Selling)
                .WithMany(d => d.SellingExpense)
                .HasForeignKey(e => e.SellingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SellingExpense_Selling");

            builder.HasOne(d => d.Account)
                .WithMany(p => p.SellingExpense)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SellingExpense_Account");
        }
    }
}