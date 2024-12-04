using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SescApp.Integration;
using SescApp.Integration.Schedule.Services;
using SescApp.Integration.Schedule.Services.Implementations;
using SescApp.Services;
using SescApp.Shared.Services;

namespace SescApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            var task = FileSystem.OpenAppPackageFileAsync("appsettings.json");

            task.Wait();

            using var stream = task.Result;

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();


            builder.Configuration.AddConfiguration(config);

            // Add device-specific services used by the SescApp.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddScoped<IUserStorage, UserStorage>();

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IScheduleService, ScheduleService>();

            builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
