using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.DataAccess.Models;

[Table("Category")]
public class Category
{
    [Key]
    public short CategoryID { get; set; }

    [Required]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [MaxLength(250)]
    public string CategoryDesciption { get; set; } = string.Empty;

    public short? ParentCategoryID { get; set; }

    public bool? IsActive { get; set; }

    [ForeignKey("ParentCategoryID")]
    public virtual Category? ParentCategory { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
