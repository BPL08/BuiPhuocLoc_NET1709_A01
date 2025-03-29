using BuiPhuocLocRazorPages.Pages.SignalR;
using BuiPhuocLocRazorPages_BO;
using BuiPhuocLocRazorPages_DAO;
using BuiPhuocLocRazorPages_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace BuiPhuocLocRazorPages.Pages.Staff.NewsPages
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepo;
        private readonly ITagRepository _tagrepo;
        private readonly IHubContext<NewsHub> _hubContext;

        public DeleteModel(INewsArticleRepository newsRepo, ITagRepository tagrepo,IHubContext<NewsHub> hubContext)
        {
            _newsRepo = newsRepo;
            _tagrepo = tagrepo;
            _hubContext = hubContext;
        }

        public SelectList DeleteArticleDropdown { get; set; }
        public NewsArticle SelectedArticle { get; set; }

        [BindProperty]
        public string SelectedArticleId { get; set; }

        public async Task OnGetAsync()
        {
         
            var articles = await _newsRepo.GetAllNewsArticlesAsync(1, int.MaxValue);
            DeleteArticleDropdown = new SelectList(articles.Articles, "NewsArticleId", "NewsTitle");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(SelectedArticleId))
            {
                SelectedArticle = await _newsRepo.GetNewsArticleByIdAsync(SelectedArticleId);
                if (SelectedArticle != null)
                {
                    SelectedArticle.Tags = await _tagrepo.GetTagsByNewsArticleIdAsync(SelectedArticleId);
                }
            }

            var articles = await _newsRepo.GetAllNewsArticlesAsync(1, int.MaxValue);
            DeleteArticleDropdown = new SelectList(articles.Articles, "NewsArticleId", "NewsTitle");

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!string.IsNullOrEmpty(SelectedArticleId))
            {
                await _newsRepo.DeleteNewsArticleAsync(SelectedArticleId);
                TempData["SuccessMessage"] = "Article deleted successfully.";
                  await _hubContext.Clients.All.SendAsync("LoadNews");
                await OnGetAsync();
                ModelState.Clear();
                return Page();
            }

            var articles = await _newsRepo.GetAllNewsArticlesAsync(1, int.MaxValue);
            DeleteArticleDropdown = new SelectList(articles.Articles, "NewsArticleId", "NewsTitle");
          
            ModelState.AddModelError("", "Failed to delete the article. Please try again.");
            return Page();
        }
    }
}
