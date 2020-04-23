using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.Property(e => e.CustomerAddress).HasMaxLength(500);

            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.CustomerPhone)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Due)
                .HasComputedColumnSql("(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]))");

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.OrganizationName).HasMaxLength(128);
        }
    }
}