using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.Services.IServices;

namespace FUNewsManagement.Services.Services;

public class NewsArticleService : INewsArticleService
{
    private readonly INewsArticleRepository _newsArticleRepository;

    public NewsArticleService(INewsArticleRepository newsArticleRepository)
    {
        _newsArticleRepository = newsArticleRepository;
    }

    public IEnumerable<NewsArticle> GetAll()
    {
        return _newsArticleRepository.GetAll();
    }

    public IEnumerable<NewsArticle> GetAllWithDetails()
    {
        return _newsArticleRepository.GetAllWithDetails();
    }

    public NewsArticle? GetById(string id)
    {
        return _newsArticleRepository.GetById(id);
    }

    public NewsArticle? GetByIdWithDetails(string id)
    {
        return _newsArticleRepository.GetByIdWithDetails(id);
    }

    public void Create(NewsArticle article, IEnumerable<int> tagIds)
    {
        _newsArticleRepository.CreateWithTags(article, tagIds);
    }

    public void Update(NewsArticle article, IEnumerable<int> tagIds)
    {
        _newsArticleRepository.UpdateWithTags(article, tagIds);
    }

    public void Delete(string id)
    {
        _newsArticleRepository.Delete(id);
    }

    public IEnumerable<NewsArticle> Search(string? searchTerm, short? categoryId, bool? status)
    {
        return _newsArticleRepository.Search(searchTerm, categoryId, status);
    }

    public IEnumerable<NewsArticle> GetByCreator(short creatorId)
    {
        return _newsArticleRepository.GetByCreator(creatorId);
    }

    public IEnumerable<NewsArticle> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return _newsArticleRepository.GetByDateRange(startDate, endDate);
    }

    public IEnumerable<NewsArticle> GetActiveNews()
    {
        return _newsArticleRepository.Search(null, null, true);
    }

    public string GenerateNewId()
    {
        return _newsArticleRepository.GenerateNewId();
    }
}
