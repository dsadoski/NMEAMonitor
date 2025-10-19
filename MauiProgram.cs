using Microsoft.Extensions.Logging;
using NMEAMon.Models;
using NMEAMon.Services;

namespace NMEAMon
{
    
    public static class MauiProgram
    {
        
        public static MauiApp CreateMauiApp()
        {
            Setup setup = new Setup();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton(sp => new UdpListenerService(port: 10110));
            builder.Services.AddTransient(sp => new NmeaService(setup));

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
