using NMEAMon.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class UdpListenerService
{
    private readonly int _port;
    private UdpClient? _udpClient;
    private CancellationTokenSource? _cts;

    public event Action<string>? OnMessageReceived;
    Record record;

    public UdpListenerService(int port)
    {
        _port = port;
    }

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _udpClient = new UdpClient(_port);

        Task.Run(async () =>
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    var result = await _udpClient.ReceiveAsync();
                    var message = Encoding.UTF8.GetString(result.Buffer);
                    OnMessageReceived?.Invoke(message);
                }
            }
            catch (ObjectDisposedException)
            {
                // Normal when stopping the listener
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UDP Listener Error: {ex.Message}");
            }
        });
    }

    public void Stop()
    {
        _cts?.Cancel();
        _udpClient?.Close();
    }
}
