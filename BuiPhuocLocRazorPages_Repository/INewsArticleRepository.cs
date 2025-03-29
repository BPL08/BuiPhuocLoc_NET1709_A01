using BuiPhuocLocRazorPages_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_Repository
{
    public interface INewsArticleRepository
    {
        Task<(List<NewsArticle> Articles, int TotalPages)> GetAllNewsArticlesAsync(int pageNumber, int pageSize);

        Task<bool> IsNewsArticleExistsAsync(string id);
        Task CreateNewsArticleAsync(NewsArticle article, List<Tag> selectedTags);

        Task UpdateNewsAsync(NewsArticle article, List<int> selectedTagIds);
        Task<bool> DeleteNewsArticleAsync(string id);

        Task<(List<NewsArticle> Articles, int TotalPages)> SearchNewsArticlesAsync(string searchTerm, int pageNumber, int pageSize);

        Task AddTagsToNewsArticleAsync(string articleId, List<Tag> tags);

        Task<NewsArticle> GetNewsArticleByIdAsync(string id);

        Task<(List<NewsArticle> Articles, int TotalPages)> GetPaginatedArticlesByCreatorIdAsync(short creatorId, int pageNumber, int pageSize);

        Task<List<NewsArticle>> GetNewsArticlesByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<(List<NewsArticle> Articles, int TotalPages)> GetActiveNewsArticlesAsync(int pageNumber, int pageSize);
    }
}
