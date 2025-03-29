using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Admin
{
    public class ManageAccountModel : PageModel
    {
        private readonly ISystemAccountRepository _repo;

        public ManageAccountModel(ISystemAccountRepository repo)
        {
            _repo = repo;
        }

        public List<SystemAccount> Accounts { get; set; }
        [BindProperty(SupportsGet = true)] public string SearchTerm { get; set; }


        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            if (!IsAdmin())
            {
                return RedirectToPage("/Error");
            }
            await _repo.DeleteAccountAsync(id);
            return RedirectToPage("/Admin/ManageAccount");
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            return role == 1;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAdmin())
            {
                return RedirectToPage("/Error");
            }
            if (string.IsNullOrEmpty(SearchTerm))
            {
                Accounts = await _repo.GetAllAccountsAsync();
            }
            else
            {
                Accounts = await _repo.SearchAccountsAsync(SearchTerm);
            }
            return Page();
        }

        
    }
}
