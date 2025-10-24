using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using NMEAMon.Services;
using System.Net.Sockets;


namespace NMEAMon
{
    public partial class App : Application
    {
        UdpListenerService UdpListenerService;
        GPSService GPSService;
        public App(UdpListenerService UdpService, GPSService gPSService )
        {
            InitializeComponent();
            UdpListenerService = UdpService;
            GPSService = gPSService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new MainPage()) { Title = "NMEAMon" };
            window.Destroying += (sender, args) =>
            {
                UdpListenerService?.Stop();
                GPSService?.Stop();
            };

            return window;
        }

        
    }
}
