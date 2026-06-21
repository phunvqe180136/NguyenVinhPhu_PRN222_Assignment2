using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.DataAccess.Models;

[Table("SystemAccount")]
public class SystemAccount
{
    [Key]
    public short AccountID { get; set; }

    [MaxLength(100)]
    public string? AccountName { get; set; }

    [MaxLength(70)]
    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }

    [MaxLength(70)]
    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> CreatedNewsArticles { get; set; } = new List<NewsArticle>();

    public virtual ICollection<NewsArticle> UpdatedNewsArticles { get; set; } = new List<NewsArticle>();
}

public static class AccountRoles
{
    public const int Staff = 1;
    public const int Lecturer = 2;
}
