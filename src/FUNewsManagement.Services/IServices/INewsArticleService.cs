using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.Services.IServices;

public interface INewsArticleService
{
    IEnumerable<NewsArticle> GetAll();
    IEnumerable<NewsArticle> GetAllWithDetails();
    NewsArticle? GetById(string id);
    NewsArticle? GetByIdWithDetails(string id);
    void Create(NewsArticle article, IEnumerable<int> tagIds);
    void Update(NewsArticle article, IEnumerable<int> tagIds);
    void Delete(string id);
    IEnumerable<NewsArticle> Search(string? searchTerm, short? categoryId, bool? status);
    IEnumerable<NewsArticle> GetByCreator(short creatorId);
    IEnumerable<NewsArticle> GetByDateRange(DateTime startDate, DateTime endDate);
    IEnumerable<NewsArticle> GetActiveNews();
    string GenerateNewId();
}
