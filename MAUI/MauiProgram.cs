using CommunityToolkit.Maui;
using MAUI.Services.AuthenticationServices;
using MAUI.Services.ToDoSevices;
using MAUI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MAUI
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

#if DEBUG
		    builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<IAuthentication, Authentication>();
            builder.Services.AddTransient<IToDo, ToDo>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<AddToDoPage>();
            return builder.Build();
        }
    }
}