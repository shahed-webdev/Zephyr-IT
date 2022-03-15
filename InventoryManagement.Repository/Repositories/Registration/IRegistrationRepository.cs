using InventoryManagement.Data;
using System.Collections.Generic;

namespace InventoryManagement.Repository
{
    public interface IRegistrationRepository : IRepository<Registration>
    {
        int GetRegID_ByUserName(string userName);
        ICollection<DDL> SubAdmins();
        ICollection<AdminInfo> GetSubAdminList();
        ICollection<AdminInfo> GetSalesPersonList();
        AdminBasic GetAdminBasic(string userName);
        AdminInfo GetAdminInfo(string userName);
        AdminInfo GetSalesPersonInfo(int registrationId);
        void UpdateCustom(string userName, AdminInfo reg);
        UserType UserTypeByUserName(string userName);
        decimal Balance(string userName);
        void BalanceAdd(int registrationId, decimal amount);
        void BalanceSubtract(int registrationId, decimal amount);
        DbResponse ValidationChange(int registrationId);
        bool GetValidation(string userName);
        void PasswordChanged(string userName, string password);
    }
}
