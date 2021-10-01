using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AlterViewVW_CapitalProfitReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW VW_CapitalProfitReport
AS 
SELECT        1 AS Id, P.TotalProductPurchaseValue, P.TotalInvestment, S.TotalProductSellingValue, S.TotalProfit, R.TotalRevenue, E.TotalExpense, S.TotalProfit - E.TotalExpense - D.DamagedAmount AS NetProfit, 
                         R.TotalRevenue - P.TotalInvestment AS LiquidAmount, D.DamagedAmount, A.AccountLiquid, MP.MonthlyTotalProfit, ME.MonthlyTotalExpense, MD.MonthlyDamagedAmount, 
                         MP.MonthlyTotalProfit - ME.MonthlyTotalExpense - MD.MonthlyDamagedAmount AS MonthlyNetProfit
FROM            (SELECT        ISNULL(SUM(PurchaseTotalPrice - PurchaseDiscountAmount - PurchaseReturnAmount), 0) AS TotalProductPurchaseValue, ISNULL(SUM(PurchasePaidAmount), 0) AS TotalInvestment
                          FROM            dbo.Purchase) AS P CROSS JOIN
                             (SELECT        ISNULL(SUM(SellingTotalPrice - SellingDiscountAmount - SellingReturnAmount - ExpenseTotal - SellingAccountCost), 0) AS TotalProductSellingValue, ISNULL(SUM(GrandProfit), 0) AS TotalProfit
                               FROM            dbo.Selling) AS S CROSS JOIN
                             (SELECT        ISNULL(SUM(SellingPaidAmount - ServiceCharge), 0) AS TotalRevenue
                               FROM            dbo.Selling AS Selling_1
                               WHERE        (SellingPaidAmount - ServiceCharge > 0)) AS R CROSS JOIN
                             (SELECT        ISNULL(SUM(ExpenseAmount), 0) AS TotalExpense
                               FROM            dbo.VW_ExpenseWithTransportation
                               WHERE        (IsApproved = 1)) AS E CROSS JOIN
                             (SELECT        ISNULL(SUM(Balance), 0) AS AccountLiquid
                               FROM            dbo.Account) AS A CROSS JOIN
                             (SELECT        ISNULL(SUM(dbo.PurchaseList.PurchasePrice), 0) AS DamagedAmount
                               FROM            dbo.ProductDamaged INNER JOIN
                                                         dbo.ProductStock ON dbo.ProductDamaged.ProductStockId = dbo.ProductStock.ProductStockId INNER JOIN
                                                         dbo.PurchaseList ON dbo.ProductStock.PurchaseListId = dbo.PurchaseList.PurchaseListId) AS D CROSS JOIN
                             (SELECT        ISNULL(SUM(GrandProfit), 0) AS MonthlyTotalProfit
                               FROM            dbo.Selling AS Selling_2
                               WHERE        (SellingPaymentStatus = 'Paid') AND (YEAR(LastUpdateDate) = YEAR(GETDATE())) AND (MONTH(LastUpdateDate) = MONTH(GETDATE()))) AS MP CROSS JOIN
                             (SELECT        ISNULL(SUM(ExpenseAmount), 0) AS MonthlyTotalExpense
                               FROM            dbo.VW_ExpenseWithTransportation AS VW_ExpenseWithTransportation_1
                               WHERE        (IsApproved = 1) AND (YEAR(ExpenseDate) = YEAR(GETDATE())) AND (MONTH(ExpenseDate) = MONTH(GETDATE()))) AS ME CROSS JOIN
                             (SELECT        ISNULL(SUM(PurchaseList_1.PurchasePrice), 0) AS MonthlyDamagedAmount
                               FROM            dbo.ProductDamaged AS ProductDamaged_1 INNER JOIN
                                                         dbo.ProductStock AS ProductStock_1 ON ProductDamaged_1.ProductStockId = ProductStock_1.ProductStockId INNER JOIN
                                                         dbo.PurchaseList AS PurchaseList_1 ON ProductStock_1.PurchaseListId = PurchaseList_1.PurchaseListId
                               WHERE        (YEAR(ProductDamaged_1.DamagedDate) = YEAR(GETDATE())) AND (MONTH(ProductDamaged_1.DamagedDate) = MONTH(GETDATE()))) AS MD

 ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW VW_CapitalProfitReport
AS
SELECT        1 AS Id, P.TotalProductPurchaseValue, P.TotalInvestment, S.TotalProductSellingValue, S.TotalProfit, R.TotalRevenue, E.TotalExpense, S.TotalProfit - E.TotalExpense AS NetProfit, 
                         R.TotalRevenue - P.TotalInvestment AS LiquidAmount
FROM            (SELECT        SUM(PurchaseTotalPrice - PurchaseDiscountAmount - PurchaseReturnAmount) AS TotalProductPurchaseValue, SUM(PurchasePaidAmount) AS TotalInvestment
                          FROM            dbo.Purchase) AS P CROSS JOIN
                             (SELECT        SUM(SellingTotalPrice - SellingDiscountAmount - SellingReturnAmount - ExpenseTotal - SellingAccountCost) AS TotalProductSellingValue, SUM(GrandProfit) AS TotalProfit
                               FROM            dbo.Selling) AS S CROSS JOIN
                             (SELECT        SUM(SellingPaidAmount - ServiceCharge) AS TotalRevenue
                               FROM            dbo.Selling AS Selling_1
                               WHERE        (SellingPaidAmount - ServiceCharge > 0)) AS R CROSS JOIN
                             (SELECT        SUM(ExpenseAmount) AS TotalExpense
                               FROM            dbo.VW_ExpenseWithTransportation
                               WHERE        (IsApproved = 1)) AS E
             ");
        }
    }
}
