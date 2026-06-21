using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.DataAccess.Models;

[Table("Tag")]
public class Tag
{
    [Key]
    public int TagID { get; set; }

    [MaxLength(50)]
    public string? TagName { get; set; }

    [MaxLength(400)]
    public string? Note { get; set; }

    public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
}
