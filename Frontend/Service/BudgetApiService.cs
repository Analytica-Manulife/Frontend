using System.Text.Json;
using BudgetService.Data;
using BudgetService.Properties.Data;

namespace Frontend.Service;

public class BudgetApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public BudgetApiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _baseUrl = config["ApiSettings:BudgetApiUrl"];
    }

    public async Task<string> GetServiceStatusAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>($"{_baseUrl}/budget/Budget/status");
        return result?["status"]?.ToString() ?? "Unknown";
    }

    public async Task<(bool isRequired, Budget? currentBudget)> CheckBudgetRequiredAsync(string accountId)
    {
        var response = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>($"{_baseUrl}/budget/Budget/{accountId}/require-budget");
        if (response == null || !response.ContainsKey("isRequired")) return (false, null);

        var isRequired = Convert.ToBoolean(response["isRequired"]);
        var budget = System.Text.Json.JsonSerializer.Deserialize<Budget>(response["currentBudget"]?.ToString() ?? "{}");
        return (isRequired, budget);
    }

    public async Task<Budget?> GetBudgetAsync(string accountId)
    {
        return await _httpClient.GetFromJsonAsync<Budget>($"{_baseUrl}/budget/Budget/{accountId}");
    }

    public async Task<Budget?> GenerateBudgetAsync(string accountId, BudgetRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/budget/Budget/{accountId}/generate", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<Budget>();
    }

    public async Task<bool> CreateBudgetAsync(string accountId, Budget budget)
    {
        var json = JsonSerializer.Serialize(budget, new JsonSerializerOptions
        {
            WriteIndented = true // Optional, makes it easier to read
        });

        Console.WriteLine("Request URL:");
        Console.WriteLine($"{_baseUrl}/budget/Budget/{accountId}/create");

        Console.WriteLine("JSON Body:");
        Console.WriteLine(json);

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/budget/Budget/{accountId}/create", budget);

        Console.WriteLine("Response:");
        Console.WriteLine(response.ToString());

        return response.IsSuccessStatusCode;
    }

    public async Task<List<Transaction>> GetTransactionsAsync(string accountId)
    {
        Console.WriteLine("called with account id"+ accountId);
        var result = await _httpClient.GetAsync($"{_baseUrl}/transection/Transaction/{accountId}");
        Console.WriteLine(result.ToString());
        
        Console.WriteLine($"{_baseUrl}/transection/Transaction/{accountId}");
        if (!result.IsSuccessStatusCode) return new List<Transaction>();
        return await result.Content.ReadFromJsonAsync<List<Transaction>>() ?? new List<Transaction>();
    }

    public async Task<bool> CreateTransactionAsync(string account_id,Transaction txn)
    {
        var result = await _httpClient.PostAsJsonAsync($"{_baseUrl}/transection/Transaction/create", txn);
        Console.WriteLine(result.ToString());
        Console.WriteLine($"{_baseUrl}/transection/Transaction/create");
        Console.WriteLine(result.Content.ToString());
        return result.IsSuccessStatusCode;
         }
}