using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Frontend.Service;
using System.ComponentModel.DataAnnotations;
using BudgetService.Enum;
using BudgetService.Properties.Data;
using Frontend.Model;

public class UploadInvoiceModel : PageModel
{
    private readonly InvoiceApiService _invoiceApiService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly BudgetApiService _budgetApiService;

    public UploadInvoiceModel(IHttpContextAccessor httpContextAccessor, InvoiceApiService invoiceApiService,
        BudgetApiService budgetApiService)
    {
        _httpContextAccessor = httpContextAccessor;
        _budgetApiService = budgetApiService;
        _invoiceApiService = invoiceApiService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Please select a file.")]
    public IFormFile InvoiceFile { get; set; }
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    public InvoiceResponse? ParsedInvoiceResponse { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string AccountId { get; set; }
    
    public void OnGet()
    {
        // Nothing needed here
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

    public async Task<IActionResult> OnPostAsync()
    {
        SetAccountId();
        if (InvoiceFile == null || InvoiceFile.Length == 0)
        {
            ModelState.AddModelError("UploadedFile", "Please select a file to upload.");
            return Page();
        }
        
        using (var stream = InvoiceFile.OpenReadStream())
        {
            var response = await _invoiceApiService.UploadInvoiceAsync(stream, InvoiceFile.FileName);
            if (response != null)
            {
                ParsedInvoiceResponse = response;
                Transactions = InvoiceParser.ConvertInvoiceResponseToTransactions(ParsedInvoiceResponse, AccountId);
                Console.WriteLine(Transactions.Count);
                await CreateTransactionsFromInvoice();
                return Page();
            }
        }
        
        ModelState.AddModelError("", "Failed to process the invoice. Please try again.");
        return Page();
    }
    
    private async Task CreateTransactionsFromInvoice()
    {
        foreach (var txn in Transactions)
        {
            var success = await _budgetApiService.CreateTransactionAsync(AccountId, txn);
            Console.WriteLine($"Transaction created: {success}, Description: {txn.Details}");
        }
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
}