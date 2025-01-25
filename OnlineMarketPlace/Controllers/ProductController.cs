using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        private readonly ProductRepository _productRepository = new();

        private readonly CategoryRepository categoryRepository = new();

        private readonly RatingAndReviewRepository ratingAndReviewRepository = new();

        private readonly ShopRepository shopRepository = new();


        //[HttpGet]

        public ProductController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        //public async Task<IActionResult> Details(int id)
        //{
        //    if (id == 0) return RedirectToAction("Index");
        //    var productById = await _productRepository.GetProductByIdAsync(id);
        //    if (productById == null) return NotFound();
        //    return View(productById);
        //}
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0) return RedirectToAction("Index");

            //var categoriesParent = categoryRepository.GetCatgoryParent();

            //var categoriesChildList = categoriesParent
            //    .Select(parent => categoryRepository.GetCatgoryChild(parent.Id))
            //    .ToList();
            //CategoriesList categoriesList = new CategoriesList(categoriesParent, categoriesChildList);
            //ViewData["CategoriesList"] = categoriesList;

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound(); // lấy 1 san pham theo id

            List<RatingAndReview> reviews = await ratingAndReviewRepository.GetReviewsByProductIdAsync(id);
            ViewData["rv"] = reviews;

            var shop = await shopRepository.GetShopByIdAsync(product.SellerId);
            ViewData["Shop"] = shop;

            var relatedProducts = await _productRepository.GetProductsByCategoryIdAsync(product.CategoryId);
            ViewData["RelatedProducts"] = relatedProducts;

            return View(product);
        }




    }
}
// add 20/1/2025