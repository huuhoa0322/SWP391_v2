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

        private readonly DiscountRepository discountRepository = new();

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int newlimit)
        {
            if (newlimit == 0)
            {
                newlimit = 8;
            }
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
            ViewData["CategoriesList"] = viewModel;
            ViewData["limit"] = newlimit;
            var discounts = await discountRepository.GetProductDiscount();
            ViewData["Discount"] = discounts;
            //Console.WriteLine(discounts[0]);
            return View(products);
        }


        public IActionResult Contact()
        {
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
