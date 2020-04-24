using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Repository
{
    public class ExpenseVM
    {
        public int ExpenseID { get; set; }
        [Required]
        public int RegistrationID { get; set; }
        [Required]
        public int ExpenseCategoryID { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public double ExpenseAmount { get; set; }
        [Display(Name = "Expense For")]
        public string ExpenseFor { get; set; }
        [Display(Name = "Payment Method")]
        public string ExpensePaymentMethod { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "d mmm yyyy")]
        public DateTime ExpenseDate { get; set; }
    }

    public class ExpenseCategoryWise
    {
        public int ExpenseCategoryID { get; set; }
        public string CategoryName { get; set; }
        public double TotalExpense { get; set; }
    }
}
