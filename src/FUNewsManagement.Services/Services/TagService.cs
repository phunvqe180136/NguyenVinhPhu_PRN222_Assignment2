using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.Services.IServices;

namespace FUNewsManagement.Services.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public IEnumerable<Tag> GetAll()
    {
        return _tagRepository.GetAll();
    }

    public Tag? GetById(int id)
    {
        return _tagRepository.GetById(id);
    }

    public void Create(Tag tag)
    {
        _tagRepository.Create(tag);
    }

    public void Update(Tag tag)
    {
        _tagRepository.Update(tag);
    }

    public void Delete(int id)
    {
        _tagRepository.Delete(id);
    }
}
