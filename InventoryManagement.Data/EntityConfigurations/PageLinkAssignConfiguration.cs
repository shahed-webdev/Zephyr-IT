using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class PageLinkAssignConfiguration : IEntityTypeConfiguration<PageLinkAssign>
    {
        public void Configure(EntityTypeBuilder<PageLinkAssign> entity)
        {
                entity.HasKey(e => e.LinkAssignId);

                entity.HasOne(d => d.Link)
                    .WithMany(p => p.PageLinkAssign)
                    .HasForeignKey(d => d.LinkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PageLinkAssign_PageLink");

                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.PageLinkAssign)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PageLinkAssign_Registration");
       
        }
    }
}