namespace FUNewsManagement.RazorPages.ViewModels;

public class NewsArticleViewModel
{
    public string NewsArticleID { get; set; } = string.Empty;
    public string? NewsTitle { get; set; }
    public string Headline { get; set; } = string.Empty;
    public DateTime? CreatedDate { get; set; }
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public short? CategoryID { get; set; }
    public string? CategoryName { get; set; }
    public bool? NewsStatus { get; set; }
    public short? CreatedByID { get; set; }
    public string? CreatedByName { get; set; }
    public short? UpdatedByID { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<int> SelectedTagIds { get; set; } = new();
    public List<TagViewModel> Tags { get; set; } = new();
}

public class TagViewModel
{
    public int TagID { get; set; }
    public string? TagName { get; set; }
    public string? Note { get; set; }
    public bool IsSelected { get; set; }
}
