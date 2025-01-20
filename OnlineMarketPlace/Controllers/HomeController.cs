using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserRepository userRepository = new();

        private readonly ProductRepository productRepository = new();

        private readonly CategoryRepository categoryRepository = new();

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            
            var products = await productRepository.GetProductsAsync();
            var categoriesParent = categoryRepository.GetCatgoryParent();

            //change for each parent category, get all child categories. using linq

            var categoriesChildList = categoriesParent
                .Select(parent => categoryRepository.GetCatgoryChild(parent.Id))
                .ToList();
            CategoriesList categoriesList = new CategoriesList(categoriesParent, categoriesChildList);
            ViewData["CategoriesList"] = categoriesList;
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
