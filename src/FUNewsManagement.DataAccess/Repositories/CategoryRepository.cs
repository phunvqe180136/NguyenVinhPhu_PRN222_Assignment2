using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Data;
using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly FunNewsDbContext _context;

    public CategoryRepository(FunNewsDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Category> GetAll()
    {
        return _context.Categories.ToList();
    }

    public Category? GetById(object id)
    {
        return _context.Categories.Find(id);
    }

    public void Create(Category entity)
    {
        _context.Categories.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Category entity)
    {
        _context.Categories.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(object id)
    {
        var entity = _context.Categories.Find(id);
        if (entity != null)
        {
            _context.Categories.Remove(entity);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Category> Search(string? searchTerm)
    {
        var query = _context.Categories.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.CategoryName.Contains(searchTerm) || 
                                    c.CategoryDesciption.Contains(searchTerm));
        }
        
        return query.ToList();
    }

    public bool CanDelete(short id)
    {
        return !_context.NewsArticles.Any(n => n.CategoryID == id);
    }
}
