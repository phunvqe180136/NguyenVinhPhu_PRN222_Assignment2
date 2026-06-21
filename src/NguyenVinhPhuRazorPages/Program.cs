using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Data;
using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Repositories;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.Services.Services;
using FUNewsManagement.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FunNewsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();
app.MapHub<Microsoft.AspNetCore.SignalR.Hub>("/newsHub");

app.MapGet("/", () => Results.Redirect("/Index"));

// News API Endpoints
app.MapGet("/api/news", async (INewsArticleService newsService) =>
{
    return Results.Ok(newsService.GetAll());
});

app.MapGet("/api/news/tags", async (ITagService tagService) =>
{
    var tags = tagService.GetAll().Select(t => new { t.TagID, t.TagName, t.Note });
    return Results.Ok(tags);
});

app.MapGet("/api/news/{id}", async (string id, INewsArticleService newsService) =>
{
    var news = newsService.GetByIdWithDetails(id);
    if (news == null) return Results.NotFound();
    
    return Results.Ok(new
    {
        news.NewsArticleID,
        news.Headline,
        news.NewsTitle,
        news.NewsContent,
        news.NewsSource,
        news.CategoryID,
        CategoryName = news.Category?.CategoryName,
        news.NewsStatus,
        news.CreatedByID,
        CreatedByName = news.CreatedBy?.AccountName,
        news.ModifiedDate,
        Tags = news.NewsTags.Select(nt => new { nt.Tag.TagID, nt.Tag.TagName })
    });
});

app.MapPost("/api/news", async (HttpContext context, INewsArticleService newsService) =>
{
    var accountId = short.Parse(context.Session.GetString("AccountID") ?? "0");
    
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var data = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(body);
    
    var news = new NewsArticle
    {
        NewsArticleID = newsService.GenerateNewId(),
        Headline = data.GetProperty("headline").GetString() ?? "",
        NewsTitle = data.GetProperty("newsTitle").GetString(),
        NewsContent = data.GetProperty("newsContent").GetString(),
        NewsSource = data.GetProperty("newsSource").GetString(),
        CategoryID = data.GetProperty("categoryID").GetInt16(),
        NewsStatus = data.GetProperty("newsStatus").GetBoolean(),
        CreatedByID = accountId,
        CreatedDate = DateTime.Now
    };
    
    var tagIds = new List<int>();
    if (data.TryGetProperty("selectedTagIds", out var tagElement))
    {
        foreach (var item in tagElement.EnumerateArray())
        {
            tagIds.Add(item.GetInt32());
        }
    }
    
    newsService.Create(news, tagIds);
    return Results.Ok(new { success = true });
});

app.MapPut("/api/news/{id}", async (string id, HttpContext context, INewsArticleService newsService) =>
{
    var accountId = short.Parse(context.Session.GetString("AccountID") ?? "0");
    
    var existingNews = newsService.GetById(id);
    if (existingNews == null) return Results.NotFound();
    
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var data = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(body);
    
    existingNews.Headline = data.GetProperty("headline").GetString() ?? "";
    existingNews.NewsTitle = data.GetProperty("newsTitle").GetString();
    existingNews.NewsContent = data.GetProperty("newsContent").GetString();
    existingNews.NewsSource = data.GetProperty("newsSource").GetString();
    existingNews.CategoryID = data.GetProperty("categoryID").GetInt16();
    existingNews.NewsStatus = data.GetProperty("newsStatus").GetBoolean();
    existingNews.UpdatedByID = accountId;
    existingNews.ModifiedDate = DateTime.Now;
    
    var tagIds = new List<int>();
    if (data.TryGetProperty("selectedTagIds", out var tagElement))
    {
        foreach (var item in tagElement.EnumerateArray())
        {
            tagIds.Add(item.GetInt32());
        }
    }
    
    newsService.Update(existingNews, tagIds);
    return Results.Ok(new { success = true });
});

app.MapDelete("/api/news/{id}", async (string id, INewsArticleService newsService) =>
{
    try
    {
        newsService.Delete(id);
        return Results.Ok(new { success = true });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, error = ex.Message });
    }
});

// Category API Endpoints
app.MapGet("/api/categories", async (ICategoryService categoryService) =>
{
    var categories = categoryService.GetAll().Select(c => new
    {
        c.CategoryID,
        c.CategoryName,
        c.CategoryDesciption,
        c.IsActive
    });
    return Results.Ok(categories);
});

app.Run();
