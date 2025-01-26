using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;

        private readonly ShopRepository _shopRepository = new();

        public ShopController(ILogger<ShopController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Shop(int id)
        {

            return View();
        }
    }
}
