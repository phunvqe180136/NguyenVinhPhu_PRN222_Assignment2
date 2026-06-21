using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.Services.IServices;

public interface ISystemAccountService
{
    IEnumerable<SystemAccount> GetAll();
    SystemAccount? GetById(short id);
    SystemAccount? GetByEmail(string email);
    SystemAccount? Login(string email, string password, string? adminPassword = null);
    void Create(SystemAccount account);
    void Update(SystemAccount account);
    void Delete(short id);
    IEnumerable<SystemAccount> Search(string? searchTerm);
    IEnumerable<SystemAccount> GetByRole(int role);
}
