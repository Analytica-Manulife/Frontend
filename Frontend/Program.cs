using Frontend.Middleware;
using Frontend.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<NewsService>();
builder.Services.AddSingleton<StockService>();
builder.Services.AddSingleton<PortfolioService>();
builder.Services.AddSingleton<BudgetApiService>();

builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<YahooFinanceService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // adjust as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.UseSession();

// Inside Configure method in Startup.cs or similar method in Program.cs
app.UseMiddleware<AuthenticationCheckMiddleware>();

app.MapGet("/", context =>
{
    context.Response.Redirect("/MarketWatch");
    return Task.CompletedTask;
});
app.Run();