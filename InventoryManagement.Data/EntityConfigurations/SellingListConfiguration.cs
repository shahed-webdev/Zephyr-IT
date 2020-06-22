using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class SellingListConfiguration : IEntityTypeConfiguration<SellingList>
    {
        public void Configure(EntityTypeBuilder<SellingList> entity)
        {
            entity.Property(e => e.Warranty).HasMaxLength(128);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.SellingPrice).HasDefaultValueSql("0");


            entity.HasOne(d => d.Selling)
                .WithMany(p => p.SellingList)
                .HasForeignKey(d => d.SellingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellingList_Selling");

        }
    }
}