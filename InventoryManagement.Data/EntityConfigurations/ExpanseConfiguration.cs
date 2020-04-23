using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data 
{
   public class ExpanseConfiguration : IEntityTypeConfiguration<Expanse>
    {
        public void Configure(EntityTypeBuilder<Expanse> entity)
        {
            
                entity.Property(e => e.ExpanseDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpanseFor).HasMaxLength(256);

                entity.Property(e => e.ExpansePaymentMethod).HasMaxLength(50);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ExpanseCategory)
                    .WithMany(p => p.Expanse)
                    .HasForeignKey(d => d.ExpanseCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expanse_ExpanseCategory");

                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.Expanse)
                    .HasForeignKey(d => d.RegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expanse_Registration");
        }
    }
}
