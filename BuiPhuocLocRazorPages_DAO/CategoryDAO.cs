using BuiPhuocLocRazorPages_BO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_DAO
{
    public class CategoryDAO
    {
        private FunewsManagementContext _context;
        private static CategoryDAO instance;
        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDAO();
                }
                return instance;
            }
        }
        public CategoryDAO()
        {
            _context = new FunewsManagementContext();
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(short id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CanDeleteCategoryAsync(short categoryId)
        {
            if(!await _context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId))
            {  return false; }
            return true;
        }

        public async Task DeleteCategoryAsync(short id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            
        }

        public async Task<List<Category>> SearchCategoriesAsync(string searchTerm)
        {
            return await _context.Categories
                .Where(c => c.CategoryName.Contains(searchTerm) || c.CategoryDesciption.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
