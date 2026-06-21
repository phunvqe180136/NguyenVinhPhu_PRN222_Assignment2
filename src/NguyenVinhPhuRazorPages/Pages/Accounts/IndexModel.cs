using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.RazorPages.Pages.Accounts;

public class IndexModel : PageModel
{
    private readonly ISystemAccountService _accountService;

    public IndexModel(ISystemAccountService accountService)
    {
        _accountService = accountService;
    }

    public IEnumerable<SystemAccount> Accounts { get; set; } = new List<SystemAccount>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public void OnGet()
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            Accounts = _accountService.Search(SearchTerm);
        }
        else
        {
            Accounts = _accountService.GetAll();
        }
    }
}
