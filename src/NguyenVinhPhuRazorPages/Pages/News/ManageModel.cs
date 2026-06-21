using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;
using Microsoft.AspNetCore.SignalR;
using FUNewsManagement.RazorPages.Hubs;

namespace FUNewsManagement.RazorPages.Pages.News;

[IgnoreAntiforgeryToken]
public class ManageModel : PageModel
{
    private readonly INewsArticleService _newsService;
    private readonly ITagService _tagService;
    private readonly IHubContext<NewsHub> _hubContext;

    public ManageModel(INewsArticleService newsService, ITagService tagService, IHubContext<NewsHub> hubContext)
    {
        _newsService = newsService;
        _tagService = tagService;
        _hubContext = hubContext;
    }

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("AccountRole");
        if (string.IsNullOrEmpty(role) || (role != "0" && role != "1"))
        {
            return RedirectToPage("/Auth/Login");
        }
        return Page();
    }

    public IActionResult OnGetTags()
    {
        var tags = _tagService.GetAll().Select(t => new { t.TagID, t.TagName, t.Note });
        return new JsonResult(tags);
    }

    public IActionResult OnGetById(string id)
    {
        var news = _newsService.GetByIdWithDetails(id);
        if (news == null)
        {
            return NotFound();
        }

        var categoryName = news.Category?.CategoryName;
        var createdByName = news.CreatedBy?.AccountName;
        return new JsonResult(new
        {
            news.NewsArticleID,
            news.Headline,
            news.NewsTitle,
            news.NewsContent,
            news.NewsSource,
            news.CategoryID,
            CategoryName = categoryName,
            news.NewsStatus,
            news.CreatedByID,
            CreatedByName = createdByName,
            news.ModifiedDate,
            Tags = news.NewsTags.Select(nt => new { nt.Tag.TagID, nt.Tag.TagName })
        });
    }

    public async Task<IActionResult> OnPostCreate([FromBody] NewsCreateModel model)
    {
        try
        {
            var accountId = short.Parse(HttpContext.Session.GetString("AccountID") ?? "0");
            
            var news = new NewsArticle
            {
                NewsArticleID = _newsService.GenerateNewId(),
                Headline = model.Headline,
                NewsTitle = model.NewsTitle,
                NewsContent = model.NewsContent,
                NewsSource = model.NewsSource,
                CategoryID = model.CategoryID,
                NewsStatus = model.NewsStatus,
                CreatedByID = accountId,
                CreatedDate = DateTime.Now
            };

            _newsService.Create(news, model.SelectedTagIds);

            await _hubContext.Clients.Group("NewsUpdates").SendAsync("ReceiveNewsUpdate", 
                "News article created", news.NewsArticleID, "Create");

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }

    public async Task<IActionResult> OnPutEdit(string id, [FromBody] NewsCreateModel model)
    {
        try
        {
            var accountId = short.Parse(HttpContext.Session.GetString("AccountID") ?? "0");
            
            var existingNews = _newsService.GetById(id);
            if (existingNews == null)
            {
                return NotFound();
            }

            existingNews.Headline = model.Headline;
            existingNews.NewsTitle = model.NewsTitle;
            existingNews.NewsContent = model.NewsContent;
            existingNews.NewsSource = model.NewsSource;
            existingNews.CategoryID = model.CategoryID;
            existingNews.NewsStatus = model.NewsStatus;
            existingNews.UpdatedByID = accountId;
            existingNews.ModifiedDate = DateTime.Now;

            _newsService.Update(existingNews, model.SelectedTagIds);

            await _hubContext.Clients.Group("NewsUpdates").SendAsync("ReceiveNewsUpdate", 
                "News article updated", existingNews.NewsArticleID, "Update");

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }

    public async Task<IActionResult> OnDelete(string id)
    {
        try
        {
            _newsService.Delete(id);
            
            await _hubContext.Clients.Group("NewsUpdates").SendAsync("ReceiveNewsUpdate", 
                "News article deleted", id, "Delete");

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }
}

public class NewsCreateModel
{
    public string Headline { get; set; } = string.Empty;
    public string? NewsTitle { get; set; }
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public short CategoryID { get; set; }
    public bool NewsStatus { get; set; }
    public List<int> SelectedTagIds { get; set; } = new();
}
