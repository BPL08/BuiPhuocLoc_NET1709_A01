using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Admin
{
    public class ReportModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;

        public ReportModel(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public List<NewsArticle> NewsArticles { get; set; } = new();

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

            return Page();
        }
    }
}
