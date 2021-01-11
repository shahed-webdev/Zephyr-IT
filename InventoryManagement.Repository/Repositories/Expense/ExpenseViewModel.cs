using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Repository
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }

        [Required]
        public int RegistrationId { get; set; }

        [Required]
        public int ExpenseCategoryId { get; set; }

        public string CategoryName { get; set; }

        [Required]
        public double ExpenseAmount { get; set; }

        public string ExpenseFor { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "dd MMM yyyy")]
        public DateTime ExpenseDate { get; set; }
    }

    public class ExpenseCategoryWise
    {
        public int ExpenseCategoryId { get; set; }
        public string CategoryName { get; set; }
        public double TotalExpense { get; set; }
    }


    public class ExpenseAddModel
    {
        public int ExpenseId { get; set; }
        [Required]
        public int RegistrationId { get; set; }
        [Required]
        public int ExpenseCategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public double ExpenseAmount { get; set; }
        public string ExpenseFor { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "dd MMM yyyy")]
        public DateTime ExpenseDate { get; set; }
    }
}
