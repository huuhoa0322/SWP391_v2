using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ProductRepository _productRepository = new();
        private readonly CategoryRepository _categoryRepository = new();

        public async Task<IActionResult> Index()
        {
            var categoriesParent = _categoryRepository.GetCatgoryParent();
            var categoriesChildList = categoriesParent
                .Select(parent => _categoryRepository.GetCatgoryChild(parent.Id))
                .ToList();

            CategoriesList categoriesList = new CategoriesList(categoriesParent, categoriesChildList);
            ViewData["CategoriesList"] = categoriesList;

            var products = await _productRepository.GetProductsAsync();
            ViewData["Products"] = products;

            return View();
        }
    }
}
