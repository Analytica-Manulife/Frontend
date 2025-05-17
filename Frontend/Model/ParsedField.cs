namespace Frontend.Model;

public class ParsedField
{
    public double AmountDue { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string InvoiceId { get; set; }
    public double InvoiceTotal { get; set; }
    public List<InvoiceItem> Items { get; set; }
    public double PreviousUnpaidBalance { get; set; }
    public string PurchaseOrder { get; set; }
    public DateTime ServiceEndDate { get; set; }
    public DateTime ServiceStartDate { get; set; }
    public double SubTotal { get; set; }
    public double TotalTax { get; set; }
    public string VendorName { get; set; }
}
