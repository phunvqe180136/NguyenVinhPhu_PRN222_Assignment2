using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Data;
using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories;

public class TagRepository : ITagRepository
{
    private readonly FunNewsDbContext _context;

    public TagRepository(FunNewsDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Tag> GetAll()
    {
        return _context.Tags.ToList();
    }

    public IEnumerable<Tag> GetAllActive()
    {
        return _context.Tags.ToList();
    }

    public Tag? GetById(object id)
    {
        return _context.Tags.Find(id);
    }

    public void Create(Tag entity)
    {
        _context.Tags.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Tag entity)
    {
        _context.Tags.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(object id)
    {
        var entity = _context.Tags.Find(id);
        if (entity != null)
        {
            _context.Tags.Remove(entity);
            _context.SaveChanges();
        }
    }
}
