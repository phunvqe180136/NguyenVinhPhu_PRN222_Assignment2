using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;

namespace FUNewsManagement.RazorPages.Pages.News;

public class DetailsModel : PageModel
{
    private readonly INewsArticleService _newsService;

    public DetailsModel(INewsArticleService newsService)
    {
        _newsService = newsService;
    }

    public FUNewsManagement.DataAccess.Models.NewsArticle? NewsArticle { get; set; }

    public IActionResult OnGet(string id)
    {
        var news = _newsService.GetByIdWithDetails(id);
        if (news == null)
        {
            return NotFound();
        }

        var role = HttpContext.Session.GetString("AccountRole");
        
        if (string.IsNullOrEmpty(role) && news.NewsStatus != true)
        {
            return NotFound();
        }
        else if (role == "2" && news.NewsStatus != true)
        {
            return NotFound();
        }

        NewsArticle = news;
        return Page();
    }
}
