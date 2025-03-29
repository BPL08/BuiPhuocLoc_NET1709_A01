using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuiPhuocLocRazorPages.Pages.Staff
{
    public class MyArticlesModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;

        public MyArticlesModel(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public List<NewsArticle> NewsArticles { get; set; } = new();

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        private const int PageSize = 2; 

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var creatorId = HttpContext.Session.GetInt32("AccountId");
            if (creatorId.HasValue)
            {
                var (articles, totalPages) = await _newsArticleRepository.GetPaginatedArticlesByCreatorIdAsync((short)creatorId.Value, pageNumber, PageSize);

                NewsArticles = articles;
                CurrentPage = pageNumber;
                TotalPages = totalPages;
            }
        }
    }
}
