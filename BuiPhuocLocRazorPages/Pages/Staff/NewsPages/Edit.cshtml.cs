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
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using BuiPhuocLocRazorPages.Pages.SignalR;

namespace BuiPhuocLocRazorPages.Pages.Staff.NewsPages
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ITagRepository _tagRepo;
        private readonly IHubContext<NewsHub> _hubContext;

        public EditModel(
            INewsArticleRepository newsRepo,
            ICategoryRepository categoryRepo,
            ITagRepository tagRepo,
            IHubContext<NewsHub> hubContext)
        {
            _newsRepo = newsRepo;
            _categoryRepo = categoryRepo;
            _tagRepo = tagRepo;
            _hubContext = hubContext;
        }

        public SelectList ArticleDropdown { get; set; }
        public List<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TagList { get; set; } = new List<SelectListItem>();

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = new NewsArticle();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        [BindProperty]
        public string SelectedArticleId { get; set; }

        public async Task OnGetAsync()
        {
            // Populate the dropdown with all articles
            var articles = await _newsRepo.GetAllNewsArticlesAsync(1, int.MaxValue);
            ArticleDropdown = new SelectList(articles.Articles, "NewsArticleId", "NewsTitle");

            // Populate categories and tags
            await LoadDropdownsAndTags();
        }

        public async Task<IActionResult> OnPostLoadAsync()
        {
            if (!string.IsNullOrEmpty(SelectedArticleId))
            {
                // Load the selected article
                NewsArticle = await _newsRepo.GetNewsArticleByIdAsync(SelectedArticleId);
                if (NewsArticle != null)
                {
                    SelectedTagIds = NewsArticle.Tags?.Select(t => t.TagId).ToList() ?? new List<int>();
                }
            }
            var articles = await _newsRepo.GetAllNewsArticlesAsync(1, int.MaxValue);
            ArticleDropdown = new SelectList(articles.Articles, "NewsArticleId", "NewsTitle");
            await LoadDropdownsAndTags();

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            try
            {
                NewsArticle.ModifiedDate = DateTime.Now;
                await _newsRepo.UpdateNewsAsync(NewsArticle, SelectedTagIds);
                TempData["SuccessMessage"] = "Article updated successfully.";
                NewsArticle = new NewsArticle();
                SelectedTagIds.Clear();
                SelectedArticleId = null;
                var articles = await _newsRepo.GetAllNewsArticlesAsync(1, int.MaxValue);
                ArticleDropdown = new SelectList(articles.Articles, "NewsArticleId", "NewsTitle");
                await LoadDropdownsAndTags();
                await _hubContext.Clients.All.SendAsync("LoadNews");
                return Page(); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating article: {ex.Message}");
                await LoadDropdownsAndTags();
                return Page();
            }
        }

        private async Task LoadDropdownsAndTags()
        {
            // Load categories
            CategoryList = (await _categoryRepo.GetCategoriesNoPagiAsync() ?? new List<Category>())
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName ?? "Unnamed Category"
                }).ToList();

            // Load tags
            TagList = (await _tagRepo.GetAllTagsAsync() ?? new List<Tag>())
                .Select(t => new SelectListItem
                {
                    Value = t.TagId.ToString(),
                    Text = t.TagName ?? "Unnamed Tag"
                }).ToList();
        }
    }
}
