using BuiPhuocLocRazorPages_BO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_DAO
{
    public class TagDAO
    {
        private FunewsManagementContext _context;
        private static TagDAO instance;

        public static TagDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TagDAO();
                }
                return instance;
            }
        }

        public TagDAO()
        {
            _context = new FunewsManagementContext();
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            using (var context = new FunewsManagementContext())
            {
                return await context.Tags.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Tag?> GetTagByNameAsync(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.TagName == name);
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _context.Tags.AsNoTracking().SingleOrDefaultAsync(t => t.TagId == id);
        }
        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds)
        {
            return await _context.Tags
                                 .Where(tag => tagIds.Contains(tag.TagId))
                                 .AsNoTracking()
                                 .ToListAsync();
        }


        public async Task CreateTagAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Tag>> GetTagsByNewsArticleIdAsync(string newsArticleId)
        {
            return await _context.Tags
                .Where(tag => tag.NewsArticles.Any(article => article.NewsArticleId == newsArticleId))
                .ToListAsync();
        }

    }
}
