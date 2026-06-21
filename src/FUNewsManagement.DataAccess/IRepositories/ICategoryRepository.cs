using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.IRepositories;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> Search(string? searchTerm);
    bool CanDelete(short id);
}
