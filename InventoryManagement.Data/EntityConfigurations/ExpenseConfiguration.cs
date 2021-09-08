using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> entity)
        {

            entity.Property(e => e.ExpenseDate)
                .HasColumnType("date");

            entity.Property(e => e.ExpenseFor).HasMaxLength(256);

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ExpenseAmount)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ExpenseCategory)
                .WithMany(p => p.Expense)
                .HasForeignKey(d => d.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expense_ExpenseCategory");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.Expense)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expense_Registration");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Expense)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Expense_Account");
        }
    }
}
