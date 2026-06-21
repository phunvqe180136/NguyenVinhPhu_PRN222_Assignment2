using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Data;
using FUNewsManagement.DataAccess.IRepositories;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories;

public class NewsArticleRepository : INewsArticleRepository
{
    private readonly FunNewsDbContext _context;

    public NewsArticleRepository(FunNewsDbContext context)
    {
        _context = context;
    }

    public IEnumerable<NewsArticle> GetAll()
    {
        return _context.NewsArticles.ToList();
    }

    public IEnumerable<NewsArticle> GetAllWithDetails()
    {
        return _context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags)
            .ThenInclude(nt => nt.Tag)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();
    }

    public NewsArticle? GetById(object id)
    {
        return _context.NewsArticles.Find(id);
    }

    public NewsArticle? GetByIdWithDetails(string id)
    {
        return _context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.UpdatedBy)
            .Include(n => n.NewsTags)
            .ThenInclude(nt => nt.Tag)
            .FirstOrDefault(n => n.NewsArticleID == id);
    }

    public void Create(NewsArticle entity)
    {
        _context.NewsArticles.Add(entity);
        _context.SaveChanges();
    }

    public void CreateWithTags(NewsArticle article, IEnumerable<int> tagIds)
    {
        _context.NewsArticles.Add(article);
        _context.SaveChanges();

        foreach (var tagId in tagIds)
        {
            _context.NewsTags.Add(new NewsTag
            {
                NewsArticleID = article.NewsArticleID,
                TagID = tagId
            });
        }
        _context.SaveChanges();
    }

    public void Update(NewsArticle entity)
    {
        _context.NewsArticles.Update(entity);
        _context.SaveChanges();
    }

    public void UpdateWithTags(NewsArticle article, IEnumerable<int> tagIds)
    {
        var existingTags = _context.NewsTags.Where(nt => nt.NewsArticleID == article.NewsArticleID);
        _context.NewsTags.RemoveRange(existingTags);

        _context.NewsArticles.Update(article);

        foreach (var tagId in tagIds)
        {
            _context.NewsTags.Add(new NewsTag
            {
                NewsArticleID = article.NewsArticleID,
                TagID = tagId
            });
        }
        _context.SaveChanges();
    }

    public void Delete(object id)
    {
        var entity = _context.NewsArticles.Find(id);
        if (entity != null)
        {
            var tags = _context.NewsTags.Where(nt => nt.NewsArticleID == entity.NewsArticleID);
            _context.NewsTags.RemoveRange(tags);
            _context.NewsArticles.Remove(entity);
            _context.SaveChanges();
        }
    }

    public IEnumerable<NewsArticle> Search(string? searchTerm, short? categoryId, bool? status)
    {
        var query = _context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags)
            .ThenInclude(nt => nt.Tag)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(n => n.Headline.Contains(searchTerm) || 
                                    (n.NewsTitle != null && n.NewsTitle.Contains(searchTerm)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(n => n.CategoryID == categoryId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(n => n.NewsStatus == status.Value);
        }

        return query.OrderByDescending(n => n.CreatedDate).ToList();
    }

    public IEnumerable<NewsArticle> GetByCreator(short creatorId)
    {
        return _context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.NewsTags)
            .ThenInclude(nt => nt.Tag)
            .Where(n => n.CreatedByID == creatorId)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();
    }

    public IEnumerable<NewsArticle> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();
    }

    public string GenerateNewId()
    {
        var allIds = _context.NewsArticles.Select(n => n.NewsArticleID).ToList();
        var maxId = 0;
        foreach (var id in allIds)
        {
            if (int.TryParse(id, out var num) && num > maxId)
            {
                maxId = num;
            }
        }
        return (maxId + 1).ToString();
    }
}
