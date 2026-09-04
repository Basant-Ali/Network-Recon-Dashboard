using Microsoft.AspNetCore.Mvc;
using NetworkWebRecon.Services;

namespace Network_Recon_Dashboard.Controllers
{
    public class ReconController : Controller
    {
        private readonly ReconService _reconService;
        public ReconController(ReconService reconService)
        {
            _reconService = reconService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Scan(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                ViewBag.ErrorMessage = "Please enter a target.";
                return View("Index");
            }
            var result = await _reconService.ScanAsync(target);
            return View("Index", result);
        }

    }
}
