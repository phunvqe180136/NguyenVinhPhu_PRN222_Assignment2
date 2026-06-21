using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.IRepositories;

public interface ITagRepository : IRepository<Tag>
{
    IEnumerable<Tag> GetAllActive();
}
