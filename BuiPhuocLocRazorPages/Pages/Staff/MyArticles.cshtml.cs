using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Staff
{
    public class MyArticlesModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ISystemAccountRepository _accountRepository;

        public MyArticlesModel(INewsArticleRepository newsArticleRepository, ISystemAccountRepository accountRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _accountRepository = accountRepository;
        }

        public List<NewsArticle> NewsArticles { get; set; } = new();
        public Dictionary<short, string> UpdatedByNames { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        private const int PageSize = 2;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var creatorId = HttpContext.Session.GetInt32("AccountId");
            if (creatorId.HasValue)
            {
                var (articles, totalPages) = await _newsArticleRepository.GetPaginatedArticlesByCreatorIdAsync(
                    (short)creatorId.Value, pageNumber, PageSize);

                NewsArticles = articles;
                CurrentPage = pageNumber;
                TotalPages = totalPages;
                var updatedByIds = NewsArticles
                    .Where(a => a.UpdatedById.HasValue)
                    .Select(a => a.UpdatedById.Value)
                    .Distinct()
                    .ToList();

                foreach (var id in updatedByIds)
                {
                    var account = await _accountRepository.GetAccountById(id);
                    UpdatedByNames[id] = account?.AccountName ?? "Unknown";
                }
            }
        }
       }
}
