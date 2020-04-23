using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class AllTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ba9dfc5-98c2-4714-b981-e88ff495175a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "6e6c6586-b81a-4c22-84d1-e6f900f9bd1d", "b0515de6-18b2-4e8f-bd30-e336dd981867" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0515de6-18b2-4e8f-bd30-e336dd981867");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e6c6586-b81a-4c22-84d1-e6f900f9bd1d");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationName = table.Column<string>(maxLength: 128, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 128, nullable: false),
                    CustomerAddress = table.Column<string>(maxLength: 500, nullable: true),
                    CustomerPhone = table.Column<string>(maxLength: 50, nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    TotalDiscount = table.Column<double>(nullable: false),
                    ReturnAmount = table.Column<double>(nullable: false),
                    Paid = table.Column<double>(nullable: false),
                    Due = table.Column<double>(nullable: false, computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]))"),
                    Photo = table.Column<byte[]>(nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ExpanseCategory",
                columns: table => new
                {
                    ExpanseCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(maxLength: 128, nullable: false),
                    TotalExpanse = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpanseCategory", x => x.ExpanseCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Institution",
                columns: table => new
                {
                    InstitutionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstitutionName = table.Column<string>(maxLength: 500, nullable: false),
                    DialogTitle = table.Column<string>(maxLength: 256, nullable: true),
                    Established = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    City = table.Column<string>(maxLength: 128, nullable: true),
                    State = table.Column<string>(maxLength: 128, nullable: true),
                    LocalArea = table.Column<string>(maxLength: 128, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 50, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Website = table.Column<string>(maxLength: 50, nullable: true),
                    InstitutionLogo = table.Column<byte[]>(nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institution", x => x.InstitutionId);
                });

            migrationBuilder.CreateTable(
                name: "PageLinkCategory",
                columns: table => new
                {
                    LinkCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(maxLength: 128, nullable: false),
                    IconClass = table.Column<string>(maxLength: 128, nullable: true),
                    SN = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageLinkCategory", x => x.LinkCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCatalogType",
                columns: table => new
                {
                    CatalogTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogType = table.Column<string>(maxLength: 128, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCatalogType", x => x.CatalogTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Validation = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    FatherName = table.Column<string>(maxLength: 128, nullable: true),
                    Designation = table.Column<string>(maxLength: 128, nullable: true),
                    DateofBirth = table.Column<string>(maxLength: 50, nullable: true),
                    NationalID = table.Column<string>(maxLength: 128, nullable: true),
                    Address = table.Column<string>(maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    PS = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.RegistrationId);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    VendorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorCompanyName = table.Column<string>(maxLength: 128, nullable: false),
                    VendorName = table.Column<string>(maxLength: 128, nullable: true),
                    VendorAddress = table.Column<string>(maxLength: 500, nullable: true),
                    VendorPhone = table.Column<string>(maxLength: 50, nullable: true),
                    TotalAmount = table.Column<double>(nullable: false),
                    TotalDiscount = table.Column<double>(nullable: false),
                    ReturnAmount = table.Column<double>(nullable: false),
                    Paid = table.Column<double>(nullable: false),
                    Due = table.Column<double>(nullable: false, computedColumnSql: "(([TotalAmount]+[ReturnAmount])-([TotalDiscount]+[Paid]))"),
                    Photo = table.Column<byte[]>(nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.VendorId);
                });

            migrationBuilder.CreateTable(
                name: "PageLink",
                columns: table => new
                {
                    LinkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkCategoryId = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(maxLength: 128, nullable: true),
                    Controller = table.Column<string>(maxLength: 128, nullable: false),
                    Action = table.Column<string>(maxLength: 128, nullable: false),
                    Title = table.Column<string>(maxLength: 128, nullable: false),
                    IconClass = table.Column<string>(maxLength: 128, nullable: true),
                    SN = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageLink", x => x.LinkId);
                    table.ForeignKey(
                        name: "FK_PageLink_PageLinkCategory",
                        column: x => x.LinkCategoryId,
                        principalTable: "PageLinkCategory",
                        principalColumn: "LinkCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCatalog",
                columns: table => new
                {
                    ProductCatalogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogTypeId = table.Column<int>(nullable: true),
                    CatalogName = table.Column<string>(maxLength: 500, nullable: false),
                    CatalogLevel = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    ItemCount = table.Column<int>(nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCatalog", x => x.ProductCatalogId);
                    table.ForeignKey(
                        name: "FK_ProductCatalog_ProductCatalogType",
                        column: x => x.CatalogTypeId,
                        principalTable: "ProductCatalogType",
                        principalColumn: "CatalogTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCatalog_ProductCatalog",
                        column: x => x.ParentId,
                        principalTable: "ProductCatalog",
                        principalColumn: "ProductCatalogId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Expanse",
                columns: table => new
                {
                    ExpanseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    ExpanseCategoryId = table.Column<int>(nullable: false),
                    ExpanseAmount = table.Column<double>(nullable: false),
                    ExpanseFor = table.Column<string>(maxLength: 256, nullable: true),
                    ExpansePaymentMethod = table.Column<string>(maxLength: 50, nullable: true),
                    ExpanseDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expanse", x => x.ExpanseId);
                    table.ForeignKey(
                        name: "FK_Expanse_ExpanseCategory",
                        column: x => x.ExpanseCategoryId,
                        principalTable: "ExpanseCategory",
                        principalColumn: "ExpanseCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expanse_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Selling",
                columns: table => new
                {
                    SellingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    SellingSN = table.Column<int>(nullable: false),
                    SellingTotalPrice = table.Column<double>(nullable: false),
                    SellingDiscountAmount = table.Column<double>(nullable: false),
                    SellingDiscountPercentage = table.Column<double>(nullable: false, computedColumnSql: "(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end)"),
                    SellingPaidAmount = table.Column<double>(nullable: false),
                    SellingReturnAmount = table.Column<double>(nullable: false),
                    SellingDueAmount = table.Column<double>(nullable: false, computedColumnSql: "(round(([SellingTotalPrice]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))"),
                    SellingPaymentStatus = table.Column<string>(unicode: false, maxLength: 4, nullable: false, computedColumnSql: "(case when (([SellingTotalPrice]+[SellingReturnAmount])-([SellingDiscountAmount]+[SellingPaidAmount]))<=(0) then 'Paid' else 'Due' end)"),
                    SellingDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Selling", x => x.SellingId);
                    table.ForeignKey(
                        name: "FK_Selling_Customer",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Selling_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellingPayment",
                columns: table => new
                {
                    SellingPaymentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ReceiptSN = table.Column<int>(nullable: false),
                    PaidAmount = table.Column<double>(nullable: false),
                    PaymentMethod = table.Column<string>(maxLength: 50, nullable: true),
                    PaidDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingPayment", x => x.SellingPaymentId);
                    table.ForeignKey(
                        name: "FK_SellingPayment_Customer",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingPayment_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ServiceSN = table.Column<int>(nullable: false),
                    ServiceTotalPrice = table.Column<double>(nullable: false),
                    ServiceDiscountAmount = table.Column<double>(nullable: false),
                    ServiceDiscountPercentage = table.Column<double>(nullable: false, computedColumnSql: "(case when [ServiceTotalPrice]=(0) then (0) else round(([ServiceDiscountAmount]*(100))/[ServiceTotalPrice],(2)) end)"),
                    ServicePaidAmount = table.Column<double>(nullable: false),
                    ServiceDueAmount = table.Column<double>(nullable: false, computedColumnSql: "(round([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]),(2)))"),
                    ServicePaymentStatus = table.Column<string>(unicode: false, maxLength: 4, nullable: false, computedColumnSql: "(case when ([ServiceTotalPrice]-([ServiceDiscountAmount]+[ServicePaidAmount]))<=(0) then 'Paid' else 'Due' end)"),
                    ServiceDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_Service_Customer",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Service_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    PurchaseSN = table.Column<int>(nullable: false),
                    PurchaseTotalPrice = table.Column<double>(nullable: false),
                    PurchaseDiscountAmount = table.Column<double>(nullable: false),
                    PurchaseDiscountPercentage = table.Column<double>(nullable: false, computedColumnSql: "(case when [PurchaseTotalPrice]=(0) then (0) else round(([PurchaseDiscountAmount]*(100))/[PurchaseTotalPrice],(2)) end)"),
                    PurchasePaidAmount = table.Column<double>(nullable: false),
                    PurchaseReturnAmount = table.Column<double>(nullable: false),
                    PurchaseDueAmount = table.Column<double>(nullable: false, computedColumnSql: "(round(([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]),(2)))"),
                    PurchasePaymentStatus = table.Column<string>(unicode: false, maxLength: 4, nullable: false, computedColumnSql: "(case when (([PurchaseTotalPrice]+[PurchaseReturnAmount])-([PurchaseDiscountAmount]+[PurchasePaidAmount]))<=(0) then 'Paid' else 'Due' end)"),
                    PurchaseDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchase_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Vendor",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasePayment",
                columns: table => new
                {
                    PurchasePaymentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    ReceiptSN = table.Column<int>(nullable: false),
                    PaidAmount = table.Column<double>(nullable: false),
                    PaymentMethod = table.Column<string>(maxLength: 50, nullable: true),
                    PaidDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePayment", x => x.PurchasePaymentId);
                    table.ForeignKey(
                        name: "FK_PurchasePayment_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasePayment_Vendor",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PageLinkAssign",
                columns: table => new
                {
                    LinkAssignId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    LinkId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageLinkAssign", x => x.LinkAssignId);
                    table.ForeignKey(
                        name: "FK_PageLinkAssign_PageLink",
                        column: x => x.LinkId,
                        principalTable: "PageLink",
                        principalColumn: "LinkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PageLinkAssign_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDevice",
                columns: table => new
                {
                    ServiceDeviceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCatalogId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    DeviceCode = table.Column<string>(maxLength: 128, nullable: false),
                    DeviceName = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDevice", x => x.ServiceDeviceId);
                    table.ForeignKey(
                        name: "FK_ServiceDevice_Customer",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceDevice_ProductCatalog",
                        column: x => x.ProductCatalogId,
                        principalTable: "ProductCatalog",
                        principalColumn: "ProductCatalogId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellingPaymentList",
                columns: table => new
                {
                    SellingPaymentListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellingPaymentId = table.Column<int>(nullable: false),
                    SellingId = table.Column<int>(nullable: false),
                    SellingPaidAmount = table.Column<double>(nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingPaymentList", x => x.SellingPaymentListId);
                    table.ForeignKey(
                        name: "FK_SellingPaymentList_Selling",
                        column: x => x.SellingId,
                        principalTable: "Selling",
                        principalColumn: "SellingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingPaymentList_SellingPayment",
                        column: x => x.SellingPaymentId,
                        principalTable: "SellingPayment",
                        principalColumn: "SellingPaymentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicePaymentList",
                columns: table => new
                {
                    ServicePaymentListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellingPaymentId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    ServicePaidAmount = table.Column<double>(nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePaymentList", x => x.ServicePaymentListId);
                    table.ForeignKey(
                        name: "FK_ServicePaymentList_SellingPayment",
                        column: x => x.SellingPaymentId,
                        principalTable: "SellingPayment",
                        principalColumn: "SellingPaymentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicePaymentList_Service",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(nullable: false),
                    ProductCatalogId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Warranty = table.Column<string>(maxLength: 128, nullable: true),
                    PurchasePrice = table.Column<double>(nullable: false),
                    SellingPrice = table.Column<double>(nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_ProductCatalog",
                        column: x => x.ProductCatalogId,
                        principalTable: "ProductCatalog",
                        principalColumn: "ProductCatalogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Purchase",
                        column: x => x.PurchaseId,
                        principalTable: "Purchase",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasePaymentList",
                columns: table => new
                {
                    PurchasePaymentListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchasePaymentId = table.Column<int>(nullable: false),
                    PurchaseId = table.Column<int>(nullable: false),
                    PurchasePaidAmount = table.Column<double>(nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePaymentList", x => x.PurchasePaymentListId);
                    table.ForeignKey(
                        name: "FK_PurchasePaymentList_Purchase",
                        column: x => x.PurchaseId,
                        principalTable: "Purchase",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasePaymentList_PurchasePayment",
                        column: x => x.PurchasePaymentId,
                        principalTable: "PurchasePayment",
                        principalColumn: "PurchasePaymentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceList",
                columns: table => new
                {
                    ServiceListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(nullable: false),
                    ServiceDeviceId = table.Column<int>(nullable: false),
                    ServiceCharge = table.Column<double>(nullable: false),
                    Problem = table.Column<string>(maxLength: 500, nullable: true),
                    Solution = table.Column<string>(maxLength: 500, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceList", x => x.ServiceListId);
                    table.ForeignKey(
                        name: "FK_ServiceList_ServiceDevice",
                        column: x => x.ServiceDeviceId,
                        principalTable: "ServiceDevice",
                        principalColumn: "ServiceDeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceList_Service",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductStock",
                columns: table => new
                {
                    ProductStockId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(maxLength: 128, nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    IsSold = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStock", x => x.ProductStockId);
                    table.ForeignKey(
                        name: "FK_ProductStock_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseAdjustment",
                columns: table => new
                {
                    PurchaseAdjustmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    PurchaseId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 128, nullable: false),
                    AdjustmentStatus = table.Column<string>(maxLength: 128, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
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

            migrationBuilder.CreateTable(
                name: "SellingAdjustment",
                columns: table => new
                {
                    SellingAdjustmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    SellingId = table.Column<int>(nullable: false),
                    ProductStockId = table.Column<int>(nullable: false),
                    AdjustmentStatus = table.Column<string>(maxLength: 128, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingAdjustment", x => x.SellingAdjustmentId);
                    table.ForeignKey(
                        name: "FK_SellingAdjustment_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingAdjustment_ProductStock",
                        column: x => x.ProductStockId,
                        principalTable: "ProductStock",
                        principalColumn: "ProductStockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingAdjustment_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingAdjustment_Selling",
                        column: x => x.SellingId,
                        principalTable: "Selling",
                        principalColumn: "SellingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellingList",
                columns: table => new
                {
                    SellingListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellingId = table.Column<int>(nullable: false),
                    ProductStockId = table.Column<int>(nullable: false),
                    SellingPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingList", x => x.SellingListId);
                    table.ForeignKey(
                        name: "FK_SellingList_ProductStock",
                        column: x => x.ProductStockId,
                        principalTable: "ProductStock",
                        principalColumn: "ProductStockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellingList_Selling",
                        column: x => x.SellingId,
                        principalTable: "Selling",
                        principalColumn: "SellingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "837d221e-3d39-4651-b239-66e68d57edb2", "c6d562f5-ba4f-4520-9394-792a5f25c409", "admin", "ADMIN" },
                    { "1de93e58-ab8f-4500-8e41-f73d0d9a4edf", "0145818c-8cc2-455c-b5ef-2ff6f9864252", "sub-admin", "SUB-ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "24bca771-762d-457c-8aff-cdd661803190", 0, "541754d1-3242-44e1-92a5-b1912e7e62b1", "admin@gmail.com", true, true, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEHghrQ379+HRhnNR7BTluhOFOY8aUpxUZnxbb5mlwDDQxQvhfiQYBJRWIkxZ5dFvGg==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "Institution",
                columns: new[] { "InstitutionId", "Address", "City", "DialogTitle", "Email", "Established", "InstitutionLogo", "InstitutionName", "LocalArea", "Phone", "PostalCode", "State", "Website" },
                values: new object[] { 1, null, null, null, null, null, null, "Institution", null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "Registration",
                columns: new[] { "RegistrationId", "Address", "DateofBirth", "Designation", "Email", "FatherName", "Image", "Name", "NationalID", "Phone", "PS", "Type", "UserName", "Validation" },
                values: new object[] { 1, null, null, null, null, null, null, "Admin", null, null, "Admin_121", "Admin", "Admin", true });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "24bca771-762d-457c-8aff-cdd661803190", "837d221e-3d39-4651-b239-66e68d57edb2" });

            migrationBuilder.CreateIndex(
                name: "IX_Expanse_ExpanseCategoryId",
                table: "Expanse",
                column: "ExpanseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expanse_RegistrationId",
                table: "Expanse",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PageLink_LinkCategoryId",
                table: "PageLink",
                column: "LinkCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PageLinkAssign_LinkId",
                table: "PageLinkAssign",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_PageLinkAssign_RegistrationId",
                table: "PageLinkAssign",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductCatalogId",
                table: "Product",
                column: "ProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_PurchaseId",
                table: "Product",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCatalog_CatalogTypeId",
                table: "ProductCatalog",
                column: "CatalogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCatalog_ParentId",
                table: "ProductCatalog",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_ProductId",
                table: "ProductStock",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_RegistrationId",
                table: "Purchase",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_VendorId",
                table: "Purchase",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAdjustment_ProductId",
                table: "PurchaseAdjustment",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAdjustment_PurchaseId",
                table: "PurchaseAdjustment",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayment_RegistrationId",
                table: "PurchasePayment",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayment_VendorId",
                table: "PurchasePayment",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePaymentList_PurchaseId",
                table: "PurchasePaymentList",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePaymentList_PurchasePaymentId",
                table: "PurchasePaymentList",
                column: "PurchasePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Selling_CustomerId",
                table: "Selling",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Selling_RegistrationId",
                table: "Selling",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingAdjustment_ProductId",
                table: "SellingAdjustment",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingAdjustment_ProductStockId",
                table: "SellingAdjustment",
                column: "ProductStockId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingAdjustment_RegistrationId",
                table: "SellingAdjustment",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingAdjustment_SellingId",
                table: "SellingAdjustment",
                column: "SellingId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingList_ProductStockId",
                table: "SellingList",
                column: "ProductStockId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingList_SellingId",
                table: "SellingList",
                column: "SellingId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPayment_CustomerId",
                table: "SellingPayment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPayment_RegistrationId",
                table: "SellingPayment",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPaymentList_SellingId",
                table: "SellingPaymentList",
                column: "SellingId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPaymentList_SellingPaymentId",
                table: "SellingPaymentList",
                column: "SellingPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CustomerId",
                table: "Service",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_RegistrationId",
                table: "Service",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDevice_CustomerId",
                table: "ServiceDevice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDevice_ProductCatalogId",
                table: "ServiceDevice",
                column: "ProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceList_ServiceDeviceId",
                table: "ServiceList",
                column: "ServiceDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceList_ServiceId",
                table: "ServiceList",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePaymentList_SellingPaymentId",
                table: "ServicePaymentList",
                column: "SellingPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePaymentList_ServiceId",
                table: "ServicePaymentList",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expanse");

            migrationBuilder.DropTable(
                name: "Institution");

            migrationBuilder.DropTable(
                name: "PageLinkAssign");

            migrationBuilder.DropTable(
                name: "PurchaseAdjustment");

            migrationBuilder.DropTable(
                name: "PurchasePaymentList");

            migrationBuilder.DropTable(
                name: "SellingAdjustment");

            migrationBuilder.DropTable(
                name: "SellingList");

            migrationBuilder.DropTable(
                name: "SellingPaymentList");

            migrationBuilder.DropTable(
                name: "ServiceList");

            migrationBuilder.DropTable(
                name: "ServicePaymentList");

            migrationBuilder.DropTable(
                name: "ExpanseCategory");

            migrationBuilder.DropTable(
                name: "PageLink");

            migrationBuilder.DropTable(
                name: "PurchasePayment");

            migrationBuilder.DropTable(
                name: "ProductStock");

            migrationBuilder.DropTable(
                name: "Selling");

            migrationBuilder.DropTable(
                name: "ServiceDevice");

            migrationBuilder.DropTable(
                name: "SellingPayment");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "PageLinkCategory");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ProductCatalog");

            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "ProductCatalogType");

            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.DropTable(
                name: "Vendor");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1de93e58-ab8f-4500-8e41-f73d0d9a4edf");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "24bca771-762d-457c-8aff-cdd661803190", "837d221e-3d39-4651-b239-66e68d57edb2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "837d221e-3d39-4651-b239-66e68d57edb2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24bca771-762d-457c-8aff-cdd661803190");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b0515de6-18b2-4e8f-bd30-e336dd981867", "bb556a99-c8ee-44da-8e68-6cd90a93c56a", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0ba9dfc5-98c2-4714-b981-e88ff495175a", "60f5a527-9b56-4a4f-9f3e-448a0ff24a8d", "sub-admin", "sub-admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6e6c6586-b81a-4c22-84d1-e6f900f9bd1d", 0, "eed120b8-5d74-4f37-9196-5a4f4dcd41f2", "admin@gmail.com", true, false, null, "admin@gmail.com", "admin", "AQAAAAEAACcQAAAAEHQeqMf4pTUYtyuHT+vaLqHizQWVvoES7kkdvnrLPcb1bQxs2TVCF65Nld4FjNImtQ==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "6e6c6586-b81a-4c22-84d1-e6f900f9bd1d", "b0515de6-18b2-4e8f-bd30-e336dd981867" });
        }
    }
}
