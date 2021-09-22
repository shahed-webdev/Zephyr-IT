using InventoryManagement.Data;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class InstitutionRepository : Repository<Institution>, IInstitutionRepository
    {
        public InstitutionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void UpdateCustom(InstitutionVM model)
        {
            var ins = Find(model.InstitutionId);

            ins.InstitutionName = model.InstitutionName;
            ins.Address = model.Address;
            ins.Phone = model.Phone;
            ins.Email = model.Email;
            ins.Website = model.Website;
            if (model.InstitutionLogo != null)
                ins.InstitutionLogo = model.InstitutionLogo;
            Update(ins);
        }

        public InstitutionVM FindCustom()
        {
            var ins = Context.Institution.First();

            if (ins == null) return null;
            return new InstitutionVM
            {
                InstitutionId = ins.InstitutionId,
                InstitutionName = ins.InstitutionName,
                Address = ins.Address,
                Phone = ins.Phone,
                Email = ins.Email,
                Website = ins.Website,
                InstitutionLogo = ins.InstitutionLogo
            };
        }

        public HomeVM HomeInfo()
        {
            var ins = Context.Institution.First();

            if (ins == null) return null;
            return new HomeVM
            {
                Address = ins.Address,
                Phone = ins.Phone,
                Email = ins.Email
            };
        }

        public int GetVoucherCountdown()
        {
            var ins = Context.Institution.First();
            return ins.VoucherCountdown;
        }

        public void IncreaseVoucherCount()
        {
            var ins = Context.Institution.First();
            ins.VoucherCountdown++;
            Update(ins);
        }

        public CapitalReportModel CapitalReport()
        {
            return Context.VW_CapitalProfitReport.Select(c => new CapitalReportModel
            {
                NetProfit = c.MonthlyNetProfit,
                LiquidAmount = c.AccountLiquid
            }).FirstOrDefault();
        }
    }
}
