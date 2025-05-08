using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Frontend.Service;
using StockMarketMicroservice.Data;
using StockMarketMicroservice.Models;
using StockMarketService.Model;

public class PortfolioModel : PageModel
{
    private readonly PortfolioService _portfolioService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly StockService _stockService;

    public PortfolioModel(PortfolioService portfolioService,IHttpContextAccessor httpContextAccessor,StockService stockService)
    {
        _portfolioService = portfolioService;
        _httpContextAccessor = httpContextAccessor;
        _stockService = stockService;

    }

    [BindProperty(SupportsGet = true)]
    public string AccountId { get; set; }
    public List<Stock> Stocks { get; set; } = new();

    public List<Portfolio> PortfolioList { get; set; }
    public List<StockTransaction> TransactionList { get; set; }

    [BindProperty]
    public BuyStockRequest BuyRequest { get; set; }

    [BindProperty]
    public SellStockRequest SellRequest { get; set; }

    public async Task OnGetAsync()
    {
        var accountIdStr = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");
        Stocks = await _stockService.GetAllAsync();

        if (!string.IsNullOrEmpty(accountIdStr))
        {
            PortfolioList = await _portfolioService.GetPortfolioAsync(accountIdStr);
            TransactionList = await _portfolioService.GetTransactionsAsync(accountIdStr);
        }
    }
    private void SetAccountId()
    {
        var accountIdStr = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");

        Console.WriteLine($"Session AccountId: {accountIdStr}");

        if (!string.IsNullOrEmpty(accountIdStr) && Guid.TryParse(accountIdStr, out var parsedId))
        {
            AccountId = accountIdStr;
        }
        else
        {
            Console.WriteLine("Failed to retrieve AccountId from session or parsing failed.");
            AccountId = "";
        }
    }

    public async Task<IActionResult> OnPostBuyAsync()
    {
        var success = await _portfolioService.BuyStockAsync(BuyRequest);
        return RedirectToPage(new { AccountId });
    }

    public async Task<IActionResult> OnPostSellAsync()
    {
        var success = await _portfolioService.SellStockAsync(SellRequest);
        return RedirectToPage(new { AccountId });
    }
}