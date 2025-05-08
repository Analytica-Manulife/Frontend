using Frontend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class LoginModel : PageModel
{
    private readonly AuthService _authService;
    private readonly ILogger<LoginModel> _logger;
    
    
    public LoginModel(AuthService authService, ILogger<LoginModel> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    public string Name { get; set; }

    [BindProperty]
    public bool IsLogin { get; set; } = true;

    public string ErrorMessage { get; set; }

    public void OnGet()
    {
        // No-op: Just render the page
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || (!IsLogin && string.IsNullOrEmpty(Name)))
        {
            ErrorMessage = "Please fill in all required fields.";
            return Page();
        }

        try
        {
            if (IsLogin)
            {
                var result = await _authService.LoginAsync(Email, Password);

                if (!result.Success)
                {
                    ErrorMessage = result.Message;
                    return Page();
                }

                // Store account ID in localStorage (done on frontend JS)
                TempData["AccountId"] = result.AccountId;
                // Store account ID in session
                HttpContext.Session.SetString("AccountId", result.AccountId.ToString());
                HttpContext.Session.SetString("Name", result.Name.ToString());
                HttpContext.Session.SetString("Email", result.Email.ToString());
                HttpContext.Session.SetString("Balance", result.Balance.ToString());

                return RedirectToPage("/NewsPage");
            }
            else
            {
                var result = await _authService.RegisterAsync(Name, Email, Password);

                if (!result.Success)
                {
                    ErrorMessage = result.Message;
                    return Page();
                }

                // Optional: redirect to login page with success message
                TempData["SuccessMessage"] = "Registration successful. Please log in.";
                return RedirectToPage("/Login");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication.");
            ErrorMessage = "An unexpected error occurred. Please try again.";
            return Page();
        }
    }
}