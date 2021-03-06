using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class AccountDepositConfiguration: IEntityTypeConfiguration<AccountDeposit>
    {
        public void Configure(EntityTypeBuilder<AccountDeposit> builder)
        {
            builder.Property(e => e.DepositAmount)
                .HasColumnType("decimal(18, 2)");

            builder.Property(e => e.Description)
                .HasMaxLength(1024);

            builder.Property(e => e.DepositDateUtc)
                .HasColumnType("date");

            builder.Property(e => e.InsertDateUtc)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            builder.HasOne(e => e.Account)
                .WithMany(d => d.AccountDeposit)
                .HasForeignKey(e=> e.AccountId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_AccountDeposit_Account");
        }
    }
}