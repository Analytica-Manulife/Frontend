using BudgetService.Properties.Data;

namespace Frontend.Filter;


public static class InvoiceItemConverter
{
    public static Transaction ConvertToTransaction(InvoiceItem item, Guid accountId)
    {
        return new Transaction
        {
            TransactionId = Guid.NewGuid(),
            AccountId = accountId,
            TransactionType = "utilities",
            Amount = Convert.ToDecimal(item.Amount),
            Details = $"{item.Description} (Product: {item.ProductCode}, Qty: {item.Quantity} {item.Unit})",
            TransactionDate = DateTime.TryParse(item.Date, out var parsedDate) ? parsedDate : DateTime.UtcNow
        };
    }
}