using BuiPhuocLocRazorPages.Pages.SignalR;
using BuiPhuocLocRazorPages_Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();

builder.Services.AddScoped<ITagRepository, TagRepository>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);

});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();
app.MapRazorPages();
app.MapHub<NewsHub>("/newsHub");



app.Run();
