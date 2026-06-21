using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.Services.IServices;

public interface ITagService
{
    IEnumerable<Tag> GetAll();
    Tag? GetById(int id);
    void Create(Tag tag);
    void Update(Tag tag);
    void Delete(int id);
}
