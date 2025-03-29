using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages.Pages.SignalR;
using BuiPhuocLocRazorPages_DAO;
using Microsoft.AspNetCore.SignalR;
using BuiPhuocLocRazorPages_Repository;

namespace BuiPhuocLocRazorPages.Pages.Staff.NewsPages
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepository _repo;
        private readonly ITagRepository _tagRepo;
        private readonly IHubContext<NewsHub> _hubContext;
        private readonly ISystemAccountRepository _accountRepo;

        public IndexModel(INewsArticleRepository repo, ITagRepository tagRepo, IHubContext<NewsHub> hubContext, ISystemAccountRepository accountRepo)
        {
            _repo = repo;
            _tagRepo = tagRepo;
            _hubContext = hubContext;
            _accountRepo = accountRepo;
        }

        public List<NewsArticle> NewsArticles { get; set; }
        public Dictionary<short, string> UpdatedByNames { get; set; } = new(); 
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }

        public int pageSize { get; set; } = 10;

        public async Task OnGetAsync(int? pageNumber, string searchTerm)
        {
            CurrentPage = pageNumber ?? 1;
            SearchTerm = searchTerm;

            (List<NewsArticle> articles, int totalPages) result;

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                // Perform search
                result = await _repo.SearchNewsArticlesAsync(SearchTerm, CurrentPage, pageSize);
            }
            else
            {
                result = await _repo.GetAllNewsArticlesAsync(CurrentPage, pageSize);
            }

            NewsArticles = result.articles;
            TotalPages = result.totalPages;

            foreach (var article in NewsArticles)
            {
                article.Tags = await _tagRepo.GetTagsByNewsArticleIdAsync(article.NewsArticleId);
            }

            var updatedByIds = NewsArticles
                .Where(a => a.UpdatedById.HasValue)
                .Select(a => a.UpdatedById.Value)
                .Distinct()
                .ToList();

            foreach (var id in updatedByIds)
            {
                var account = await _accountRepo.GetAccountById(id);
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
