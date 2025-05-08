using System.Net.Http.Json;
using System.Text.Json;
using Frontend.Model;
using Microsoft.Extensions.Configuration;
using StockMarketMicroservice.Models;

namespace Frontend.Service;

public class StockService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public StockService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _baseUrl = config["ApiSettings:StockApiUrl"];
    }

    // Get all stocks
    public async Task<List<Stock>> GetAllAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"{_baseUrl}/stocks");
            return JsonSerializer.Deserialize<List<Stock>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
            throw;
        }
    }

    // Get a single stock
    public async Task<Stock?> GetByTickerAsync(string ticker)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/stocks/{ticker}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Stock>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    // Get real-time data from Yahoo API through backend
    public async Task<YahooStock?> GetYahooStockAsync(string ticker)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Portfolio/GetStockData/{ticker}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<YahooStock>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Yahoo Fetch Error: {ex.Message}");
            return null;
        }
    }

    // Buy stock
    public async Task<bool> BuyStockAsync(Guid accountId, string ticker, int quantity, decimal price)
    {
        var request = new
        {
            AccountId = accountId,
            Ticker = ticker,
            Quantity = quantity,
            BuyPrice = price
        };
        Console.WriteLine("Buying stock for accountId: " + accountId);

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Portfolio/BuyStock", request);
        Console.WriteLine("response "+await response.Content.ReadAsStringAsync());
        
        return response.IsSuccessStatusCode;
    }

    // Sell stock
    public async Task<bool> SellStockAsync(Guid accountId, string ticker, int quantity, decimal price)
    {
        var request = new
        {
            AccountId = accountId,
            Ticker = ticker,
            Quantity = quantity,
            SellPrice = price
        };

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Portfolio/SellStock", request);
        return response.IsSuccessStatusCode;
    }

    public List<Stock> GetTopGainers(List<Stock> stocks, int count = 5)
    {
        return stocks.OrderByDescending(s => s.ChangeAmount).Take(count).ToList();
    }

    public List<Stock> GetTopLosers(List<Stock> stocks, int count = 4)
    {
        return stocks.OrderBy(s => s.ChangeAmount).Take(count).ToList();
    }
}
