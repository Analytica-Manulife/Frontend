namespace Frontend.Model;

public class NewsArticle
{
    public int NewsId { get; set; }
    public string Headline { get; set; }
    public string Content { get; set; }
    public string ArticleLink { get; set; }
    public string? ImageLink { get; set; }
    public string? Company { get; set; }
}