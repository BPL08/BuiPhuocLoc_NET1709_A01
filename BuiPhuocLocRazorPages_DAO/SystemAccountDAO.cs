using BuiPhuocLocRazorPages_BO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_DAO
{
    public class SystemAccountDAO
    {
        private FunewsManagementContext _context;
        private static SystemAccountDAO instance;
        public static SystemAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemAccountDAO();
                }
                return instance;
            }
        }
        public SystemAccountDAO()
        {
            _context = new FunewsManagementContext();
        }
        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {

            SystemAccount account =await _context.SystemAccounts
                .SingleOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
            return account;
        }

        public async Task<List<SystemAccount>> GetAllAccountsAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }

        public async Task<SystemAccount?> GetAccountByIdAsync(short id)
        {
            return await _context.SystemAccounts.FindAsync(id);
        }

        public async Task CreateAccountAsync(SystemAccount account)
        {
            _context.SystemAccounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(SystemAccount account)
        {
            _context.SystemAccounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(short id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account != null)
            {
                _context.SystemAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<SystemAccount>> SearchAccountsAsync(string searchTerm)
        {
            return await _context.SystemAccounts
                .Where(a => a.AccountEmail.Contains(searchTerm) || a.AccountName.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
