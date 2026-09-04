using Microsoft.AspNetCore.Mvc;
using NetworkWebRecon.Models;
using NetworkWebRecon.Services;

namespace NetworkWebRecon.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReconService _reconService;
        private readonly XssService _xssService;

        public ReportController(
            ReconService reconService,
            XssService xssService)
        {
            _reconService = reconService;
            _xssService = xssService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Generate(
            string target,
            string webUrl,
            string xssParameter)
        {
            var report = new ReportViewModel();

            // Network Recon
            var networkResult =
                await _reconService.ScanAsync(target);

            report.Target =
                networkResult.Target;

            report.ResolvedIP =
                networkResult.ResolvedIP;

            report.IsAlive =
                networkResult.IsAlive;

            report.Ports =
                networkResult.Ports;

            // Web Recon
            try
            {
                using HttpClient client =
                    new HttpClient();

                client.Timeout =
                    TimeSpan.FromSeconds(5);

                var response =
                    await client.GetAsync(webUrl);

                report.WebUrl = webUrl;

                report.WebStatusCode =
                    (int)response.StatusCode;

                report.WebStatus =
                    response.StatusCode.ToString();

                report.WebServer =
                    response.Headers.Server?.ToString()
                    ?? "Unknown";
            }
            catch
            {
                report.WebUrl = webUrl;
                report.WebStatusCode = 0;
                report.WebStatus = "Unable to connect";
                report.WebServer = "Unknown";
            }

            // XSS Test
            if (!string.IsNullOrWhiteSpace(xssParameter))
            {
                report.XssParameter =
                    xssParameter;

                report.XssReflectionDetected =
                    await _xssService.TestReflectionAsync(
                        webUrl,
                        xssParameter);
            }

            return View("Report", report);
        }
    }
}