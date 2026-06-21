using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.DataAccess.Models;

[Table("NewsTag")]
public class NewsTag
{
    [MaxLength(20)]
    public string NewsArticleID { get; set; } = string.Empty;

    public int TagID { get; set; }

    [ForeignKey("NewsArticleID")]
    public virtual NewsArticle? NewsArticle { get; set; }

    [ForeignKey("TagID")]
    public virtual Tag? Tag { get; set; }
}
