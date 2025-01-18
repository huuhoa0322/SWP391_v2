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
<<<<<<< HEAD
=======

>>>>>>> e5f578c6701fae8b3651ac3b10940e22b70f6e88
        public async Task<IActionResult> Index()
        {
            
            var products = await productRepository.GetProductsAsync();
            var categoriesParent = categoryRepository.GetCatgoryParent();

            //change for each parent category, get all child categories. using linq
<<<<<<< HEAD
            var categoriesChildList = categoriesParent
                .Select(parent => categoryRepository.GetCatgoryChild(parent.Id))
                .ToList();
=======
           var categoriesChildList = categoriesParent
               .Select(parent => categoryRepository.GetCatgoryChild(parent.Id))
               .ToList();
>>>>>>> e5f578c6701fae8b3651ac3b10940e22b70f6e88

            //var categoriesChildList = (await Task.WhenAll(
            //categoriesParent.Select(async parent => await categoryRepository.GetCategoryChildAsync(parent.Id)))).ToList();
            //Console.WriteLine();
            // insert to datview
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
