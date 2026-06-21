using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.RazorPages.Pages.Profile;

public class IndexModel : PageModel
{
    private readonly ISystemAccountService _accountService;

    public IndexModel(ISystemAccountService accountService)
    {
        _accountService = accountService;
    }

    public SystemAccount? Account { get; set; }

    [BindProperty]
    public string AccountName { get; set; } = string.Empty;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string? NewPassword { get; set; }

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        var accountId = HttpContext.Session.GetString("AccountID");
        var role = HttpContext.Session.GetString("AccountRole");

        if (string.IsNullOrEmpty(accountId))
        {
            RedirectToPage("/Auth/Login");
            return;
        }

        if (role == "0")
        {
            Account = new SystemAccount
            {
                AccountID = 0,
                AccountName = HttpContext.Session.GetString("AccountName") ?? "Administrator",
                AccountEmail = HttpContext.Session.GetString("AccountEmail") ?? ""
            };
        }
        else
        {
            Account = _accountService.GetById(short.Parse(accountId));
        }

        if (Account != null)
        {
            AccountName = Account.AccountName ?? "";
            Email = Account.AccountEmail ?? "";
        }
    }

    public IActionResult OnPost()
    {
        var accountId = HttpContext.Session.GetString("AccountID");
        var role = HttpContext.Session.GetString("AccountRole");

        if (string.IsNullOrEmpty(accountId))
        {
            return RedirectToPage("/Auth/Login");
        }

        try
        {
            if (role == "0")
            {
                HttpContext.Session.SetString("AccountName", AccountName);
                SuccessMessage = "Profile updated successfully!";
            }
            else
            {
                var account = _accountService.GetById(short.Parse(accountId));
                if (account != null)
                {
                    account.AccountName = AccountName;
                    account.AccountEmail = Email;
                    
                    if (!string.IsNullOrEmpty(NewPassword))
                    {
                        account.AccountPassword = NewPassword;
                    }

                    _accountService.Update(account);
                    HttpContext.Session.SetString("AccountName", AccountName);
                    HttpContext.Session.SetString("AccountEmail", Email);
                    SuccessMessage = "Profile updated successfully!";
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error updating profile: " + ex.Message;
        }

        return Page();
    }
}
