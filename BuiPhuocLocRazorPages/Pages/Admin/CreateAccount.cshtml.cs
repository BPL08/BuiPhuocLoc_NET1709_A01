using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Admin
{
    public class CreateAccountModel : PageModel
    {
        private readonly ISystemAccountRepository _repo;

        public CreateAccountModel(ISystemAccountRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public SystemAccount Account { get; set; } = new SystemAccount();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            bool validate=await _repo.AccountExistsAsync(Account.AccountId);
            if(validate)
            {
                ErrorMessage = "There is already an account with that id";
                return Page();
            }
            try
            {
                await _repo.CreateAccountAsync(Account);
                return RedirectToPage("/Admin/ManageAccount");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error creating account: " + ex.Message;
                return Page();
            }
        }
    }
}
