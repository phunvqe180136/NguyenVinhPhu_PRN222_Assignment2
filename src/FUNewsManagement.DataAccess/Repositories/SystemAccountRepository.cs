using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Data;
using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories;

public class SystemAccountRepository : ISystemAccountRepository
{
    private readonly FunNewsDbContext _context;

    public SystemAccountRepository(FunNewsDbContext context)
    {
        _context = context;
    }

    public IEnumerable<SystemAccount> GetAll()
    {
        return _context.SystemAccounts.ToList();
    }

    public SystemAccount? GetById(object id)
    {
        return _context.SystemAccounts.Find(id);
    }

    public SystemAccount? GetByEmail(string email)
    {
        return _context.SystemAccounts.FirstOrDefault(a => a.AccountEmail == email);
    }

    public void Create(SystemAccount entity)
    {
        _context.SystemAccounts.Add(entity);
        _context.SaveChanges();
    }

    public void Update(SystemAccount entity)
    {
        _context.SystemAccounts.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(object id)
    {
        var entity = _context.SystemAccounts.Find(id);
        if (entity != null)
        {
            _context.SystemAccounts.Remove(entity);
            _context.SaveChanges();
        }
    }

    public IEnumerable<SystemAccount> Search(string? searchTerm)
    {
        var query = _context.SystemAccounts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(a => a.AccountName.Contains(searchTerm) || 
                                    a.AccountEmail.Contains(searchTerm));
        }

        return query.ToList();
    }

    public IEnumerable<SystemAccount> GetByRole(int role)
    {
        return _context.SystemAccounts.Where(a => a.AccountRole == role).ToList();
    }
}
