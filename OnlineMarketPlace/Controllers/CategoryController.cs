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

    }
}
