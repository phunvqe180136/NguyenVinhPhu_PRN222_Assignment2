using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.Services.IServices;

public interface ICategoryService
{
    IEnumerable<Category> GetAll();
    Category? GetById(short id);
    void Create(Category category);
    void Update(Category category);
    bool Delete(short id);
    IEnumerable<Category> Search(string? searchTerm);
}
