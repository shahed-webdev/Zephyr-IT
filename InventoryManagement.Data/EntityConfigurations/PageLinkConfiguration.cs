using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class PageLinkConfiguration : IEntityTypeConfiguration<PageLink>
    {
        public void Configure(EntityTypeBuilder<PageLink> entity)
        {
            entity.HasKey(e => e.LinkId);

            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.Controller)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.IconClass).HasMaxLength(128);

            entity.Property(e => e.RoleId).HasMaxLength(128);

            entity.Property(e => e.Sn).HasColumnName("SN");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(128);

            entity.HasOne(d => d.LinkCategory)
                .WithMany(p => p.PageLink)
                .HasForeignKey(d => d.LinkCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PageLink_PageLinkCategory");
        }
    }
}