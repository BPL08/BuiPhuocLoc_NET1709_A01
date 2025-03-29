using BuiPhuocLocRazorPages_BO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_DAO
{
    public class NewsArticleDAO
    {
        private FunewsManagementContext _context;
        private static NewsArticleDAO instance;

        public static NewsArticleDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NewsArticleDAO();
                }
                return instance;
            }
        }

        public NewsArticleDAO()
        {
            _context = new FunewsManagementContext();
        }

        public async Task<List<NewsArticle>> GetAllNewsArticlesAsync()
        {
            using (var context = new FunewsManagementContext())
            {
                return await context.NewsArticles
                    .AsNoTracking()
                    .Include(n => n.Category)
                    .Include(n => n.Tags)
                    .Include(n => n.CreatedBy)
                    .ToListAsync();
            }
        }

        public async Task<NewsArticle?> GetNewsArticleByIdAsync(string id)
        {
            return await _context.NewsArticles
                .AsNoTracking()
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public async Task CreateNewsArticleAsync(NewsArticle article)
        {
            _context.NewsArticles.Add(article);
            await _context.SaveChangesAsync();

            _context.Entry(article).State = EntityState.Detached;
        }

        public async Task UpdateNewsArticleAsync(NewsArticle article)
        {
            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task<bool> DeleteNewsArticleAsync(string id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var article = await _context.NewsArticles
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);

            if (article == null) return false;

            article.Tags.Clear();
            await _context.SaveChangesAsync();

            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }

        public async Task<NewsArticle?> GetNewsArticleWithTagsByIdAsync(string articleId)
        {
            return await _context.NewsArticles
                .Include(n => n.Tags)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NewsArticleId == articleId);
        }

        public async Task UpdateNewsArticleTagsAsync(string articleId, List<Tag> tags)
        {
            // Fetch the existing article with its tags
            var existingArticle = await _context.NewsArticles
                .Include(a => a.Tags) // Include the tags relationship
                .FirstOrDefaultAsync(a => a.NewsArticleId == articleId);

            if (existingArticle == null)
            {
                throw new KeyNotFoundException("News article not found.");
            }

            // Clear the existing tags
            existingArticle.Tags.Clear();

            // Add the new tags
            if (tags != null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    // Ensure the tag is tracked by EF Core
                    var trackedTag = await _context.Tags.FirstOrDefaultAsync(t => t.TagId == tag.TagId);
                    if (trackedTag != null)
                    {
                        existingArticle.Tags.Add(trackedTag);
                    }
                    else
                    {
                        // If the tag is not in the database, throw an exception or handle it
                        throw new KeyNotFoundException($"Tag with ID {tag.TagId} not found.");
                    }
                }
            }

            // Save the changes
            await _context.SaveChangesAsync();
        }

        public async Task<NewsArticle?> GetNewsArticleWithTagsAsync(string articleId)
        {
            return await _context.NewsArticles
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == articleId);
        }

        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds)
        {
            return await _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToListAsync();
        }

        public async Task UpdateAsync(NewsArticle article)
        {
            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void DetachEntity(object entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public async Task<(List<NewsArticle> Articles, int TotalCount)> GetPaginatedNewsArticlesByCreatorIdAsync(short creatorId, int pageNumber, int pageSize)
        {
            var query = _context.NewsArticles
                .AsNoTracking()
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedById == creatorId);
            int totalCount = await query.CountAsync();

            var articles = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (articles, totalCount);
        }
        public async Task<List<NewsArticle>> GetNewsArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .AsNoTracking()
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<(List<NewsArticle> Articles, int TotalCount)> GetActiveNewsArticlesWithTagsAndCategoryAsync(int pageNumber, int pageSize)
        {
            var query = _context.NewsArticles
                .AsNoTracking()
                .Include(n => n.Category) 
                .Include(n => n.Tags)   
                .Include(n=>n.CreatedBy)
                .Where(n => n.NewsStatus == true); 
            int totalCount = await query.CountAsync();
            var articles = await query
                .OrderByDescending(n => n.CreatedDate) 
                .Skip((pageNumber - 1) * pageSize)   
                .Take(pageSize)                      
                .ToListAsync();
            return (articles, totalCount);
        }
    }
}
