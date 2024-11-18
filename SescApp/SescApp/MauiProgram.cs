﻿using Microsoft.Extensions.Logging;
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

            // Add device-specific services used by the SescApp.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddScoped<IScheduleService, ScheduleService>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
