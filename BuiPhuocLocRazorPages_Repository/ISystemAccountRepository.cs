using BuiPhuocLocRazorPages_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_Repository
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount?> LoginAsync(string email, string password);

        Task<List<SystemAccount>> GetAllAccountsAsync();
        Task<bool> AccountExistsAsync(short id);
        Task CreateAccountAsync(SystemAccount account);
        Task UpdateAccountAsync(SystemAccount account);
        Task DeleteAccountAsync(short id);
        Task<List<SystemAccount>> SearchAccountsAsync(string searchTerm);

        Task<SystemAccount> GetAccountById(short id);

        Task<bool> UpdateProfileAsync(SystemAccount account);
    }
}
