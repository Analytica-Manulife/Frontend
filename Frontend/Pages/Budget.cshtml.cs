using BudgetService.Data;
using BudgetService.Enum;
using BudgetService.Properties.Data;
using Frontend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class BudgetModel : PageModel
{
    private readonly BudgetApiService _api;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly InvoiceApiService _invoiceApi;

    public BudgetModel(BudgetApiService api, IHttpContextAccessor httpContextAccessor, InvoiceApiService invoiceApi)
    {
        _api = api;
        _httpContextAccessor = httpContextAccessor;
        _invoiceApi = invoiceApi;

    }

    public string? Status { get; set; }
    public Budget? Budget { get; set; }
    public Budget? GeneratedBudget { get; set; }
    public string? Story { get; set; }

    public List<Transaction>? Transactions { get; set; }

    public async Task OnGetAsync()
    {
        var accountIdStr = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");
        Status = await _api.GetServiceStatusAsync();
        Budget = await _api.GetBudgetAsync(accountIdStr);
        Transactions = await _api.GetTransactionsAsync(accountIdStr);
        Console.WriteLine(Transactions.Count);
    }

    public async Task<IActionResult> OnPostGenerate(
         decimal income, decimal emi, decimal educationExpense, decimal medicalExpense)
    {
        var accountId = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");

        var request = new BudgetRequest
        {
            Income = income,
            EMI = emi,
            EducationExpense = educationExpense,
            MedicalExpense = medicalExpense
        };
        GeneratedBudget = await _api.GenerateBudgetAsync(accountId, request);
        Budget = await _api.GetBudgetAsync(accountId); 
        Transactions = await _api.GetTransactionsAsync(accountId);
        return Page();
    }

    public async Task<IActionResult> OnPostAddTransaction(
        string transactionType, decimal amount, string details)
    {        
        var accountId = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");

        var txn = new Transaction
        {
            AccountId = Guid.Parse(accountId),
            TransactionType = transactionType,
            Amount = amount,
            Details = details,
            TransactionDate = DateTime.UtcNow
        };

        await _api.CreateTransactionAsync(accountId, txn);
        Transactions = await _api.GetTransactionsAsync(accountId);
        Budget = await _api.GetBudgetAsync(accountId); 

        return Page();
    }

    public async Task<IActionResult> OnPostCreateCustom(
        DateTime periodStart, DateTime periodEnd, decimal Entertainment, decimal Education,
        decimal Investment, decimal DailyNeeds, decimal Housing, decimal Utilities,
        decimal Transportation, decimal Healthcare, decimal SavingsGoal, decimal Travel)
    {
        var accountId = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");

        Budget customBudget = new Budget
        {
            BudgetId = Guid.NewGuid(), // Add this line
            AccountId = Guid.Parse(accountId),
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
            EntertainmentBudget = Entertainment,
            EducationBudget = Education,
            InvestmentBudget = Investment,
            DailyNeedsBudget = DailyNeeds,
            HousingBudget = Housing,
            UtilitiesBudget = Utilities,
            TransportationBudget = Transportation,
            HealthcareBudget = Healthcare,
            SavingsGoal = SavingsGoal,
            TravelBudget = Travel
        };
        Console.WriteLine("Custom Budget Details:");
        Console.WriteLine($"AccountId: {customBudget.AccountId}");
        Console.WriteLine($"Period: {customBudget.PeriodStart:yyyy-MM-dd} to {customBudget.PeriodEnd:yyyy-MM-dd}");
        Console.WriteLine($"Entertainment: {customBudget.EntertainmentBudget}");
        Console.WriteLine($"Education: {customBudget.EducationBudget}");
        Console.WriteLine($"Investment: {customBudget.InvestmentBudget}");
        Console.WriteLine($"Daily Needs: {customBudget.DailyNeedsBudget}");
        Console.WriteLine($"Housing: {customBudget.HousingBudget}");
        Console.WriteLine($"Utilities: {customBudget.UtilitiesBudget}");
        Console.WriteLine($"Transportation: {customBudget.TransportationBudget}");
        Console.WriteLine($"Healthcare: {customBudget.HealthcareBudget}");
        Console.WriteLine($"Savings Goal: {customBudget.SavingsGoal}");
        Console.WriteLine($"Travel: {customBudget.TravelBudget}");

        await _api.CreateBudgetAsync(accountId, customBudget);
        Budget = await _api.GetBudgetAsync(accountId);
        return Page();
    }
    public string GetTransactionIcon(TransactionType type)
    {
        return type switch
        {
            TransactionType.Entertainment => "bi bi-controller",
            TransactionType.Education => "bi bi-journal-bookmark",
            TransactionType.Investment => "bi bi-graph-up",
            TransactionType.DailyNeeds => "bi bi-basket",
            TransactionType.Housing => "bi bi-house-door",
            TransactionType.Utilities => "bi bi-lightning",
            TransactionType.Transportation => "bi bi-truck",
            TransactionType.Healthcare => "bi bi-heart-pulse",
            TransactionType.Savings => "bi bi-piggy-bank",
            TransactionType.Travel => "bi bi-airplane",
            _ => "bi bi-receipt"
        };
    }
    public async Task<IActionResult> OnPostGenerateStoryAsync()
    {
        var accountId = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");

        if (string.IsNullOrEmpty(accountId))
        {
            Story = "Account ID not found in session.";
            return Page();
        }

        Transactions = await _api.GetTransactionsAsync(accountId);

        if (Transactions == null || Transactions.Count == 0)
        {
            Story = "No transactions available to generate a story.";
            return Page();
        }

        var storyResponse = await _invoiceApi.GenerateWeeklyStoryAsync(Transactions);

        Story = storyResponse?.Story ?? "Failed to generate story.";

        // Keep everything else updated
        Budget = await _api.GetBudgetAsync(accountId);

        return Page();
    }

}
