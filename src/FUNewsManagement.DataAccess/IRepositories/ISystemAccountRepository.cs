using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.IRepositories;

public interface ISystemAccountRepository : IRepository<SystemAccount>
{
    SystemAccount? GetByEmail(string email);
    IEnumerable<SystemAccount> Search(string? searchTerm);
    IEnumerable<SystemAccount> GetByRole(int role);
}
