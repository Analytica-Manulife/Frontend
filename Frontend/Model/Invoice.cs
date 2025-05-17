using System.Collections.Generic;
using Frontend.Model;

public class Invoice
{
    public decimal AmountDue { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string DueDate { get; set; }
    public string InvoiceDate { get; set; }
    public string InvoiceId { get; set; }
    public decimal InvoiceTotal { get; set; }
    public List<InvoiceItem> Items { get; set; }
    public decimal PreviousUnpaidBalance { get; set; }
    public string PurchaseOrder { get; set; }
    public string ServiceEndDate { get; set; }
    public string ServiceStartDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalTax { get; set; }
    public string VendorName { get; set; }
}