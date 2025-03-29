using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        public async Task<(List<Category> Categories, int TotalPages)> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            var allCategories = await CategoryDAO.Instance.GetAllCategoriesAsync();
            int totalPages = (allCategories.Count + pageSize - 1) / pageSize;
            var paginatedCategories = allCategories
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
            return (paginatedCategories, totalPages);
        }

        public async Task<List<Category>> GetCategoriesNoPagiAsync()
            => await CategoryDAO.Instance.GetAllCategoriesAsync();

        public async Task<(List<Category> Categories, int TotalPages)> SearchCategoriesAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var filteredCategories = await CategoryDAO.Instance.SearchCategoriesAsync(searchTerm);
            int totalPages = (filteredCategories.Count + pageSize - 1) / pageSize;
            var paginatedCategories = filteredCategories
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
            return (paginatedCategories, totalPages);
        }

        public async Task<bool> CategoryExistsAsync(short id)
        {
            var category = await CategoryDAO.Instance.GetCategoryByIdAsync(id);
            return category != null;
        }

        public async Task<Category> GetCategoryByIdAsync(short id)
        {
            Category category = await CategoryDAO.Instance.GetCategoryByIdAsync(id);
            return category;
        }

        public Task CreateCategoryAsync(Category category) =>
            CategoryDAO.Instance.CreateCategoryAsync(category);

        public async Task UpdateCategoryAsync(Category category)
        {
            Category existingCategory = await CategoryDAO.Instance.GetCategoryByIdAsync(category.CategoryId);
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryDesciption = category.CategoryDesciption;
            existingCategory.ParentCategoryId = category.ParentCategoryId;
            existingCategory.IsActive = category.IsActive;
            await CategoryDAO.Instance.UpdateCategoryAsync(existingCategory);
        }

        public async Task<bool> DeleteCategoryAsync(short id)
        {
            var hasNewsArticles = await CategoryDAO.Instance.CanDeleteCategoryAsync(id);
            if (!hasNewsArticles)
            {
                await CategoryDAO.Instance.DeleteCategoryAsync(id);
                return true;
            }
            return false;
        }

      
    }

}
