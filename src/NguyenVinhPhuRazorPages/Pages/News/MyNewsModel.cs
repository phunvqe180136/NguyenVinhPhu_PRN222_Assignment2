using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.RazorPages.Pages.News;

public class MyNewsModel : PageModel
{
    private readonly INewsArticleService _newsService;

    public MyNewsModel(INewsArticleService newsService)
    {
        _newsService = newsService;
    }

    public IEnumerable<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("AccountRole");
        if (string.IsNullOrEmpty(role) || role != "1")
        {
            return RedirectToPage("/Auth/Login");
        }

        var accountId = short.Parse(HttpContext.Session.GetString("AccountID") ?? "0");
        NewsArticles = _newsService.GetByCreator(accountId);

        return Page();
    }
}
