using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Repository
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        public RegistrationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public int GetRegID_ByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return -1;

            return Context.Registration.FirstOrDefault(r => r.UserName == userName).RegistrationId;
        }

        public ICollection<AdminInfo> GetSubAdminList()
        {
            return Context.Registration.Where(r => r.Type == UserType.SubAdmin.ToString()).Select(r => new AdminInfo
            {
                UserName = r.UserName,
                Name = r.Name,
                RegistrationId = r.RegistrationId,
                Type = r.Type,
                FatherName = r.FatherName,
                Address = r.Address,
                DateofBirth = r.DateofBirth,
                Designation = r.Designation,
                Email = r.Email,
                Image = r.Image,
                NationalId = r.NationalId,
                Phone = r.Phone,
                Validation = r.Validation.Value,
                Balance = r.Balance, 
                Password = r.Ps
            }).ToList();
        }

        public ICollection<AdminInfo> GetSalesPersonList()
        {
            return Context.Registration.Where(r => r.Type == UserType.SalesPerson.ToString()).Select(r => new AdminInfo
            {
                UserName = r.UserName,
                Name = r.Name,
                RegistrationId = r.RegistrationId,
                Type = r.Type,
                FatherName = r.FatherName,
                Address = r.Address,
                DateofBirth = r.DateofBirth,
                Designation = r.Designation,
                Email = r.Email,
                Image = r.Image,
                NationalId = r.NationalId,
                Phone = r.Phone,
                Validation = r.Validation.Value,
                Balance = r.Balance, 
                Password = r.Ps
            }).ToList();
        }

        public ICollection<DDL> SubAdmins()
        {
            return Context.Registration?.Where(r => r.Type == UserType.SubAdmin.ToString()).Select(r =>
                new DDL { value = r.RegistrationId, label = r.Name + " (" + r.UserName + ")" }).ToList();
        }

        public AdminBasic GetAdminBasic(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            return Context.Registration.Where(r => r.UserName == userName).Select(r => new AdminBasic
            {
                Name = r.Name,
                Image = r.Image,
                RegistrationId = r.RegistrationId,
                Type = r.Type
            }).FirstOrDefault();
        }

        public AdminInfo GetAdminInfo(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            return Context.Registration.Where(r => r.UserName == userName).Select(r => new AdminInfo
            {
                RegistrationId = r.RegistrationId,
                Name = r.Name,
                UserName = r.UserName,
                Type = r.Type,
                Image = r.Image,
                FatherName = r.FatherName,
                Designation = r.Designation,
                DateofBirth = r.DateofBirth,
                NationalId = r.NationalId,
                Address = r.Address,
                Phone = r.Phone,
                Email = r.Email,
                Balance = r.Balance,
                Validation = r.Validation.Value
            }).FirstOrDefault();
        }

        public AdminInfo GetSalesPersonInfo(int registrationId)
        {
            return Context.Registration.Where(r => r.RegistrationId == registrationId).Select(r => new AdminInfo
            {
                RegistrationId = r.RegistrationId,
                Name = r.Name,
                UserName = r.UserName,
                Type = r.Type,
                Image = r.Image,
                FatherName = r.FatherName,
                Designation = r.Designation,
                DateofBirth = r.DateofBirth,
                NationalId = r.NationalId,
                Address = r.Address,
                Phone = r.Phone,
                Email = r.Email,
                Balance = r.Balance,
                Validation = r.Validation.Value
            }).FirstOrDefault();
        }


        public void UpdateCustom(string userName, AdminInfo reg)
        {
            var r = Context.Registration.FirstOrDefault(u => u.UserName == userName);

            r.FatherName = reg.FatherName;
            r.Name = reg.Name;
            r.Phone = reg.Phone;
            r.Email = reg.Email;
            if (reg.Image != null)
                r.Image = reg.Image;
            r.Address = reg.Address;
            r.Designation = reg.Designation;
            r.NationalId = reg.NationalId;
            r.DateofBirth = reg.DateofBirth;

            Update(r);
        }

        public UserType UserTypeByUserName(string userName)
        {
            var type = Context.Registration.FirstOrDefault(r => r.UserName == userName).Type;

            UserType userType;
            Enum.TryParse(type, out userType);

            return userType;
        }

        public decimal Balance(string userName)
        {
            return Context.Registration.FirstOrDefault(r => r.UserName == userName)?.Balance ?? 0;
        }

        public void BalanceAdd(int registrationId, decimal amount)
        {
            var account = Context.Registration.Find(registrationId);
            account.Balance += amount;
            Context.Registration.Update(account);
        }

        public void BalanceSubtract(int registrationId, decimal amount)
        {
            var account = Context.Registration.Find(registrationId);
            account.Balance -= amount;
            Context.Registration.Update(account);
        }

        public DbResponse ValidationChange(int registrationId)
        {
            var registration = Context.Registration.Find(registrationId);
            if (registration == null) return new DbResponse(false, "User Not Found");
            registration.Validation = !registration.Validation;
            Context.Registration.Update(registration);
            Context.SaveChanges();
            return new DbResponse(true,
                registration.Validation.Value ? "User Access Unlock Successfully" : "User Access Lock Successfully");
        }

        public bool GetValidation(string userName)
        {
            return Context.Registration.FirstOrDefault(r => r.UserName.ToLower() == userName.ToLower())?.Validation ??
                   false;
        }

        public void PasswordChanged(string userName, string password)
        {
            var registration = Context.Registration.FirstOrDefault(r => r.UserName == userName);

            if (registration == null) return;
            registration.Ps = password;
            Context.Registration.Update(registration);
            Context.SaveChanges();
        }
    }
}