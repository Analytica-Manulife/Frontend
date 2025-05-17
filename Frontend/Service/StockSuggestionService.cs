
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Service
{
    public class StockSuggestionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public StockSuggestionService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["ApiSettings:InvoiceApiUrl"]; 
        }

        public async Task<string?> GetStockSuggestionAsync(object userPortfolio, object marketData)
        {
            var payload = new
            {
                user_portfolio = userPortfolio,
                market_data = marketData
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/suggest-stock", content);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error or throw exception as needed
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Expected response schema: { "suggestion": "string" }
            var result = JsonSerializer.Deserialize<StockSuggestionResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Suggestion;
        }

        private class StockSuggestionResponse
        {
            public string? Suggestion { get; set; }
        }
    }
}
