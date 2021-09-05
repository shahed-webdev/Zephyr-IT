using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AddCapitalLiquidAmountReportView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create view VW_CapitalProfitReport as
                SELECT  1 AS Id, P.TotalProductPurchaseValue, P.TotalInvestment, S.TotalProductSellingValue, S.TotalProfit, R.TotalRevenue, E.TotalExpense, S.TotalProfit - E.TotalExpense AS NetProfit, R.TotalRevenue - P.TotalInvestment AS LiquidAmount
                         FROM                         
                             (SELECT SUM(PurchaseTotalPrice - PurchaseDiscountAmount - PurchaseReturnAmount) AS TotalProductPurchaseValue, SUM(PurchasePaidAmount) AS TotalInvestment
                               FROM  dbo.Purchase) AS P CROSS JOIN
                             (SELECT SUM(SellingTotalPrice - SellingDiscountAmount - SellingReturnAmount - ExpenseTotal - SellingAccountCost) AS TotalProductSellingValue, SUM(GrandProfit) AS TotalProfit
                               FROM  dbo.Selling) AS S CROSS JOIN
                             (SELECT SUM(SellingPaidAmount - ServiceCharge) AS TotalRevenue
                               FROM  dbo.Selling AS Selling_1
                               WHERE (SellingPaidAmount - ServiceCharge > 0)) AS R CROSS JOIN
                             (SELECT SUM(ExpenseAmount) AS TotalExpense
                               FROM  dbo.VW_ExpenseWithTransportation
                               WHERE (IsApproved = 1)) AS E;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop view VW_CapitalProfitReport;");
        }
    }
}
