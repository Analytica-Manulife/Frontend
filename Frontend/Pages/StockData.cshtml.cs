using Frontend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Frontend.Service;
using StockMarketMicroservice.Models;

namespace Frontend.Pages;

public class StockDataModel : PageModel
{
    private readonly StockService _stockService;
    private readonly YahooFinanceService _stockService2;
    private readonly NewsService _newsService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public StockDataModel(StockService stockService, YahooFinanceService stockService2, NewsService newsService, IHttpContextAccessor httpContextAccessor)
    {
        _stockService = stockService;
        _stockService2 = stockService2;
        _newsService = newsService;
        _httpContextAccessor = httpContextAccessor;
    }

    [BindProperty(SupportsGet = true)]
    public string Ticker { get; set; }

    [BindProperty]
    public int Quantity { get; set; }

    [BindProperty]
    public decimal Price { get; set; }
    
    [BindProperty]
    public decimal Total { get; set; }

    [BindProperty]
    public Guid AccountId { get; set; }

    public Stock Stock { get; set; }
    public YahooStock? LiveStockData { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<NewsArticle> Articles { get; set; } = new();
    public List<NewsArticle> AllArticles { get; set; } = new();

    // Helper method to retrieve and set AccountId from session
    private void SetAccountId()
    {
        var accountIdStr = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");

        Console.WriteLine($"Session AccountId: {accountIdStr}");

        if (!string.IsNullOrEmpty(accountIdStr) && Guid.TryParse(accountIdStr, out var parsedId))
        {
            AccountId = parsedId;
        }
        else
        {
            Console.WriteLine("Failed to retrieve AccountId from session or parsing failed.");
            AccountId = Guid.Empty;
        }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        SetAccountId(); // Retrieve and set AccountId from session
        
        if (string.IsNullOrEmpty(Ticker))
            return RedirectToPage("/MarketWatch");

        LiveStockData = await _stockService2.GetStockDataAsync(Ticker);
        Stock = await _stockService.GetByTickerAsync(Ticker);
        Total = Stock.Volume * Stock.Price;

        if (LiveStockData == null)
            return NotFound("Stock data not found.");

        // Fetch all articles
        AllArticles = await _newsService.GetAllAsync();

        // Extract the first word from the company name (assumed to be the ticker symbol for simplicity)
        var companyNameFirstWord = Stock.CompanyName.Split(' ').FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(companyNameFirstWord))
        {
            // Filter articles based on the company name's first word
            var filteredArticles = _newsService.FilterArticles(AllArticles, companyNameFirstWord);

            if (filteredArticles != null && filteredArticles.Count > 0)
            {
                Articles = filteredArticles;
            }
            else
            {
                Articles = AllArticles; // Default to all articles if no matches found
            }
        }
        else
        {
            Articles = AllArticles;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostBuyAsync()
    {
        SetAccountId(); // Retrieve and set AccountId from session
        
        var result = await _stockService.BuyStockAsync(AccountId, Ticker, Quantity, Price);
        Message = result ? "Stock purchase successful." : "Failed to purchase stock.";
        Console.WriteLine("Result" + result);
        return await OnGetAsync(); // Reload the page data
    }

    public async Task<IActionResult> OnPostSellAsync()
    {
        SetAccountId(); // Retrieve and set AccountId from session
        
        var result = await _stockService.SellStockAsync(AccountId, Ticker, Quantity, Price);
        Message = result ? "Stock sold successfully." : "Failed to sell stock.";
        Console.WriteLine("Result" + result);
        return await OnGetAsync(); // Reload the page data
    }

    public async Task<IActionResult> GetNewsForStock()
    {
        SetAccountId(); // Retrieve and set AccountId from session
        
        var result = await _stockService.SellStockAsync(AccountId, Ticker, Quantity, Price);
        Message = result ? "Stock sold successfully." : "Failed to sell stock.";
        Console.WriteLine("Result" + result);

        return await OnGetAsync(); // Reload the page data
    }
}
