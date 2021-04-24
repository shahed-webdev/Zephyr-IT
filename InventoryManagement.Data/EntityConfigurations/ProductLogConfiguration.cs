using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class ProductLogConfiguration : IEntityTypeConfiguration<ProductLog>
    {
        public void Configure(EntityTypeBuilder<ProductLog> builder)
        {
            builder.Property(e => e.Details)
                .IsRequired()
                .HasMaxLength(1024);
            builder.Property(e => e.LogStatus)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion(c => c.ToString(), c => Enum.Parse<ProductLogStatus>(c));

            builder.Property(e => e.CreatedOnUtc)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            builder.HasOne(e => e.ProductStock)
                .WithMany(e => e.ProductLog)
                .HasForeignKey(e => e.ProductStockId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductLog_ProductStock");

            builder.HasOne(e => e.Registration)
                .WithMany(e => e.ProductLog)
                .HasForeignKey(e => e.ActivityByRegistrationId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ProductLog_Registration");
        }
    }
}