﻿using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public class ProductStockViewModel
    {
        public string ProductCode { get; set; }
    }

    public class ProductStockDetailsViewModel
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductCatalogName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public int? SellingSn { get; set; }
        public int? SellingId { get; set; }
        public int? PurchaseId { get; set; }
    }

    public class ProductStockReportModel
    {
        public ProductStockReportModel()
        {
            ProductList = new List<ProductStockListModel>();
        }
        public int ProductCatalogId { get; set; }
        public string ProductCatalogName { get; set; }
        public List<ProductStockListModel> ProductList { get; set; }
    }

    public class ProductStockListModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string Note { get; set; }
        public decimal SellingPrice { get; set; }
        public string[] ProductCodes { get; set; }
    }

}