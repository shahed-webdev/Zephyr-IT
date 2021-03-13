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

                entity.Property(e => e.ServiceDiscountPercentage)
                   .HasComputedColumnSql("(case when [ServiceTotalPrice]=(0) then (0) else ([ServiceDiscountAmount]*(100))/[ServiceTotalPrice] end) PERSISTED");

                entity.Property(e => e.ServiceDueAmount)
                    .HasComputedColumnSql("([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount])) PERSISTED");

                entity.Property(e => e.ServicePaymentStatus)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComputedColumnSql("(case when ([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]))<=(0) then 'Paid' else 'Due' end) PERSISTED");

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


                entity.Property(e => e.ServiceTotalPrice)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceDiscountAmount)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServicePaidAmount)
                    .HasColumnType("decimal(18, 2)");


        }
    }
}