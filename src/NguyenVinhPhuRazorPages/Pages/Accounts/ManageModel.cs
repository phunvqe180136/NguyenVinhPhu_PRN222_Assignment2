using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.RazorPages.Pages.Accounts;

[IgnoreAntiforgeryToken]
public class ManageModel : PageModel
{
    private readonly ISystemAccountService _accountService;

    public ManageModel(ISystemAccountService accountService)
    {
        _accountService = accountService;
    }

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("AccountRole");
        if (string.IsNullOrEmpty(role) || role != "0")
        {
            return RedirectToPage("/Auth/Login");
        }
        return Page();
    }

    public IActionResult OnGetById(short id)
    {
        var account = _accountService.GetById(id);
        if (account == null)
        {
            return NotFound();
        }

        return new JsonResult(new
        {
            account.AccountID,
            account.AccountName,
            account.AccountEmail,
            account.AccountRole
        });
    }

    public IActionResult OnPostCreate([FromBody] SystemAccount account)
    {
        try
        {
            var maxId = _accountService.GetAll().Any() 
                ? _accountService.GetAll().Max(a => a.AccountID) 
                : (short)0;
            account.AccountID = (short)(maxId + 1);
            _accountService.Create(account);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }

    public IActionResult OnPutEdit(short id, [FromBody] SystemAccount account)
    {
        try
        {
            var existing = _accountService.GetById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.AccountName = account.AccountName;
            existing.AccountEmail = account.AccountEmail;
            existing.AccountRole = account.AccountRole;
            
            if (!string.IsNullOrEmpty(account.AccountPassword))
            {
                existing.AccountPassword = account.AccountPassword;
            }

            _accountService.Update(existing);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }

    public IActionResult OnDelete(short id)
    {
        try
        {
            _accountService.Delete(id);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }
}
