using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Admin
{
    public class ReportModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ISystemAccountRepository _accountRepo; // Add this

        public ReportModel(INewsArticleRepository newsArticleRepository, ISystemAccountRepository accountRepo)
        {
            _newsArticleRepository = newsArticleRepository;
            _accountRepo = accountRepo; // Initialize it
        }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public List<NewsArticle> NewsArticles { get; set; } = new();
        public Dictionary<short, string> UpdatedByNames { get; set; } = new(); // Add this

        public async Task<IActionResult> OnPostAsync()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError(string.Empty, "Start Date cannot be later than End Date.");
                return Page();
            }

            NewsArticles = await _newsArticleRepository.GetNewsArticlesByDateRangeAsync(StartDate, EndDate);

            if (NewsArticles == null || NewsArticles.Count == 0)
            {
                TempData["ErrorMessage"] = "No articles found for the selected date range.";
                return RedirectToPage();
            }

            var updatedByIds = NewsArticles
                .Where(a => a.UpdatedById.HasValue)
                .Select(a => a.UpdatedById.Value)
                .Distinct()
                .ToList();

            foreach (var id in updatedByIds)
            {
                var account = await _accountRepo.GetAccountById(id);
                UpdatedByNames[id] = account?.AccountName ?? "Unknown";
            }

            return Page();
        }
    }
}
