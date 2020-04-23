using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    internal class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> entity)
        {

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ServiceDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ServiceDiscountPercentage).HasComputedColumnSql("(case when [ServiceTotalPrice]=(0) then (0) else round(([ServiceDiscountAmount]*(100))/[ServiceTotalPrice],(2)) end)");

                entity.Property(e => e.ServiceDueAmount).HasComputedColumnSql("(round([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]),(2)))");

                entity.Property(e => e.ServicePaymentStatus)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComputedColumnSql("(case when ([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]))<=(0) then 'Paid' else 'Due' end)");

                entity.Property(e => e.ServiceSn).HasColumnName("ServiceSN");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Service)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_Customer");

                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.Service)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_Registration");

        }
    }
}