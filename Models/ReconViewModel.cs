using Network_Recon_Dashboard.Models;

namespace NetworkWebRecon.Models
{
    public class ReconViewModel
    {
        public string Target { get; set; } = "";

        public string ResolvedIP { get; set; } = "";

        public bool IsAlive { get; set; }

        public List<PortResult> Ports { get; set; } = new();

        public DnsResult Dns { get; set; } = new();

        public int OpenPortsCount => Ports.Count(p => p.IsOpen);
    }
}