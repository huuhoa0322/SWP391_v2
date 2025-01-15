using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserRepository userRepository = new();



        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string a = userRepository.GetUsers()[0].Username;
            ViewData["Message"] = a;
            //ViewBag.Message2 = "welcome here ViewBag!";

            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
