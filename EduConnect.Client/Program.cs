using EduConnect.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using EduConnect.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ============== DI ==============
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Core services
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<HocLieuService>();
builder.Services.AddScoped<CauHoiHocLieuService>();
builder.Services.AddScoped<LopHocService>();
builder.Services.AddScoped<HocLieuCauHoiService>();
builder.Services.AddScoped<HocSinhService>();
builder.Services.AddScoped<BaiLamHocLieuService>();

// ========== HttpClient + JWT Handler ==========
builder.Services.AddTransient<AuthMessageHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7276/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthMessageHandler>();

// Default HttpClient resolve v? "ApiClient"
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

// ============== Run ==============
var app = builder.Build();

// ? Cho phép static class (vd: GoogleInterop) truy c?p container
Program.ServiceProvider = app.Services;

await app.RunAsync();

/// <summary>
/// Cho phép các class static nh? GoogleInterop truy c?p DI container
/// </summary>
public partial class Program
{
    public static IServiceProvider? ServiceProvider { get; set; }
}
