using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagement.Services.IServices;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.RazorPages.Pages.Categories;

[IgnoreAntiforgeryToken]
public class ManageModel : PageModel
{
    private readonly ICategoryService _categoryService;

    public ManageModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("AccountRole");
        if (string.IsNullOrEmpty(role) || role != "1")
        {
            return RedirectToPage("/Auth/Login");
        }
        return Page();
    }

    public IActionResult OnGetById(short id)
    {
        var category = _categoryService.GetById(id);
        if (category == null)
        {
            return NotFound();
        }

        return new JsonResult(new
        {
            category.CategoryID,
            category.CategoryName,
            category.CategoryDesciption,
            category.IsActive
        });
    }

    public IActionResult OnGetAll()
    {
        var categories = _categoryService.GetAll().Select(c => new
        {
            c.CategoryID,
            c.CategoryName,
            c.CategoryDesciption,
            c.IsActive
        });
        return new JsonResult(categories);
    }

    public IActionResult OnPostCreate([FromBody] Category category)
    {
        try
        {
            var maxId = _categoryService.GetAll().Max(c => c.CategoryID);
            category.CategoryID = (short)(maxId + 1);
            _categoryService.Create(category);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }

    public IActionResult OnPutEdit(short id, [FromBody] Category category)
    {
        try
        {
            var existing = _categoryService.GetById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.CategoryName = category.CategoryName;
            existing.CategoryDesciption = category.CategoryDesciption;
            existing.IsActive = category.IsActive;

            _categoryService.Update(existing);
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
            var canDelete = true;
            foreach (var c in _categoryService.GetAll())
            {
                if (c.CategoryID == id)
                {
                    canDelete = _categoryService.Delete(id);
                    break;
                }
            }

            if (!canDelete)
            {
                return new JsonResult(new { success = false, error = "Cannot delete this category because it is associated with existing news articles." });
            }

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, error = ex.Message });
        }
    }
}
