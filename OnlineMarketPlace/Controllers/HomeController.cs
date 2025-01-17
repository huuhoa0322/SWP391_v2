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

        private readonly ProductRepository productRepository = new();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {;
            var products = productRepository.GetProducts();
            return View(products);
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
