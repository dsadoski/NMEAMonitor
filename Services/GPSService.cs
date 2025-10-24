using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;

using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

public class GPSService
{
    private CancellationTokenSource? _cts;
    private bool _isRunning;
    public event Action<Location>? OnLocationReceived;
    bool isInitialized = false;

    public async Task Start()
    {
        if (_isRunning)
            return;
        _isRunning = true;
        _cts = new CancellationTokenSource();
        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                await Task.Delay(1000); // Wait for 1 second
                var location = await Geolocation.Default.GetLocationAsync();
                OnLocationReceived.Invoke(location);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GPS Service Error: {ex.Message}");
        }
    }

    public async Task<Location> GetCachedLocation()
    {
        try
        {
            return await Geolocation.Default.GetLastKnownLocationAsync();

            /*if (location != null)
                return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";*/
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
        }
        catch (Exception ex)
        {
            // Unable to get location
        }

        return new Location();
    }

    public void Stop()
    {
        _cts?.Cancel();
        
    }

}
