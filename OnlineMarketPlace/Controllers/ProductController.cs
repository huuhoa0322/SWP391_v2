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

        //public ProductController(ILogger<LoginController> logger)
        //{
        //    _logger = logger;
        //} // bỏ

        [HttpGet]


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


            var categoriesParent = categoryRepository.GetCatgoryParent();

            //change for each parent category, get all child categories. using linq

            var categoriesChildList = categoriesParent
                .Select(parent => categoryRepository.GetCatgoryChild(parent.Id))
                .ToList();
            CategoriesList categoriesList = new CategoriesList(categoriesParent, categoriesChildList);
            ViewData["CategoriesList"] = categoriesList;

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }


    }
}
// add 20/1/2025