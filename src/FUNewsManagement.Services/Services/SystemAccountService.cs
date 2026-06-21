using Microsoft.Extensions.Configuration;
using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.Services.IServices;

namespace FUNewsManagement.Services.Services;

public class SystemAccountService : ISystemAccountService
{
    private readonly ISystemAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public SystemAccountService(ISystemAccountRepository accountRepository, IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _configuration = configuration;
    }

    public IEnumerable<SystemAccount> GetAll()
    {
        return _accountRepository.GetAll();
    }

    public SystemAccount? GetById(short id)
    {
        return _accountRepository.GetById(id);
    }

    public SystemAccount? GetByEmail(string email)
    {
        return _accountRepository.GetByEmail(email);
    }

    public SystemAccount? Login(string email, string password, string? adminPassword = null)
    {
        var adminEmail = _configuration["AdminAccount:Email"];
        var adminPwd = _configuration["AdminAccount:Password"];

        if (email == adminEmail && password == adminPwd)
        {
            return new SystemAccount
            {
                AccountID = 0,
                AccountEmail = adminEmail,
                AccountName = "Administrator",
                AccountRole = 0
            };
        }

        var account = _accountRepository.GetByEmail(email);
        if (account != null && account.AccountPassword == password)
        {
            return account;
        }

        return null;
    }

    public void Create(SystemAccount account)
    {
        _accountRepository.Create(account);
    }

    public void Update(SystemAccount account)
    {
        _accountRepository.Update(account);
    }

    public void Delete(short id)
    {
        _accountRepository.Delete(id);
    }

    public IEnumerable<SystemAccount> Search(string? searchTerm)
    {
        return _accountRepository.Search(searchTerm);
    }

    public IEnumerable<SystemAccount> GetByRole(int role)
    {
        return _accountRepository.GetByRole(role);
    }
}
