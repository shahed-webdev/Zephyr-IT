using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

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


            builder.HasOne(e => e.Selling)
                .WithMany(e => e.ProductLog)
                .HasForeignKey(e => e.SellingId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ProductLog_Selling");

            builder.HasOne(e => e.Purchase)
                .WithMany(e => e.ProductLog)
                .HasForeignKey(e => e.PurchaseId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ProductLog_Purchase");
        }
    }
}