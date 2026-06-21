using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Data;

public class FunNewsDbContext : DbContext
{
    public FunNewsDbContext(DbContextOptions<FunNewsDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<NewsArticle> NewsArticles { get; set; }
    public DbSet<NewsTag> NewsTags { get; set; }
    public DbSet<SystemAccount> SystemAccounts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NewsTag>()
            .HasKey(nt => new { nt.NewsArticleID, nt.TagID });

        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.InverseParentCategory)
            .HasForeignKey(c => c.ParentCategoryID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NewsArticle>()
            .HasOne(n => n.Category)
            .WithMany(c => c.NewsArticles)
            .HasForeignKey(n => n.CategoryID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NewsArticle>()
            .HasOne(n => n.CreatedBy)
            .WithMany(a => a.CreatedNewsArticles)
            .HasForeignKey(n => n.CreatedByID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NewsArticle>()
            .HasOne(n => n.UpdatedBy)
            .WithMany(a => a.UpdatedNewsArticles)
            .HasForeignKey(n => n.UpdatedByID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NewsTag>()
            .HasOne(nt => nt.NewsArticle)
            .WithMany(n => n.NewsTags)
            .HasForeignKey(nt => nt.NewsArticleID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NewsTag>()
            .HasOne(nt => nt.Tag)
            .WithMany(t => t.NewsTags)
            .HasForeignKey(nt => nt.TagID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>().Property(c => c.CategoryID).ValueGeneratedOnAdd();
        modelBuilder.Entity<SystemAccount>().Property(a => a.AccountID).ValueGeneratedOnAdd();
        modelBuilder.Entity<Tag>().Property(t => t.TagID).ValueGeneratedOnAdd();
    }
}
