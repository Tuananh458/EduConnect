using EduConnect.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using EduConnect.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ===== LocalStorage & Auth =====
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// ===== Register core services =====
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<HocLieuService>();
builder.Services.AddScoped<CauHoiHocLieuService>();
builder.Services.AddScoped<LopHocService>();
builder.Services.AddScoped<HocLieuCauHoiService>();
builder.Services.AddScoped<HocSinhService>();

// ===== Custom Auth Handler =====
// ?? ??ng ký handler không inject HttpClient
builder.Services.AddTransient<AuthMessageHandler>();

// ===== HttpClient cho API =====
builder.Services.AddHttpClient("ApiClient", client =>
{
    // ?? BaseAddress: ??t ?úng URL backend API c?a b?n
    client.BaseAddress = new Uri("https://localhost:7276/");
})
.AddHttpMessageHandler<AuthMessageHandler>();

// ===== HttpClient m?c ??nh ?? inject vào service =====
builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("ApiClient");
});

// ?? Ch?y ?ng d?ng
await builder.Build().RunAsync();
