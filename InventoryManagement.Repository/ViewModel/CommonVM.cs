using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class DDL
    {
        public int value { get; set; }
        public string label { get; set; }
    }
    public class RoleDDL
    {
        public string RoleId { get; set; }
        public string Role { get; set; }
    }

    public class AjaxContent<TData> where TData : class
    {
        public bool Status { get; set; }
        public TData Data { get; set; }
    }

    public class MonthName
    {
        public int MonthNumber { get; set; }
        public string Month { get; set; }
    }

    public class AllMonth
    {
        public ICollection<MonthName> AllMonthNames { get; }

        public AllMonth()
        {
            AllMonthNames = new List<MonthName>()
                {
                    new MonthName {MonthNumber = 1, Month = "JAN"},
                    new MonthName {MonthNumber = 2, Month = "FEB"},
                    new MonthName {MonthNumber = 3, Month = "MAR"},
                    new MonthName {MonthNumber = 4, Month = "APR"},
                    new MonthName {MonthNumber = 5, Month = "MAY"},
                    new MonthName {MonthNumber = 6, Month = "JUN"},
                    new MonthName {MonthNumber = 7, Month = "JUL"},
                    new MonthName {MonthNumber = 8, Month = "AUG"},
                    new MonthName {MonthNumber = 9, Month = "SEP"},
                    new MonthName {MonthNumber = 10, Month = "OCT"},
                    new MonthName {MonthNumber = 11, Month = "NOV"},
                    new MonthName {MonthNumber = 12, Month = "DEC"}
                };
        }
    }
    public class MonthlyAmount
    {
        public int MonthNumber { get; set; }
        public decimal Amount { get; set; }
    }

    public class MonthIncomeExpense
    {
        public string Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
