using PopularGameEngines.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PopularGameEngines.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult FAQ() => View();

        public IActionResult References() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
