using Microsoft.AspNetCore.Mvc;
using NetworkWebRecon.Services;

namespace NetworkWebRecon.Controllers
{
    public class WebSecurityController : Controller
    {
        private readonly XssService _xssService;

        public WebSecurityController(
            XssService xssService)
        {
            _xssService = xssService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Scan(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                ViewBag.Error =
                    "Please enter a URL.";

                return View("Index");
            }

            try
            {
                using HttpClient client =
                    new HttpClient();

                client.Timeout =
                    TimeSpan.FromSeconds(5);

                var response =
                    await client.GetAsync(url);

                ViewBag.Url = url;

                ViewBag.StatusCode =
                    (int)response.StatusCode;

                ViewBag.Status =
                    response.StatusCode.ToString();

                if (response.Headers.Server != null)
                {
                    ViewBag.Server =
                        response.Headers.Server.ToString();
                }
                else
                {
                    ViewBag.Server = "Unknown";
                }

                return View("Index");
            }
            catch
            {
                ViewBag.Error =
                    "Unable to connect to the URL.";

                return View("Index");
            }
        }


        [HttpGet]
        public IActionResult Test(string query)
        {
            ViewBag.Query = query;

            return View();
        }


        // XSS Scanner
        [HttpPost]
        public async Task<IActionResult> XssScan(
            string url,
            string parameter)
        {
            if (string.IsNullOrWhiteSpace(url) ||
                string.IsNullOrWhiteSpace(parameter))
            {
                ViewBag.XssError =
                    "Please enter URL and parameter.";

                return View("Index");
            }

            bool reflected =
                await _xssService.TestReflectionAsync(
                    url,
                    parameter);

            ViewBag.XssUrl = url;
            ViewBag.XssParameter = parameter;
            ViewBag.XssResult = reflected;

            return View("Index");
        }
    }
}