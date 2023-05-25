using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PersonDemoBlazorUI;
using PersonDemoBlazorUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
  
var configuration = builder.Configuration;
var apiBaseUrl = configuration.GetSection("ApiSettings")["BaseUrl"];

builder.Services.AddScoped(x => {
    var apiUrl = new Uri(apiBaseUrl);
    return new HttpClient() { BaseAddress = apiUrl };
});
builder.Services.AddScoped<HttpService>();
builder.Services.AddScoped<PersonService>();

await builder.Build().RunAsync();
