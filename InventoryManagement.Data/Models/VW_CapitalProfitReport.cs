namespace InventoryManagement.Data
{
    public class VW_CapitalProfitReport
    {
        public int Id { get; set; }
        public decimal TotalProductPurchaseValue { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalProductSellingValue { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetProfit { get; set; }
        public decimal LiquidAmount { get; set; }
        public decimal DamagedAmount { get; set; }
        public decimal AccountLiquid { get; set; }
        public decimal MonthlyTotalProfit { get; set; }
        public decimal MonthlyTotalExpense { get; set; }
        public decimal MonthlyDamagedAmount { get; set; }
        public decimal MonthlyNetProfit { get; set; }
    }
}