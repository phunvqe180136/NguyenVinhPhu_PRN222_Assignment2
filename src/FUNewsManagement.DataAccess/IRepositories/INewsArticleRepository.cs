using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.IRepositories;

public interface INewsArticleRepository : IRepository<NewsArticle>
{
    IEnumerable<NewsArticle> GetAllWithDetails();
    NewsArticle? GetByIdWithDetails(string id);
    IEnumerable<NewsArticle> Search(string? searchTerm, short? categoryId, bool? status);
    IEnumerable<NewsArticle> GetByCreator(short creatorId);
    void CreateWithTags(NewsArticle article, IEnumerable<int> tagIds);
    void UpdateWithTags(NewsArticle article, IEnumerable<int> tagIds);
    IEnumerable<NewsArticle> GetByDateRange(DateTime startDate, DateTime endDate);
    string GenerateNewId();
}
