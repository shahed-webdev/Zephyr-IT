using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class Delete_PurchaseAdjustment_Add_PurchaseList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Purchase",
                table: "Product");

            migrationBuilder.DropTable(
                name: "PurchaseAdjustment");

            migrationBuilder.DropIndex(
                name: "IX_Product_PurchaseId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseListId",
                table: "ProductStock",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "Product",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateTable(
                name: "PurchaseList",
                columns: table => new
                {
                    PurchaseListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    PurchasePrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseList", x => x.PurchaseListId);
                    table.ForeignKey(
                        name: "FK_PurchaseList_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_PurchaseList_Purchase",
                        column: x => x.PurchaseId,
                        principalTable: "Purchase",
                        principalColumn: "PurchaseId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_PurchaseListId",
                table: "ProductStock",
                column: "PurchaseListId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseList_ProductId",
                table: "PurchaseList",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseList_PurchaseId",
                table: "PurchaseList",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_PurchaseList_PurchaseListId",
                table: "ProductStock",
                column: "PurchaseListId",
                principalTable: "PurchaseList",
                principalColumn: "PurchaseListId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_PurchaseList_PurchaseListId",
                table: "ProductStock");

            migrationBuilder.DropTable(
                name: "PurchaseList");

            migrationBuilder.DropIndex(
                name: "IX_ProductStock_PurchaseListId",
                table: "ProductStock");

            migrationBuilder.DropColumn(
                name: "PurchaseListId",
                table: "ProductStock");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "Product",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldDefaultValueSql: "0");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PurchasePrice",
                table: "Product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "PurchaseAdjustment",
                columns: table => new
                {
                    PurchaseAdjustmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdjustmentStatus = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ProductCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseAdjustment", x => x.PurchaseAdjustmentId);
                    table.ForeignKey(
                        name: "FK_PurchaseAdjustment_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseAdjustment_Purchase",
                        column: x => x.PurchaseId,
                        principalTable: "Purchase",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_PurchaseId",
                table: "Product",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAdjustment_ProductId",
                table: "PurchaseAdjustment",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAdjustment_PurchaseId",
                table: "PurchaseAdjustment",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Purchase",
                table: "Product",
                column: "PurchaseId",
                principalTable: "Purchase",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
