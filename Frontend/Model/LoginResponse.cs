namespace Frontend.Model;

public class LoginResponse
{
    public string Message { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public Guid Account { get; set; }
}