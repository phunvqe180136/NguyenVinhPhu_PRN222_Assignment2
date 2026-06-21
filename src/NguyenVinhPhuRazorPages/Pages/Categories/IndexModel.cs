using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.RazorPages.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ICategoryService _categoryService;

    public IndexModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public void OnGet()
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            Categories = _categoryService.Search(SearchTerm);
        }
        else
        {
            Categories = _categoryService.GetAll();
        }
    }
}
