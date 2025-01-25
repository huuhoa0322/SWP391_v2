using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var viewModel = new CategoriesList();

            // Retrieve all parent categories
            viewModel.CategoriesParent = await categoryRepository.GetCatgoryParent();

            // Retrieve child categories for each parent
            var childCategoriesArray = await Task.WhenAll(
                viewModel.CategoriesParent.Select(async parent =>
                {
                    using (var context = new OnlineShoppingContext()) // use other context for each query
                    {
                        return await context.Categories
                            .Where(c => c.ParentId == parent.Id)
                            .ToListAsync();
                    }
                })
            );
            viewModel.CategoriesChild = childCategoriesArray.ToList();
            //CategoriesList categoriesList = new CategoriesList(categoriesParent, flattenedCategoriesChildList);
            ViewData["CategoriesList"] = viewModel;
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
