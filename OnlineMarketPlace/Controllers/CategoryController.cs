using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;
using System.Threading.Tasks;
using System.Linq;

namespace OnlineMarketPlace.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ProductRepository _productRepository = new();
        private readonly CategoryRepository _categoryRepository = new();

        private const int Pagesize = 9;

        public async Task<IActionResult> Index(int limit = 9)
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
            ViewData["CategoriesList"] = viewModel;

            // Giới hạn số lượng sản phẩm hiển thị ban đầu
            var products = await _productRepository.GetProductsAsync();
            var limitedProducts = products.Take(limit).ToList();

            ViewData["Products"] = limitedProducts;
            ViewData["TotalProducts"] = products.Count;
            ViewData["Limit"] = limit;

            return View();
        }

        public async Task<IActionResult> Search(string searchString, int pageNumber = 1)
        {
            var viewModel = new CategoriesList();

            // Lấy danh mục cha và con (giữ nguyên phần này)
            viewModel.CategoriesParent = await _categoryRepository.GetCatgoryParent();
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
            ViewData["CategoriesList"] = viewModel;

            // Tìm kiếm sản phẩm theo tên
            var products = await _productRepository.SearchProductsByNameAsync(searchString, pageNumber, Pagesize);
            var totalProducts = await _productRepository.GetTotalProductsCountAsync(searchString);

            ViewData["Products"] = products;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalProducts / (double)Pagesize);
            ViewData["SearchString"] = searchString;

            return View("Index");
        }

    }
}
