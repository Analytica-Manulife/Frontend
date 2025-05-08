using System.Text.Json;
using Frontend.Model;

namespace Frontend.Service;

public class NewsService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public NewsService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _baseUrl = config["ApiSettings:NewsApiUrl"];
    }

    public async Task<List<NewsArticle>> GetAllAsync()
    {
        var response = await _httpClient.GetStringAsync($"{_baseUrl}/news");
        return JsonSerializer.Deserialize<List<NewsArticle>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public List<NewsArticle> FilterArticles(List<NewsArticle> articles, string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return articles;

        return articles.Where(article =>
            (!string.IsNullOrEmpty(article.Headline) && article.Headline.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(article.Content) && article.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        ).ToList();
    }
}