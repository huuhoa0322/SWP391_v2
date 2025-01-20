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

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }


    }
}
// add 20/1/2025