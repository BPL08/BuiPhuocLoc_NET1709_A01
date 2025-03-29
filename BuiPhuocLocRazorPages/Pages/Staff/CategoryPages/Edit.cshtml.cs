using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;

namespace BuiPhuocLocRazorPages.Pages.Staff.CategoryPages
{
    public class EditModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepo;

        public EditModel(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [BindProperty]
        public Category Category { get; set; }

        public SelectList ParentCategories { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var getid=(short)id;
            Category = await _categoryRepo.GetCategoryByIdAsync(getid);
            if (Category == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepo.GetCategoriesNoPagiAsync();
            ParentCategories = new SelectList(categories, "CategoryId", "CategoryName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepo.GetCategoriesNoPagiAsync();
                ParentCategories = new SelectList(categories, "CategoryId", "CategoryName");
                return Page();
            }

            await _categoryRepo.UpdateCategoryAsync(Category);
            return RedirectToPage("./Index");
        }
    }
}
