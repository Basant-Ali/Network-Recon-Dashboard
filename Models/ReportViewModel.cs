using Network_Recon_Dashboard.Models;

namespace NetworkWebRecon.Models
{
    public class ReportViewModel
    {
        public string Target { get; set; } = "";

        public string ResolvedIP { get; set; } = "";

        public bool IsAlive { get; set; }

        public List<PortResult> Ports { get; set; } = new();

        public string WebUrl { get; set; } = "";

        public int WebStatusCode { get; set; }

        public string WebStatus { get; set; } = "";

        public string WebServer { get; set; } = "";

        public string XssParameter { get; set; } = "";

        public bool XssReflectionDetected { get; set; }
    }
}