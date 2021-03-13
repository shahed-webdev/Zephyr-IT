using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class ServiceListConfiguration : IEntityTypeConfiguration<ServiceList>
    {
        public void Configure(EntityTypeBuilder<ServiceList> entity)
        {

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Problem).HasMaxLength(500);

                entity.Property(e => e.Solution).HasMaxLength(500);

                entity.HasOne(d => d.ServiceDevice)
                    .WithMany(p => p.ServiceList)
                    .HasForeignKey(d => d.ServiceDeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceList_ServiceDevice");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceList)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceList_Service");



                entity.Property(e => e.ServiceCharge)
                    .HasColumnType("decimal(18, 2)");
        }
    }
}