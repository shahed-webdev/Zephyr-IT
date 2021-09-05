using InventoryManagement.Data;

namespace InventoryManagement.Repository
{
    public interface IInstitutionRepository : IRepository<Institution>
    {
        void UpdateCustom(InstitutionVM model);
        InstitutionVM FindCustom();
        HomeVM HomeInfo();
        int GetVoucherCountdown();
        void IncreaseVoucherCount();
        CapitalReportModel CapitalReport();
    }
}
