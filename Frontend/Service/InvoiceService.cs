using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using BudgetService.Properties.Data;
using Frontend.Model;

namespace Frontend.Service
{
    public class InvoiceApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public InvoiceApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["ApiSettings:InvoiceApiUrl"];
        }

        public async Task<InvoiceResponse?> UploadInvoiceAsync(Stream fileStream, string fileName)
        {
            var form = new MultipartFormDataContent();
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            form.Add(fileContent, "file", fileName);

            Console.WriteLine($"[InvoiceApiService] Sending request to {_baseUrl}/upload-invoice");

            var response = await _httpClient.PostAsync($"{_baseUrl}/upload-invoice", form);

            Console.WriteLine($"[InvoiceApiService] HTTP status: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("[InvoiceApiService] Raw response:");
            Console.WriteLine(json);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("[InvoiceApiService] Failed to upload invoice.");
                return null;
            }

            try
            {
                // Configure JSON options to handle snake_case property names
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };

                InvoiceResponse result = JsonSerializer.Deserialize<InvoiceResponse>(json, options);
                Console.WriteLine("[InvoiceApiService] Successfully deserialized invoice response.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[InvoiceApiService] Error deserializing response: {ex.Message}");
                return null;
            }
        }
        public async Task<StoryResponse?> GenerateWeeklyStoryAsync(List<Transaction> transactions)
        {
            try
            {
                Console.WriteLine($"[InvoiceApiService] Sending transactions to {_baseUrl}/weekly-story");

                var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/weekly-story", content);

                Console.WriteLine($"[InvoiceApiService] HTTP status: {response.StatusCode}");

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("[InvoiceApiService] Raw response:");
                Console.WriteLine(responseBody);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("[InvoiceApiService] Failed to get story.");
                    return null;
                }

                var result = JsonSerializer.Deserialize<StoryResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Console.WriteLine("[InvoiceApiService] Successfully deserialized story response.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[InvoiceApiService] Error in GenerateWeeklyStoryAsync: {ex.Message}");
                return null;
            }
        }

    }
}
