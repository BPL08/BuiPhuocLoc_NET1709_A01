using BuiPhuocLocRazorPages_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiPhuocLocRazorPages_Repository
{
    public interface ICategoryRepository
    {
        Task<bool> DeleteCategoryAsync(short id);

        Task<bool> CategoryExistsAsync(short id);

        Task<Category> GetCategoryByIdAsync(short id);

        Task<List<Category>> GetCategoriesNoPagiAsync();

        Task<(List<Category> Categories, int TotalPages)> GetAllCategoriesAsync(int pageNumber, int pageSize);

        Task CreateCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category);

        Task<(List<Category> Categories, int TotalPages)> SearchCategoriesAsync(string searchTerm, int pageNumber, int pageSize);
    }
}
