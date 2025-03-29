using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;

namespace BuiPhuocLocRazorPages.Pages.Staff.CategoryPages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepo;
        public IndexModel(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public List<Category> Categories { get; set; } = new();
        [BindProperty(SupportsGet = true)] public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public int pageSize { get; set; } = 4;

        public async Task OnGetAsync(string searchTerm, int? pageNumber)
        {
         

            SearchTerm = searchTerm;
            PageNumber = pageNumber ?? 1;

            var (categories, totalPages) = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _categoryRepo.GetAllCategoriesAsync(PageNumber, pageSize)
                : await _categoryRepo.SearchCategoriesAsync(SearchTerm, PageNumber, pageSize);

            Categories = categories;
            TotalPages = totalPages;
        }

        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            var result = await _categoryRepo.DeleteCategoryAsync(id);

            if (result)
            {
                SuccessMessage = "Category deleted successfully.";
            }
            else
            {
                ErrorMessage = "Cannot delete category because it is associated with news articles.";
            }

            return RedirectToPage("./Index", new { SearchTerm, PageNumber });
        }
    }
}
