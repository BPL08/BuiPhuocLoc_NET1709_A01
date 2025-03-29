using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_Repository
{
    public class NewsArticleRepository : INewsArticleRepository
    {


        public async Task<(List<NewsArticle> Articles, int TotalPages)> GetAllNewsArticlesAsync(int pageNumber, int pageSize)
        {
            var allArticles = await NewsArticleDAO.Instance.GetAllNewsArticlesAsync();
            int totalPages = (allArticles.Count + pageSize - 1) / pageSize;

            var paginatedArticles = allArticles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (paginatedArticles, totalPages);
        }

        public async Task<bool> IsNewsArticleExistsAsync(string id)
        {
            var article = await NewsArticleDAO.Instance.GetNewsArticleByIdAsync(id);
            return article != null;
        }


        public async Task CreateNewsArticleAsync(NewsArticle article, List<Tag> selectedTags)
        {
            if (await NewsArticleDAO.Instance.GetNewsArticleByIdAsync(article.NewsArticleId) != null)
            {
                throw new InvalidOperationException("A news article with this ID already exists.");
            }

            await NewsArticleDAO.Instance.CreateNewsArticleAsync(article);

            if (selectedTags != null && selectedTags.Any())
            {
                await NewsArticleDAO.Instance.UpdateNewsArticleTagsAsync(article.NewsArticleId, selectedTags);
            }
        }


        public async Task UpdateNewsAsync(NewsArticle article, List<int> selectedTagIds)
        {
            try
            {
               
                var existingArticle = await NewsArticleDAO.Instance.GetNewsArticleWithTagsAsync(article.NewsArticleId);
                if (existingArticle == null)
                {
                    throw new KeyNotFoundException("News article not found.");
                }
                existingArticle.NewsTitle = article.NewsTitle;
                existingArticle.Headline = article.Headline;
                existingArticle.NewsContent = article.NewsContent;
                existingArticle.NewsSource = article.NewsSource;
                existingArticle.CategoryId = article.CategoryId;
                existingArticle.NewsStatus = article.NewsStatus;
                existingArticle.ModifiedDate = article.ModifiedDate ?? DateTime.Now;
                existingArticle.UpdatedById = article.UpdatedById;
                existingArticle.Tags.Clear();
                if (selectedTagIds != null && selectedTagIds.Any())
                {
                    var tagsToAdd = await NewsArticleDAO.Instance.GetTagsByIdsAsync(selectedTagIds);
                    foreach (var tag in tagsToAdd)
                    {
                        existingArticle.Tags.Add(tag); 
                    }
                }
                await NewsArticleDAO.Instance.SaveChangesAsync();
                NewsArticleDAO.Instance.DetachEntity(existingArticle);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating news article: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteNewsArticleAsync(string id)
        {
            var existingArticle = await NewsArticleDAO.Instance.GetNewsArticleByIdAsync(id);
            if (existingArticle == null)
            {
                return false;
            }

            await NewsArticleDAO.Instance.DeleteNewsArticleAsync(id);
            return true;
        }

        public async Task<(List<NewsArticle> Articles, int TotalPages)> SearchNewsArticlesAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var allArticles = await NewsArticleDAO.Instance.GetAllNewsArticlesAsync();
            var filteredArticles = allArticles
                .Where(a => a.NewsTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                         || a.Headline.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            int totalPages = (filteredArticles.Count + pageSize - 1) / pageSize;

            var paginatedArticles = filteredArticles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (paginatedArticles, totalPages);
        }
        public async Task AddTagsToNewsArticleAsync(string articleId, List<Tag> tags)
        {
            await NewsArticleDAO.Instance.UpdateNewsArticleTagsAsync(articleId, tags);
        }
        public async Task<NewsArticle> GetNewsArticleByIdAsync(string id)
            => await NewsArticleDAO.Instance.GetNewsArticleByIdAsync(id);
        public async Task UpdateNewsArticleAsync(NewsArticle article, List<int> selectedTagIds)
        {
            var existingArticle = await NewsArticleDAO.Instance.GetNewsArticleWithTagsByIdAsync(article.NewsArticleId);
            if (existingArticle == null)
            {
                throw new KeyNotFoundException("News article not found.");
            }

            existingArticle.NewsTitle = article.NewsTitle;
            existingArticle.Headline = article.Headline;
            existingArticle.NewsContent = article.NewsContent;
            existingArticle.CategoryId = article.CategoryId;
            existingArticle.NewsStatus = article.NewsStatus;
            existingArticle.ModifiedDate = DateTime.Now;

            existingArticle.Tags.Clear();

            var tagsToAdd = await TagDAO.Instance.GetTagsByIdsAsync(selectedTagIds);

            foreach (var tag in tagsToAdd)
            {
                existingArticle.Tags.Add(tag);
            }

            await NewsArticleDAO.Instance.UpdateNewsArticleAsync(existingArticle);
        }
        public async Task<(List<NewsArticle> Articles, int TotalPages)> GetPaginatedArticlesByCreatorIdAsync(short creatorId, int pageNumber, int pageSize)
        {
            var (articles, totalCount) = await NewsArticleDAO.Instance.GetPaginatedNewsArticlesByCreatorIdAsync(creatorId, pageNumber, pageSize);

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return (articles, totalPages);
        }
        public async Task<List<NewsArticle>> GetNewsArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await NewsArticleDAO.Instance.GetNewsArticlesByDateRangeAsync(startDate, endDate);
        }

        public async Task<(List<NewsArticle> Articles, int TotalPages)> GetActiveNewsArticlesAsync(int pageNumber, int pageSize)
        {
            var (articles, totalCount) = await NewsArticleDAO.Instance.GetActiveNewsArticlesWithTagsAndCategoryAsync(pageNumber, pageSize);
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return (articles, totalPages);
        }

    }
}
