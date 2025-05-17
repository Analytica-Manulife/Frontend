using System;
using System.Collections.Generic;

public class InvoiceItem
{
    public double Amount { get; set; }
    public string Date { get; set; }  // Using string for date format
    public string Description { get; set; }
    public string ProductCode { get; set; }
    public double Quantity { get; set; }  // Changed from int to double to handle non-integer values
    public double? Tax { get; set; }  // Made nullable to match JSON
    public string Unit { get; set; }  // Optional in JSON
    public double UnitPrice { get; set; }
}
