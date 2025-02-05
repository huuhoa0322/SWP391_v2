using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;
using System.Threading.Tasks;
using System.Linq;

namespace OnlineMarketPlace.Controllers
{
    public class CategoryController : Controller
    {   // Khai bao repository cho san pham va danh muc
        private readonly ProductRepository _productRepository = new();
        private readonly CategoryRepository _categoryRepository = new();

        //Hien thi danh sach danh muc va san pham, gioi han so luong san pham hien thi
        public async Task<IActionResult> Index(int limit = 9)
        {
            var viewModel = new CategoriesList
            {
                // Lay danh muc cha
                CategoriesParent = await _categoryRepository.GetCatgoryParent()
            };

            // Lay danh muc con cua tung danh muc cha
            var childCategoriesArray = await Task.WhenAll(
                viewModel.CategoriesParent.Select(async parent =>
                {
                    using (var context = new OnlineShoppingContext()) //ket noi voi CSDL
                    {
                        return await context.Categories
                            .Where(c => c.ParentId == parent.Id) // Tim danh muc con theo ParentId
                            .ToListAsync(); // Chuyen ket qua sang danh sach
                    }
                })
            );
            // Gop cac danh muc con vao viewModel
            viewModel.CategoriesChild = childCategoriesArray.ToList();
            ViewData["CategoriesList"] = viewModel; // Luu danh muc vao ViewData de hien thi

            // Lay danh sach san pham tu ProductRepository
            var products = await _productRepository.GetProductsAsync();
            ViewData["Products"] = products.Take(limit).ToList(); //lay so luong san pham theo limit

            //luu total va limit de xu ly nut "see more"
            ViewData["TotalProducts"] = products.Count;
            ViewData["Limit"] = limit;

            return View();
        }
        //search theo ten
        public async Task<IActionResult> Search(string searchString)
        {
            var viewModel = new CategoriesList();

            // Lay danh muc cha va con 
            viewModel.CategoriesParent = await _categoryRepository.GetCatgoryParent();
            var childCategoriesArray = await Task.WhenAll(
                viewModel.CategoriesParent.Select(async parent =>
                {
                    using (var context = new OnlineShoppingContext())
                    {
                        return await context.Categories
                            .Where(c => c.ParentId == parent.Id) // Tim danh muc con theo ParentId
                            .ToListAsync();
                    }
                })
            );
            viewModel.CategoriesChild = childCategoriesArray.ToList();
            ViewData["CategoriesList"] = viewModel;

            // Tim kiem san pham theo ten
            var products = await _productRepository.SearchProductsByNameAsync(searchString);

            ViewData["Products"] = products;
            ViewData["SearchString"] = searchString;

            return View("Index");
        }

        //Hien thi san pham theo danh muc
        public async Task<IActionResult> ProductsByCategory(int categoryId)
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

            // Loc san pham theo danh muc duoc truyen vao
            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            ViewData["Products"] = products;
            ViewData["CategoryId"] = categoryId; // Luu categoryId de xu ly Sort

            return View("Index");
        }

        //Loc san pham theo gia
        [HttpGet("Category/FilterByPriceRange")]
        public async Task<IActionResult> FilterByPrice(string price)
        {
            double minPrice = 0, maxPrice = double.MaxValue;
            // Parse khoang gia tu tham so truyen vao
            if (!string.IsNullOrEmpty(price))
            { 
                var parts = price.Split('-'); // Tach khoang gia theo dau '-'
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
        // Sap xep san pham theo tieu chi
        [HttpGet("Category/Sort")]
        public async Task<IActionResult> Sort(string sortBy, int? categoryId) // Cho phep categoryId = null
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

            // Neu categoryId co gia tri, loc theo danh muc, neu khong, lay tat ca san pham
            var products = categoryId.HasValue
                ? await _productRepository.GetProductsByCategoryIdAsync(categoryId.Value)
                : await _productRepository.GetProductsAsync(); // Lay tat ca san pham neu categoryId = null

            // Sap xep san pham theo tieu chi
            products = sortBy switch
            {
                "price-asc" => products.OrderBy(p => p.Price).ToList(),
                "price-desc" => products.OrderByDescending(p => p.Price).ToList(),
                "name-asc" => products.OrderBy(p => p.Name).ToList(),
                "name-desc" => products.OrderByDescending(p => p.Name).ToList(),
                _ => products // Mac dinh: khong sap xep
            };
            // Luu san pham da sap xep vao ViewData
            ViewData["Products"] = products;
            ViewData["SortBy"] = sortBy;
            ViewData["CategoryId"] = categoryId; // Lưu để hiển thị lại trên giao diện

            return View("Index");
        }
    }
}
