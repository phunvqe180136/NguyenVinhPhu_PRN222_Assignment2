using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.Services.IServices;

namespace FUNewsManagement.Services.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public IEnumerable<Category> GetAll()
    {
        return _categoryRepository.GetAll();
    }

    public Category? GetById(short id)
    {
        return _categoryRepository.GetById(id);
    }

    public void Create(Category category)
    {
        _categoryRepository.Create(category);
    }

    public void Update(Category category)
    {
        _categoryRepository.Update(category);
    }

    public bool Delete(short id)
    {
        if (_categoryRepository.CanDelete(id))
        {
            _categoryRepository.Delete(id);
            return true;
        }
        return false;
    }

    public IEnumerable<Category> Search(string? searchTerm)
    {
        return _categoryRepository.Search(searchTerm);
    }
}
