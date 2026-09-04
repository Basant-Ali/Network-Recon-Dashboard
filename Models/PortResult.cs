namespace Network_Recon_Dashboard.Models
{
    public class PortResult
    {
        public int Port { get; set; }
        public string Service { get; set; } = "";
        public bool IsOpen{ get; set; }
        public string Banner { get; set; } = "";
    }
}
