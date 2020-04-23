using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class PageLinkCategoryConfiguration : IEntityTypeConfiguration<PageLinkCategory>
    {
        public void Configure(EntityTypeBuilder<PageLinkCategory> entity)
        {
            entity.HasKey(e => e.LinkCategoryId);

            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.IconClass).HasMaxLength(128);

            entity.Property(e => e.Sn).HasColumnName("SN");
        }
    }
}