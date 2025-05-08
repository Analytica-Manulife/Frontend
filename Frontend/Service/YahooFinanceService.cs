using System.Text.Json;
using Frontend.Model;

namespace Frontend.Service;

public class YahooFinanceService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "8CP0FKA1DUXMRYWO";
    public YahooFinanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<YahooStock?> GetStockDataAsync(string ticker)
    {
        var url = $"https://www.alphavantage.co/query?function=OVERVIEW&symbol={ticker}&apikey={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();

        var stock = JsonSerializer.Deserialize<YahooStock>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return stock;
    }
}