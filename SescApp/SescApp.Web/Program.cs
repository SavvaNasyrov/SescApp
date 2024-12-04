using SescApp.Integration;
using SescApp.Integration.Schedule.Services;
using SescApp.Integration.Schedule.Services.Implementations;
using SescApp.Shared.Services;
using SescApp.Web.Components;
using SescApp.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add device-specific services used by the SescApp.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddScoped<IUserStorage, UserStorage>();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IScheduleService, ScheduleService>();

builder.Services.AddMemoryCache();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(SescApp.Shared._Imports).Assembly,
        typeof(SescApp.Web.Client._Imports).Assembly);

app.Run();
