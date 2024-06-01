using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VolleyLeague.Client.Blazor;
using VolleyLeague.Client.Blazor.Authentication;
using VolleyLeague.Client.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAccount, AccountService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IMatchOrganizerService, MatchOrganizerService>();
builder.Services.AddScoped<ITypedResultService, TypedResultService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7068") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

//options =>
//{
//    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"));
//});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

await builder.Build().RunAsync();
