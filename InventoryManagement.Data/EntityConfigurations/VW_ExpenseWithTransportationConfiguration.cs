using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class VW_ExpenseWithTransportationConfiguration : IEntityTypeConfiguration<VW_ExpenseWithTransportation>
    {
        public void Configure(EntityTypeBuilder<VW_ExpenseWithTransportation> builder)
        {
            builder.ToView(nameof(VW_ExpenseWithTransportation))
                .HasKey(t => t.Id);
        }
    }
}