using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;

namespace BuiPhuocLocRazorPages.Pages.Staff.CategoryPages
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepo;
        public CreateModel(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [BindProperty]
        public Category Category { get; set; }

        public SelectList ParentCategories { get; set; }

        public async Task OnGetAsync()
        {
            
            var categories = await _categoryRepo.GetCategoriesNoPagiAsync();
            ParentCategories = new SelectList(categories, "CategoryId", "CategoryName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepo.GetAllCategoriesAsync(1, int.MaxValue);
                ParentCategories = new SelectList(categories.Item1, "CategoryId", "CategoryName");
                return Page();
            }

            await _categoryRepo.CreateCategoryAsync(Category);
            return RedirectToPage("./Index");
        }
    }
}
