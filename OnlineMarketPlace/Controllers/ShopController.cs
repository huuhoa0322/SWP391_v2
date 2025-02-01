using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;

        private readonly ShopRepository _shopRepository = new();

        private readonly ProductRepository _productRepository = new();

        private readonly CategoryRepository _categoryRepository = new();

        private readonly RatingAndReviewRepository _ratingAndReviewRepository = new();

        public ShopController(ILogger<ShopController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Shop(int id)
        {
            //Console.WriteLine("ShopId: " + id + " | Time: " + DateTime.Now);

            var shop = await _shopRepository.GetShopByIdAsync(id);
            var rating = await _ratingAndReviewRepository.GetAverageRatingByShopAsync(id);

            if (shop == null)
            {
                ViewData["rating"] = rating.HasValue ? rating.Value.ToString("F1") : "0";
                ViewData["Shop"] = null;
                return View();
            }

            var categoryList = new CategoriesList();

            // Get catergories parent in shop
            categoryList.CategoriesParent = await _categoryRepository.GetCatgoryParentByShop(id);

            // Get categories child for each parent in shop
            var categoriesChild = new List<List<CategoryModel>>();

            foreach (var parent in categoryList.CategoriesParent)
            {
                var childList = await _categoryRepository.GetCatgoryChildByShop(parent.Id, id);
                categoriesChild.Add(childList);
            }

            categoryList.CategoriesChild = categoriesChild;

            //for (int i = 0; i < categoryList.CategoriesParent.Count; i++)
            //{
            //    var parent = categoryList.CategoriesParent[i];
            //    var children = categoryList.CategoriesChild[i];

            //    Console.WriteLine($"Danh mục cha: {parent.Name}");
            //    Console.WriteLine($"Danh mục con count: {children?.Count ?? 0}");
            //}


            ViewData["CategoriesList"] = categoryList;
            ViewData["shop"] = shop;
            ViewData["rating"] = rating.HasValue ? rating.Value.ToString("F1") : "0";
            //Console.WriteLine("Rating:" + ViewData["rating"]);
            return View();
        }

    }
}
