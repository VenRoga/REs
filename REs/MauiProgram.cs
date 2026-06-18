using REs.Resources.ViewModels;
using REs.Resources.Pages;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using REs.Services;
using System.Net.Http.Json;
using ModelsLib;


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
            builder.Services.AddHttpClient<APIServices>(client =>
            {
#if ANDROID
                client.BaseAddress = new Uri("http://10.0.2.2:5000/");
#elif WINDOWS
                client.BaseAddress = new Uri("http://localhost:5000/");
#else
                client.BaseAddress = new Uri("http://localhost:5000/"); 
#endif
            });
            #endregion
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
