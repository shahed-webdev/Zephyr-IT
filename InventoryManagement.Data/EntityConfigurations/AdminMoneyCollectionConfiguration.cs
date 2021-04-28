using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data
{
    public class AdminMoneyCollectionConfiguration : IEntityTypeConfiguration<AdminMoneyCollection>
    {
        public void Configure(EntityTypeBuilder<AdminMoneyCollection> builder)
        {
            builder.Property(e => e.CollectionAmount)
                .HasColumnType("decimal(18, 2)");

            builder.Property(e => e.Description)
                .HasMaxLength(1024);

            builder.Property(e => e.InsertDateUtc)
                .HasColumnType("date")
                .HasDefaultValueSql("(getutcdate())");

            builder.HasOne(e => e.Registration)
                .WithMany(d => d.AdminMoneyCollection)
                .HasForeignKey(e => e.RegistrationId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_AdminMoneyCollection_Registration");
        }
    }
}