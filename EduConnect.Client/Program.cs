using EduConnect.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using EduConnect.Client.Services;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();


builder.Services.AddScoped<HocLieuService>();
builder.Services.AddScoped<CauHoiHocLieuService>();





// ? K?t n?i t?i backend API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7276/api/")
});

await builder.Build().RunAsync();
