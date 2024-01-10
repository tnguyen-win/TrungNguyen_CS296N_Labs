using PopularGameEngines.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PopularGameEngines.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        public IActionResult Overview() => View();

        public IActionResult References() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
