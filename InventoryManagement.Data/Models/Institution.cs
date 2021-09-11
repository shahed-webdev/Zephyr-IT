using System;

namespace InventoryManagement.Data
{
    public partial class Institution
    {
        public int InstitutionId { get; set; }
        public int VoucherCountdown { get; set; }
        public string InstitutionName { get; set; }
        public string DialogTitle { get; set; }
        public string Established { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string LocalArea { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public byte[] InstitutionLogo { get; set; }
        public DateTime InsertDate { get; set; }
        public int? DefaultAccountId { get; set; }
        public Account Account { get; set; }
        public decimal Capital { get; set; }
    }
}
