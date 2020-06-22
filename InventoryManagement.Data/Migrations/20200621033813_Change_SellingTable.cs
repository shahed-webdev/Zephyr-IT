using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class Change_SellingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_PurchaseList_PurchaseListId",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_SellingList_ProductStock",
                table: "SellingList");

            migrationBuilder.DropIndex(
                name: "IX_SellingList_ProductStockId",
                table: "SellingList");

            migrationBuilder.DropColumn(
                name: "ProductStockId",
                table: "SellingList");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "SellingList",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SellingList",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "SellingList",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "PurchaseList",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "PurchasePrice",
                table: "PurchaseList",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PurchaseList",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseListId",
                table: "ProductStock",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SellingListId",
                table: "ProductStock",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SellingList_ProductId",
                table: "SellingList",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_SellingListId",
                table: "ProductStock",
                column: "SellingListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_PurchaseList",
                table: "ProductStock",
                column: "PurchaseListId",
                principalTable: "PurchaseList",
                principalColumn: "PurchaseListId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_SellingList",
                table: "ProductStock",
                column: "SellingListId",
                principalTable: "SellingList",
                principalColumn: "SellingListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellingList_Product",
                table: "SellingList",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_PurchaseList",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_SellingList",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_SellingList_Product_ProductId",
                table: "SellingList");

            migrationBuilder.DropIndex(
                name: "IX_SellingList_ProductId",
                table: "SellingList");

            migrationBuilder.DropIndex(
                name: "IX_ProductStock_SellingListId",
                table: "ProductStock");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SellingList");

            migrationBuilder.DropColumn(
                name: "SellingListId",
                table: "ProductStock");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "SellingList",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SellingList",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductStockId",
                table: "SellingList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "PurchaseList",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "PurchasePrice",
                table: "PurchaseList",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PurchaseList",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseListId",
                table: "ProductStock",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_SellingList_ProductStockId",
                table: "SellingList",
                column: "ProductStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_PurchaseList_PurchaseListId",
                table: "ProductStock",
                column: "PurchaseListId",
                principalTable: "PurchaseList",
                principalColumn: "PurchaseListId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellingList_ProductStock",
                table: "SellingList",
                column: "ProductStockId",
                principalTable: "ProductStock",
                principalColumn: "ProductStockId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
