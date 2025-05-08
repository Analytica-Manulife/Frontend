using Frontend.Model;
using Frontend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class NewsModel : PageModel
{
    private readonly NewsService _newsService;
    private readonly ILogger<NewsModel> _logger;

    public NewsModel(NewsService newsService, ILogger<NewsModel> logger)
    {
        _newsService = newsService;
        _logger = logger;
    }
    
    public List<NewsArticle> Articles { get; set; } = new();
    public List<NewsArticle> AllArticles { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all news articles");
            AllArticles = await _newsService.GetAllAsync();
            
            if (!string.IsNullOrWhiteSpace(Search))
            {
                _logger.LogInformation("Filtering news articles with query: {Query}", Search);
                // Filter articles matching the search query
                var filteredArticles = _newsService.FilterArticles(AllArticles, Search);
                
                if (filteredArticles != null && filteredArticles.Count > 0)
                {
                    // Set the featured article to be the first search result
                    Articles = new List<NewsArticle>();
                    Articles.Add(filteredArticles[0]);
                    
                    // Add all articles to display in the latest news section
                    // First add remaining filtered articles (if any)
                    for (int i = 1; i < filteredArticles.Count; i++)
                    {
                        Articles.Add(filteredArticles[i]);
                    }
                    
                    // Then add all non-matching articles
                    foreach (var article in AllArticles)
                    {
                        if (!filteredArticles.Contains(article))
                        {
                            Articles.Add(article);
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("No articles found matching search criteria");
                    Articles = AllArticles;
                }
            }
            else
            {
                Articles = AllArticles;
            }

            if (Articles == null || Articles.Count == 0)
            {
                _logger.LogWarning("No articles found");
                Articles = new List<NewsArticle>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching news articles");
            Articles = new List<NewsArticle>();
        }
    }
}