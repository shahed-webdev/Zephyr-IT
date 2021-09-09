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
        public decimal ExpenseAmount { get; set; }

        public string ExpenseFor { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d MMMM, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
    }

    public class ExpenseCategoryWise
    {
        public int ExpenseCategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalExpense { get; set; }
    }


    public class ExpenseAddModel
    {
        public int ExpenseId { get; set; }
        [Required]
        public int ExpenseCategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public decimal ExpenseAmount { get; set; }
        public string ExpenseFor { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d MMMM, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }
        public int? AccountId { get; set; }
    }

    public class ExpenseDetailsModel
    {
        public int ExpenseId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public bool IsApproved { get; set; }
        public string CreateBy { get; set; }
        public string CategoryName { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string ExpenseFor { get; set; }

        [DisplayFormat(DataFormatString = "{0:d MMMM, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
    }

    public class ExpenseUpdateAccountUpdateModel
    {
        public bool IsApproved { get; set; }
        public decimal PrevAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public int? PrevAccountId { get; set; }
        public int? CurrentAccountId { get; set; }
    }
}
