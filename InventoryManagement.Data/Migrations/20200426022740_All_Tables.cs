using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Data.Migrations
{
    public partial class All_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                name: "ExpenseCategory",
                columns: table => new
                {
                    ExpenseCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(maxLength: 128, nullable: false),
                    TotalExpense = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategory", x => x.ExpenseCategoryId);
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Expense",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(nullable: false),
                    ExpenseCategoryId = table.Column<int>(nullable: false),
                    ExpenseAmount = table.Column<double>(nullable: false),
                    ExpenseFor = table.Column<string>(maxLength: 256, nullable: true),
                    ExpensePaymentMethod = table.Column<string>(maxLength: 50, nullable: true),
                    ExpenseDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expense_ExpenseCategory",
                        column: x => x.ExpenseCategoryId,
                        principalTable: "ExpenseCategory",
                        principalColumn: "ExpenseCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense_Registration",
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseCategoryId",
                table: "Expense",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_RegistrationId",
                table: "Expense",
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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Expense");

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
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ExpenseCategory");

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
        }
    }
}
