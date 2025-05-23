using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Staff
{
    public class UpdateProfileModel : PageModel
    {
        private readonly ISystemAccountRepository _accountRepository;

        public UpdateProfileModel(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [BindProperty]
        public short AccountId { get;  set; } // Changed to short

        [BindProperty]
        public string AccountName { get; set; }

        [BindProperty]
        public string AccountEmail { get; set; }

        [BindProperty]
        public string AccountPassword { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId.HasValue)
            {
                AccountId = (short)accountId.Value; 
                var account = await _accountRepository.GetAccountById(AccountId);
                if (account != null)
                {
                    AccountName = account.AccountName;
                    AccountEmail = account.AccountEmail;
                }
                else
                {
                    ErrorMessage = "Account not found.";
                }
            }
            else
            {
                ErrorMessage = "Account ID is missing or invalid.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId.HasValue)
            {
                AccountId = (short)accountId.Value;
            }
            else
            {
                ErrorMessage = "Account ID is missing or invalid.";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var account = await _accountRepository.GetAccountById(AccountId);
            if (account == null)
            {
                ErrorMessage = "Account not found.";
                return Page();
            }

            account.AccountName = AccountName;
            account.AccountEmail = AccountEmail;
            account.AccountPassword = AccountPassword;

            var result = await _accountRepository.UpdateProfileAsync(account);
            if (result)
            {
                return RedirectToPage("UpdateProfile");
            }
            else
            {
                ErrorMessage = "Failed to update account information. Please try again.";
                return Page();
            }
        }

    }
}
