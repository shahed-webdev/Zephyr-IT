using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data 
{
   public class ExpanseCategoryConfiguration :IEntityTypeConfiguration<ExpanseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpanseCategory> entity)
        {
            entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(128);
       
        }
    }
}
