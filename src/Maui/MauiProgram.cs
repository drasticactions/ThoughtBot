using AIChatModel;
using Drastic.Services;
using Microsoft.Extensions.Logging;
using ThoughtPal.Services;
using CommunityToolkit.Maui;

namespace ThoughtBot.DotNetMaui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<IErrorHandlerService, ChatErrorHandler>();
        builder.Services.AddSingleton<IAppDispatcher, ChatAppDispatcher>();
        builder.Services.AddSingleton<AIClientWrapper>();
        builder.Services.AddSingleton<AIChatViewModel>();
        builder.UseMauiCommunityToolkit().UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}