using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SescApp.Shared.Services;
using SescApp.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the SescApp.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();
