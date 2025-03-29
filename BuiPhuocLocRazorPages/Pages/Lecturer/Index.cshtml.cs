using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Lecturer
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ISystemAccountRepository _accountRepository;

        public IndexModel(INewsArticleRepository newsArticleRepository, ISystemAccountRepository accountRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _accountRepository = accountRepository;
        }

        public List<NewsArticle> NewsArticles { get; set; } = new();
        public Dictionary<short, string> UpdatedByNames { get; set; } = new(); // Map UpdatedById to AccountName
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        private const int PageSize = 5;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;

            // Fetch active news articles
            var (articles, totalPages) = await _newsArticleRepository.GetActiveNewsArticlesAsync(pageNumber, PageSize);
            NewsArticles = articles;
            TotalPages = totalPages;

            // Fetch account names for UpdatedById
            var updatedByIds = NewsArticles
                .Where(a => a.UpdatedById.HasValue)
                .Select(a => a.UpdatedById.Value)
                .Distinct()
                .ToList();

            foreach (var id in updatedByIds)
            {
                var account = await _accountRepository.GetAccountById(id);
                if (account != null)
                {
                    UpdatedByNames[id] = account.AccountName;
                }
                else
                {
                    UpdatedByNames[id] = "Unknown";
                }
            }
        }
    }
}
