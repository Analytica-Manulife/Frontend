using System;
using System.Collections.Generic;
using BudgetService.Properties.Data;
using Frontend.Model;

public static class InvoiceParser
{
    public static List<Transaction> ConvertInvoiceResponseToTransactions(InvoiceResponse invoiceResponse, string accountId)
    {
        var transactions = new List<Transaction>();

        if (invoiceResponse?.ParsedFields == null)
            return transactions;

        foreach (var parsedField in invoiceResponse.ParsedFields)
        {
            if (parsedField.Items == null)
                continue;

            foreach (var item in parsedField.Items)
            {
                var transaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    AccountId = Guid.Parse(accountId),
                    TransactionType = "ENTERTAINMENT",
                    Amount = Convert.ToDecimal(item.Amount),
                    Details = $"{item.Description} (Product: {item.ProductCode}, Qty: {item.Quantity} {item.Unit})",
                    TransactionDate = DateTime.UtcNow
                };

                transactions.Add(transaction);
            }
        }

        return transactions;
    }
}