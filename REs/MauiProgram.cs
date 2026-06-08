using REs.Resources.ViewModels;
using REs.Resources.Pages;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using REs.Services;

namespace REs
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                // Пакеты
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            #region Регистрация
            // Регистрация страниц и их моделей представления
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageVM>();

            builder.Services.AddTransient<InProccessPage>();
            builder.Services.AddTransient<InProccessVM>();

            builder.Services.AddTransient<CompletedPage>();
            builder.Services.AddTransient<CompletedVM>();

            builder.Services.AddTransient<NewTaskPage>();
            builder.Services.AddTransient<NewTaskVM>();

            // Регистрация сервиса API
            builder.Services.AddSingleton<APIServices>();
            #endregion
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
