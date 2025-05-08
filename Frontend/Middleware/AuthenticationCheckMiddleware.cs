namespace Frontend.Middleware;

public class AuthenticationCheckMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();
        if (path != null && (path.Contains("/login") || path.Contains("/register")))
        {
            await _next(context);
            return;
        }

        var accountId = context.Session.GetString("AccountId");
        if (string.IsNullOrEmpty(accountId))
        {
            context.Response.Redirect("/Login");
            return;
        }

        await _next(context);
    }

}