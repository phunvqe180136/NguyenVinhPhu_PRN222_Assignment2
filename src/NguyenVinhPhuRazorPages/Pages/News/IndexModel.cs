using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.RazorPages.Pages.News;

public class IndexModel : PageModel
{
    private readonly INewsArticleService _newsService;
    private readonly ICategoryService _categoryService;

    public IndexModel(INewsArticleService newsService, ICategoryService categoryService)
    {
        _newsService = newsService;
        _categoryService = categoryService;
    }

    public IEnumerable<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public short? CategoryId { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool? Status { get; set; }

    public void OnGet()
    {
        var role = HttpContext.Session.GetString("AccountRole");
        
        if (string.IsNullOrEmpty(role))
        {
            NewsArticles = _newsService.Search(SearchTerm, CategoryId, true);
        }
        else if (int.Parse(role) == 2)
        {
            NewsArticles = _newsService.Search(SearchTerm, CategoryId, true);
        }
        else
        {
            NewsArticles = _newsService.Search(SearchTerm, CategoryId, Status);
        }

        Categories = _categoryService.GetAll();
    }
}
