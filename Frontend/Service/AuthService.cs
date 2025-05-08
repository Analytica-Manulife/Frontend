using Frontend.Model;

namespace Frontend.Service;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    
    public AuthService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = config["ApiSettings:AuthApiUrl"];
    }

    public async Task<(bool Success, string Message, string Email, string Name, decimal Balance, Guid AccountId)> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/login", new { email, password });
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return (false, error, null, null, 0, Guid.Empty);
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return (true, result.Message, result.Email, result.Name, result.Balance, result.Account);
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string name, string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/register", new { name, email, password });
        var message = await response.Content.ReadAsStringAsync();
        return (response.IsSuccessStatusCode, message);
    }
}