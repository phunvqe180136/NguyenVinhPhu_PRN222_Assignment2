using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.RazorPages.ViewModels;

namespace FUNewsManagement.RazorPages.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly ISystemAccountService _accountService;

    public LoginModel(ISystemAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public LoginViewModel Login { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        var email = Request.Form["Email"].ToString();
        var password = Request.Form["Password"].ToString();

        System.Diagnostics.Debug.WriteLine($"[DEBUG] Email: {email}, Password: {password}");

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessage = "Email and Password are required.";
            return Page();
        }

        var account = _accountService.Login(email, password);

        System.Diagnostics.Debug.WriteLine($"[DEBUG] Account found: {account != null}");

        if (account != null)
        {
            HttpContext.Session.SetString("AccountID", account.AccountID.ToString());
            HttpContext.Session.SetString("AccountEmail", account.AccountEmail ?? "");
            HttpContext.Session.SetString("AccountName", account.AccountName ?? "");
            HttpContext.Session.SetString("AccountRole", account.AccountRole.ToString());

            System.Diagnostics.Debug.WriteLine("[DEBUG] Session set, redirecting to /Index");
            return new RedirectResult("/Index");
        }

        ErrorMessage = "Invalid email or password.";
        return Page();
    }
}
