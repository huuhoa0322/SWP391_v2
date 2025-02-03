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


        public async Task<IActionResult> Index(int limit = 9)
        {
            var viewModel = new CategoriesList
            {
                CategoriesParent = await _categoryRepository.GetCatgoryParent()
            };

            // Lấy danh mục con của từng danh mục cha
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

            // Lấy danh sách sản phẩm và giới hạn số lượng hiển thị
            var products = await _productRepository.GetProductsAsync();
            ViewData["Products"] = products.Take(limit).ToList();

            // Lưu total và limit để xử lý nút "Xem thêm"
            ViewData["TotalProducts"] = products.Count;
            ViewData["Limit"] = limit;

            return View();
        }


        public async Task<IActionResult> Search(string searchString)
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

            // Tìm kiếm sản phẩm theo tên (không giới hạn số lượng)
            var products = await _productRepository.SearchProductsByNameAsync(searchString);

            ViewData["Products"] = products;
            ViewData["SearchString"] = searchString;

            return View("Index");
        }


        public async Task<IActionResult> ProductsByCategory(int categoryId)
        {
            var viewModel = new CategoriesList
            {
                CategoriesParent = await _categoryRepository.GetCatgoryParent()
            };

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

            // Lấy danh sách sản phẩm theo danh mục
            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            ViewData["Products"] = products;

            return View("Index"); // Điều hướng đến trang chính nhưng chỉ hiển thị sản phẩm thuộc danh mục
        }


        [HttpGet("Category/FilterByPriceRange")]
        public async Task<IActionResult> FilterByPrice(string price)
        {
            double minPrice = 0, maxPrice = double.MaxValue;

            if (!string.IsNullOrEmpty(price))
            {
                var parts = price.Split('-');
                if (parts.Length > 0 && double.TryParse(parts[0], out double min))
                    minPrice = min;

                if (parts.Length > 1 && double.TryParse(parts[1], out double max))
                    maxPrice = max;
            }

            var viewModel = new CategoriesList
            {
                CategoriesParent = await _categoryRepository.GetCatgoryParent()
            };

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

            // Lọc sản phẩm theo giá đã parse
            var products = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            ViewData["Products"] = products;

            return View("Index");
        }


        [HttpGet("Category/Sort")]
        public async Task<IActionResult> Sort(string sortBy)
        {
            var viewModel = new CategoriesList
            {
                CategoriesParent = await _categoryRepository.GetCatgoryParent()
            };

            var childCategoriesArray = await Task.WhenAll(
                viewModel.CategoriesParent.Select(async parent =>
                {
                    using var context = new OnlineShoppingContext();
                    return await context.Categories
                        .Where(c => c.ParentId == parent.Id)
                        .ToListAsync();
                })
            );
            viewModel.CategoriesChild = childCategoriesArray.ToList();
            ViewData["CategoriesList"] = viewModel;

            // Lấy danh sách sản phẩm đã sắp xếp
            var products = await _productRepository.GetSortedProductsAsync(sortBy);
            ViewData["Products"] = products;
            ViewData["SortBy"] = sortBy; // Lưu trạng thái lọc để cập nhật UI

            return View("Index");
        }


    }
}
