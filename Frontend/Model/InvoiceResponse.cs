namespace Frontend.Model;

public class InvoiceResponse
{
    public string InvoiceUrl { get; set; }
    public List<ParsedField> ParsedFields { get; set; }
}