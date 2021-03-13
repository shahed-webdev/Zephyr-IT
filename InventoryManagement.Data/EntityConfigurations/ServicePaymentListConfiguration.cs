using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class ServicePaymentListConfiguration : IEntityTypeConfiguration<ServicePaymentList>
    {
        public void Configure(EntityTypeBuilder<ServicePaymentList> entity)
        {

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.SellingPayment)
                    .WithMany(p => p.ServicePaymentList)
                    .HasForeignKey(d => d.SellingPaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServicePaymentList_SellingPayment");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServicePaymentList)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServicePaymentList_Service");


                entity.Property(e => e.ServicePaidAmount)
                    .HasColumnType("decimal(18, 2)");

        }
    }
}