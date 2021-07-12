using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class SellingPromiseDateChangeConfiguration : IEntityTypeConfiguration<SellingPromiseDateMiss>
    {
        public void Configure(EntityTypeBuilder<SellingPromiseDateMiss> builder)
        {
            builder.Property(e => e.InsertDateUtc)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            builder.Property(e => e.MissDate)
                .HasColumnType("date");

            builder.HasOne(e => e.Selling)
                .WithMany(e => e.SellingPromiseDateMisses)
                .HasForeignKey(d => d.SellingId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_SellingPromiseDateMiss_Selling");

            builder.HasOne(e => e.Registration)
                .WithMany(e => e.SellingPromiseDateMisses)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_SellingPromiseDateMiss_Registration");


        }
    }
}