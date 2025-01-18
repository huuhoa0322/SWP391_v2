using OnlineMarketPlace.Repository;
using OnlineMarketPlace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace OnlineMarketPlace.Controllers
{
    public class CategoryController : Controller
    {
        private readonly OnlineShoppingContext _dataContext;
        public CategoryController(OnlineShoppingContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            CategoryModel? category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();

            if (category == null) return RedirectToAction("Index");

            var productsByCategory = _dataContext.Products.Where(p => p.CategoryId == category.Id);

            return View(await productsByCategory.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
