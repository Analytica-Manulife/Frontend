using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using StockMarketMicroservice.Data;
using StockMarketMicroservice.Models;
using StockMarketService.Model;

public class PortfolioService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _baseUrl2;

    public PortfolioService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _baseUrl = config["ApiSettings:StockApiUrl"];
        _baseUrl2 = config["ApiSettings:TransectionApiUrl"];

    }

    public async Task<List<Portfolio>> GetPortfolioAsync(string accountId)
    {
        return await _httpClient.GetFromJsonAsync<List<Portfolio>>($"{_baseUrl}/Portfolio/{accountId}/portfolio");
    }

    public async Task<List<StockTransaction>> GetTransactionsAsync(string accountId)
    {
        return await _httpClient.GetFromJsonAsync<List<StockTransaction>>($"{_baseUrl2}/transections/StockTransaction/{accountId}");
    }
    public async Task<bool> BuyStockAsync(BuyStockRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Portfolio/BuyStock", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SellStockAsync(SellStockRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Portfolio/SellStock", request);
        return response.IsSuccessStatusCode;
    }
}