using REs.Resources.ViewModels;
using REs.Resources.Pages;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace REs
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Регистрация страниц и их моделей представления
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageVM>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
