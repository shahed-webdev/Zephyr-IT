using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class ExpenseTransportationConfiguration : IEntityTypeConfiguration<ExpenseTransportation>
    {
        public void Configure(EntityTypeBuilder<ExpenseTransportation> builder)
        {
            builder.Property(p => p.ExpenseNote)
                .HasMaxLength(500);

            builder.Property(e => e.ExpenseDate)
                .HasColumnType("date");

            builder.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.Customer)
                .WithMany(p => p.ExpenseTransportation)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Registration)
                .WithMany(p => p.ExpenseTransportation)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.TotalExpense)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.Account)
                .WithMany(p => p.ExpenseTransportation)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ExpenseTransportation_Account");
        }
    }
}