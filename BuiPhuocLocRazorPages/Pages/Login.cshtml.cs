using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public LoginModel(ISystemAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var adminEmail = _configuration["DefaultAdmin:Email"];
            var adminPassword = _configuration["DefaultAdmin:Password"];
            var account = await _accountRepository.LoginAsync(Email, Password);

            if (Email == adminEmail && Password == adminPassword)
            {
                HttpContext.Session.SetInt32("AccountRole", 0); 
                return RedirectToPage("/Admin/Index");
            }


            if (account == null)
            {
                ErrorMessage = "Email or password is not right.";
                return Page();
            }
            HttpContext.Session.SetInt32("AccountId", account.AccountId);
            HttpContext.Session.SetString("AccountEmail", account.AccountEmail ?? "");
            HttpContext.Session.SetInt32("AccountRole", account.AccountRole ?? 0);
            if (account.AccountRole == 2)
            {
                return RedirectToPage("/Lecturer/Index");
            }
            else if (account.AccountRole == 1)
            {
                return RedirectToPage("/Staff/NavigatePage");
            }

            ErrorMessage = "Role is not valid";
            return Page();
        }
    }
}
