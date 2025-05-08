using Microsoft.AspNetCore.Mvc.RazorPages;
using Frontend.Model;
using Frontend.Service;
using StockMarketMicroservice.Models;


public class MarketWatch : PageModel
{
    private readonly StockService _stockService;

    public MarketWatch(StockService stockService)
    {
        _stockService = stockService;
    }

    public List<Stock> Stocks { get; set; } = new();
    public List<Stock> TopGainers { get; set; } = new();

    public async Task OnGetAsync()
    {
        Stocks = await _stockService.GetAllAsync();
        TopGainers = _stockService.GetTopGainers(Stocks);
    }
}