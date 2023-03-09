global using BPIWebApplication.Client.Services.LoginServices;
global using BPIWebApplication.Client.Services.ProcedureServices;
global using BPIWebApplication.Shared;
using Blazored.SessionStorage;
using BPIWebApplication.Client;
using BPIWebApplication.Client.Services.CashierLogbookServices;
using BPIWebApplication.Client.Services.ManagementServices;
using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Client.Services.StandarizationServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IProcedureService, ProcedureService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddScoped<IPettyCashService, PettyCashService>();
builder.Services.AddScoped<ICashierLogbookService, CashierLogbookService>();
builder.Services.AddScoped<IStandarizationService, StandarizationService>();

builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();
