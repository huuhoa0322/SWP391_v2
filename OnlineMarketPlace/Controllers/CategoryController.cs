using Microsoft.AspNetCore.Mvc;

namespace OnlineMarketPlace.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
