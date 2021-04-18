using System.ComponentModel;

namespace InventoryManagement.Data
{
    public enum UserType
    {
        [Description("Admin")]
        Admin,

        [Description("SubAdmin")]
        SubAdmin,

        [Description("SalesPerson")]
        SalesPerson
    }
}