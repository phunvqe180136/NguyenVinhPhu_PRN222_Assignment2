using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.RazorPages.Pages.Statistics;

public class IndexModel : PageModel
{
    private readonly INewsArticleService _newsService;

    public IndexModel(INewsArticleService newsService)
    {
        _newsService = newsService;
    }

    public IEnumerable<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

    [BindProperty(SupportsGet = true)]
    public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);

    [BindProperty(SupportsGet = true)]
    public DateTime EndDate { get; set; } = DateTime.Now;

    public int TotalArticles { get; set; }
    public int ActiveArticles { get; set; }
    public int InactiveArticles { get; set; }

    public void OnGet()
    {
        var role = HttpContext.Session.GetString("AccountRole");
        if (string.IsNullOrEmpty(role) || role != "0")
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        NewsArticles = _newsService.GetByDateRange(StartDate, EndDate);
        TotalArticles = NewsArticles.Count();
        ActiveArticles = NewsArticles.Count(n => n.NewsStatus == true);
        InactiveArticles = NewsArticles.Count(n => n.NewsStatus == false);
    }
}
