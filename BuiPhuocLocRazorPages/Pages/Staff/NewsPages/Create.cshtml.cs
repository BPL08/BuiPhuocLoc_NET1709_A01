using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using BuiPhuocLocRazorPages.Pages.SignalR;

namespace BuiPhuocLocRazorPages.Pages.Staff.NewsPages
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepo;
        private readonly ITagRepository _tagRepo;
        private readonly ICategoryRepository _categoryRepo;
                private readonly IHubContext<NewsHub> _hubContext;

        [BindProperty] public NewsArticle NewsArticle { get; set; }
        [BindProperty] public short SelectedCategoryId { get; set; }
        [BindProperty] public List<int> SelectedTagIds { get; set; } = new();

        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> TagList { get; set; }

        public CreateModel(INewsArticleRepository newsRepo, ITagRepository tagRepo, ICategoryRepository categoryRepo,IHubContext<NewsHub> hubContext)
        {
            _newsRepo = newsRepo;
            _tagRepo = tagRepo;
            _categoryRepo = categoryRepo;
            _hubContext = hubContext;
        }

        public async Task OnGetAsync()
        {
            CategoryList = (await _categoryRepo.GetCategoriesNoPagiAsync())
                            .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
                            .ToList();
            TagList = (await _tagRepo.GetAllTagsAsync())
                         .Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName })
                         .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }
            if (await _newsRepo.IsNewsArticleExistsAsync(NewsArticle.NewsArticleId))
            {
                ModelState.AddModelError("NewsArticle.NewsArticleId", "This News Article ID already exists.");
                await OnGetAsync();
                return Page();
            }
            short? id = (short)HttpContext.Session.GetInt32("AccountId");
            var newArticle = new NewsArticle
            {
                NewsArticleId = NewsArticle.NewsArticleId,
                NewsTitle = NewsArticle.NewsTitle,
                Headline = NewsArticle.Headline,
                NewsContent = NewsArticle.NewsContent,
                CategoryId = SelectedCategoryId,
                NewsStatus = NewsArticle.NewsStatus,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                NewsSource = NewsArticle.NewsSource,
                CreatedById = id,
                UpdatedById = id
            };

            var selectedTags = await _tagRepo.GetTagsByIdsAsync(SelectedTagIds);

            await _newsRepo.CreateNewsArticleAsync(newArticle, selectedTags);
            await _hubContext.Clients.All.SendAsync("LoadNews");
            TempData["SuccessMessage"] = "News article created successfully.";
            await OnGetAsync(); 
            ModelState.Clear(); 
            return Page();
        }





    }
}
