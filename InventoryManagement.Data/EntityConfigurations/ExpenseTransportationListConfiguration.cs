using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class ExpenseTransportationListConfiguration : IEntityTypeConfiguration<ExpenseTransportationList>
    {
        public void Configure(EntityTypeBuilder<ExpenseTransportationList> builder)
        {
            builder.Property(p => p.ExpenseFor)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(p => p.Vehicle)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(p => p.ExpenseFor)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasOne(d => d.ExpenseTransportation)
                .WithMany(p => p.ExpenseTransportationList)
                .HasForeignKey(d => d.ExpenseTransportationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.ExpenseAmount)
                .HasColumnType("decimal(18, 2)");
        }
    }
}