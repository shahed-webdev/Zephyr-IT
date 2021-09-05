using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class VW_CapitalProfitReportConfiguration : IEntityTypeConfiguration<VW_CapitalProfitReport>
    {
        public void Configure(EntityTypeBuilder<VW_CapitalProfitReport> builder)
        {
            builder.ToView(nameof(VW_CapitalProfitReport)).HasKey(t => t.Id);
        }
    }
}