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
        public async Task<IActionResult> Shop(int id, int productId)
        {
            //Console.WriteLine("ShopId: " + id + " | Time: " + DateTime.Now);
            Console.WriteLine(productId);
            var shop = await _shopRepository.GetShopByIdAsync(id);
            var rating = await _ratingAndReviewRepository.GetAverageRatingByProductAsync(id);

            if (shop == null)
            {
                //ViewData["rating"] = rating.HasValue ? rating.Value.ToString("F1") : "0";
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

            //recommended product = random products same categoryId/cateParent/shop
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product != null)
            {
                var productsByCate = shop.Products.Where(p => p.CategoryId == product.CategoryId);
                var recommendProduct = new List<Product>();
                var productsByParentCategory = shop.Products
                    .Where(p => p.Category.ParentId == product.Category.ParentId && p.CategoryId != product.CategoryId);

                if (productsByCate.Count() >= 8)
                {
                    recommendProduct = productsByCate.OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                }
                else
                {
                    if (productsByCate.Count() + productsByParentCategory.Count() >= 8)
                    {
                        recommendProduct = productsByCate.OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                        recommendProduct.AddRange(productsByParentCategory.OrderBy(x => Guid.NewGuid()).Take(8 - recommendProduct.Count));
                    }
                    else
                    {
                        recommendProduct = productsByCate.OrderBy(x => Guid.NewGuid()).ToList();
                        recommendProduct.AddRange(productsByParentCategory.OrderBy(x => Guid.NewGuid()));
                        recommendProduct.AddRange(shop.Products.Where(p => p.Category.ParentId != product.Category.ParentId).OrderBy(x => Guid.NewGuid()).Take(8 - recommendProduct.Count - productsByParentCategory.Count()));
                    }
                }

                Console.WriteLine("kkkkk");

                ViewData["recommendProduct"] = recommendProduct;
            }


            ViewData["CategoriesList"] = categoryList;
            ViewData["shop"] = shop;
            ViewData["rating"] = rating.HasValue ? rating.Value.ToString("F1") : "0";
            //Console.WriteLine("Rating:" + ViewData["rating"]);
            return View();
        }

    }
}
