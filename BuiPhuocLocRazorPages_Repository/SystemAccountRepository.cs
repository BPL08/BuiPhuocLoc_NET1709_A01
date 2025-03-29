using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_Repository
{
    public class SystemAccountRepository:ISystemAccountRepository
    {
        public Task<SystemAccount?> LoginAsync(string email, string password)
            => SystemAccountDAO.Instance.AuthenticateAsync(email, password);

        public Task<List<SystemAccount>> GetAllAccountsAsync() =>
            SystemAccountDAO.Instance.GetAllAccountsAsync();

        public async Task<bool> AccountExistsAsync(short id)
        {
            var account = await SystemAccountDAO.Instance.GetAccountByIdAsync(id);
            return account != null;
        }
        public async Task<SystemAccount> GetAccountById(short id)
        {
            SystemAccount account = await SystemAccountDAO.Instance.GetAccountByIdAsync(id);
            return account;
        }

        public Task CreateAccountAsync(SystemAccount account) =>
            SystemAccountDAO.Instance.CreateAccountAsync(account);

        public async Task UpdateAccountAsync(SystemAccount account)
        {
            SystemAccount foundedaccount = await SystemAccountDAO.Instance.GetAccountByIdAsync(account.AccountId);
            foundedaccount.AccountName = account.AccountName;
            foundedaccount.AccountEmail = account.AccountEmail;
            foundedaccount.AccountRole = account.AccountRole;
            foundedaccount.AccountPassword = account.AccountPassword;   
            await SystemAccountDAO.Instance.UpdateAccountAsync(foundedaccount);
        }

        public Task DeleteAccountAsync(short id) =>
            SystemAccountDAO.Instance.DeleteAccountAsync(id);

        public Task<List<SystemAccount>> SearchAccountsAsync(string searchTerm) =>
            SystemAccountDAO.Instance.SearchAccountsAsync(searchTerm);


        public async Task<bool> UpdateProfileAsync(SystemAccount account)
        {
            var existingAccount = await SystemAccountDAO.Instance.GetAccountByIdAsync(account.AccountId);
            if (existingAccount == null) return false;
            existingAccount.AccountName = account.AccountName;
            existingAccount.AccountEmail = account.AccountEmail;
            existingAccount.AccountPassword = account.AccountPassword;

            await SystemAccountDAO.Instance.UpdateAccountAsync(existingAccount);
            return true;
        }
    }
}
