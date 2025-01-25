using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var viewModel = new CategoriesList();

            // Retrieve all parent categories
            viewModel.CategoriesParent = await _categoryRepository.GetCatgoryParent();

            // Retrieve child categories for each parent
            var childCategoriesArray = await Task.WhenAll(
                viewModel.CategoriesParent.Select(async parent =>
                {
                    using (var context = new OnlineShoppingContext())
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

            var products = await _productRepository.GetProductsAsync();
            ViewData["Products"] = products;

            return View();
        }
    }
}
