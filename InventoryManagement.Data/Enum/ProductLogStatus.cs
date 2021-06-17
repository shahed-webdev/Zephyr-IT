using System.ComponentModel;

namespace InventoryManagement.Data
{
    public enum ProductLogStatus
    {
        [Description("Buy")]
        Buy,

        [Description("Sale")]
        Sale,

        [Description("Return")]
        Return,

        [Description("Warranty Acceptance")]
        WarrantyAcceptance,

        [Description("Warranty Delivery")]
        WarrantyDelivery,

        [Description("Product Damaged")]
        Damaged,

        [Description("Purchase Bill Update")]
        PurchaseUpdate
    }
}