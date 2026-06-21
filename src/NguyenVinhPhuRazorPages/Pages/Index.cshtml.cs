using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;

namespace FUNewsManagement.RazorPages.Pages;

public class IndexModel : PageModel
{
    private readonly INewsArticleService _newsService;

    public IndexModel(INewsArticleService newsService)
    {
        _newsService = newsService;
    }

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("AccountRole");
        var accountId = HttpContext.Session.GetString("AccountID");
        
        System.Diagnostics.Debug.WriteLine($"[INDEX] Role: {role}, AccountID: {accountId}");
        
        if (string.IsNullOrEmpty(role))
        {
            System.Diagnostics.Debug.WriteLine("[INDEX] Redirecting to login - role is empty");
            return RedirectToPage("/Auth/Login");
        }
        
        System.Diagnostics.Debug.WriteLine("[INDEX] Session OK, showing page");
        return Page();
    }
}
