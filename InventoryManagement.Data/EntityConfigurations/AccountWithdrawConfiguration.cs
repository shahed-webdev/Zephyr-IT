using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class AccountWithdrawConfiguration : IEntityTypeConfiguration<AccountWithdraw>
    {
        public void Configure(EntityTypeBuilder<AccountWithdraw> builder)
        {
            builder.Property(e => e.WithdrawAmount)
                .HasColumnType("decimal(18, 2)");

            builder.Property(e => e.Description)
                .HasMaxLength(1024);

            builder.Property(e => e.WithdrawDateUtc)
                .HasColumnType("date");

            builder.Property(e => e.InsertDateUtc)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            builder.HasOne(e => e.Account)
                .WithMany(d => d.AccountWithdraw)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_AccountWithdraw_Account");
        }
    }
}