using Network_Recon_Dashboard.Models;
using NetworkWebRecon.Models;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetworkWebRecon.Services
{
    public class ReconService
    {
        public async Task<ReconViewModel> ScanAsync(string target)
        {
            var result = new ReconViewModel();

            // Save Target
            result.Target = target;

            // DNS Resolution
            IPAddress[] addresses =
                await Dns.GetHostAddressesAsync(target);

            if (addresses.Length > 0)
            {
                result.ResolvedIP =
                    addresses[0].ToString();
            }

            // Host Discovery using Ping
            try
            {
                using Ping ping = new Ping();

                PingReply reply =
                    await ping.SendPingAsync(target, 2000);

                result.IsAlive =
                    reply.Status == IPStatus.Success;
            }
            catch
            {
                result.IsAlive = false;
            }

            // Common Ports
            int[] commonPorts =
            {
                21,
                22,
                25,
                53,
                80,
                110,
                135,
                139,
                143,
                443,
                445,
                3306,
                3389,
                8080
            };

            // Port Scanning
            foreach (int port in commonPorts)
            {
                bool isOpen =
                    await CheckPortAsync(
                        result.ResolvedIP,
                        port);

                string banner = "";

                // If the Port is open, try to get the Banner
                if (isOpen)
                {
                    if (port == 80 || port == 443 || port == 8080)
                    {
                        banner = await GetHttpBannerAsync(
                            result.ResolvedIP,
                            port);
                    }
                    else
                    {
                        banner = await GetBannerAsync(
                            result.ResolvedIP,
                            port);
                    }
                }

                result.Ports.Add(new PortResult
                {
                    Port = port,
                    Service = GetServiceName(port),
                    IsOpen = isOpen,
                    Banner = banner
                });
            }

            return result;
        }


        // =========================================
        // Port Scanner
        // =========================================

        private async Task<bool> CheckPortAsync(
            string ip,
            int port)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            try
            {
                using TcpClient client =
                    new TcpClient();

                var connection =
                    client.ConnectAsync(ip, port);

                var timeout =
                    Task.Delay(1000);

                var completed =
                    await Task.WhenAny(
                        connection,
                        timeout);

                return completed == connection &&
                       client.Connected;
            }
            catch
            {
                return false;
            }
        }


        // =========================================
        // Banner Grabbing
        // =========================================

        private async Task<string> GetBannerAsync(
            string ip,
            int port)
        {
            try
            {
                using TcpClient client =
                    new TcpClient();

                // Timeout للـ Connection
                var connectTask =
                    client.ConnectAsync(ip, port);

                if (await Task.WhenAny(
                        connectTask,
                        Task.Delay(1000))
                    != connectTask)
                {
                    return "Connection timeout";
                }

                using NetworkStream stream =
                    client.GetStream();

                byte[] buffer =
                    new byte[1024];

                // Timeout for waiting for the Banner
                var readTask =
                    stream.ReadAsync(
                        buffer,
                        0,
                        buffer.Length);

                if (await Task.WhenAny(
                        readTask,
                        Task.Delay(1000))
                    != readTask)
                {
                    return "No banner received";
                }

                int bytesRead =
                    await readTask;

                if (bytesRead > 0)
                {
                    return System.Text.Encoding.ASCII
                        .GetString(
                            buffer,
                            0,
                            bytesRead)
                        .Trim();
                }

                return "No banner received";
            }
            catch
            {
                return "Banner unavailable";
            }
        }


        // =========================================
        // Service Detection
        // =========================================

        private string GetServiceName(int port)
        {
            return port switch
            {
                21 => "FTP",
                22 => "SSH",
                25 => "SMTP",
                53 => "DNS",
                80 => "HTTP",
                110 => "POP3",
                135 => "MS RPC",
                139 => "NetBIOS",
                143 => "IMAP",
                443 => "HTTPS",
                445 => "SMB",
                3306 => "MySQL",
                3389 => "RDP",
                8080 => "HTTP Proxy",
                _ => "Unknown"
            };
        }

        private async Task<string> GetHttpBannerAsync(string ip, int port)
        {
            try
            {
                using HttpClient client = new HttpClient();

                client.Timeout = TimeSpan.FromSeconds(3);

                string protocol = port == 443 ? "https" : "http";

                var response = await client.GetAsync(
                    $"{protocol}://{ip}:{port}"
                );

                string server = "";

                if (response.Headers.Server != null)
                {
                    server = response.Headers.Server.ToString();
                }

                return $"HTTP {response.StatusCode} | Server: {server}";
            }
            catch
            {
                return "HTTP service detected";
            }
        }
    }
}