using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {

            builder.Property(e => e.AccountName)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(e=> e.Balance)
                .HasColumnType("decimal(18, 2)");    
            
            builder.Property(e=> e.CostPercentage)
                .HasColumnType("decimal(18, 2)");
        }
    }
}