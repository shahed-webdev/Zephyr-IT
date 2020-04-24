using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Data 
{
   public class ExpenseCategoryConfiguration :IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> entity)
        {
            entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(128);
       
        }
    }
}
