using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Admin
{
    public class UpdateAccountModel : PageModel
    {
        private readonly ISystemAccountRepository _repo;

        public UpdateAccountModel(ISystemAccountRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public SystemAccount Account { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            short Id= (short)id;
            var existingAccount = await _repo.GetAccountById(Id);
            if (existingAccount == null)
            {
                ErrorMessage = "Account not found.";
                return RedirectToPage("/Admin/ManageAccount");
            }
            Account = existingAccount;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            if (!await _repo.AccountExistsAsync(Account.AccountId))
            {
                ErrorMessage = "Account no longer exists.";
                return Page();
            }
            try
            {
                await _repo.UpdateAccountAsync(Account);
                return RedirectToPage("/Admin/ManageAccount");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error updating account: " + ex.Message;
                return Page();
            }
        }
    }
}
