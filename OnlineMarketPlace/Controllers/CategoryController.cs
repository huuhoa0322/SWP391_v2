using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;
using System.Threading.Tasks;
using System.Linq;
using Castle.Components.DictionaryAdapter.Xml;

namespace OnlineMarketPlace.Controllers
{
    public class CategoryController : Controller
    {   // Khai bao repository cho san pham va danh muc
        private readonly ProductRepository _productRepository = new();
        private readonly CategoryRepository _categoryRepository = new();

        //Hien thi danh sach danh muc va san pham, gioi han so luong san pham hien thi
        public async Task<IActionResult> Index(int page = 1, int limit = 9)
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

            var products = await _productRepository.GetProductsAsync();
            var totalProducts = products.Count;

            // Pagination logic
            var totalPages = (int)Math.Ceiling((double)totalProducts / limit);
            var pagedProducts = products.Skip((page - 1) * limit).Take(limit).ToList();

            ViewData["Products"] = pagedProducts;
            ViewData["TotalProducts"] = totalProducts;
            ViewData["Limit"] = limit;
            ViewData["Page"] = page;
            ViewData["TotalPages"] = totalPages;

            return View();
        }

        public async Task<IActionResult> Search(string searchString, string price = null, string sortBy = null, int? categoryId = null, int page = 1, int limit = 9)
        {
            var viewModel = new CategoriesList();
            viewModel.CategoriesParent = await _categoryRepository.GetCatgoryParent();
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

            var products = await _productRepository.SearchProductsByNameAsync(searchString);

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            if (!string.IsNullOrEmpty(price))
            {
                var parts = price.Split('-');
                if (parts.Length == 2 && double.TryParse(parts[0], out double minPrice) && double.TryParse(parts[1], out double maxPrice))
                {
                    products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
                }
            }

            // Apply sorting
            products = sortBy switch
            {
                "price-asc" => products.OrderBy(p => p.Price).ToList(),
                "price-desc" => products.OrderByDescending(p => p.Price).ToList(),
                "name-asc" => products.OrderBy(p => p.Name).ToList(),
                "name-desc" => products.OrderByDescending(p => p.Name).ToList(),
                _ => products
            };

            var totalProducts = products.Count();
            var totalPages = (int)Math.Ceiling((double)totalProducts / limit);
            var pagedProducts = products.Skip((page - 1) * limit).Take(limit).ToList();

            ViewData["Products"] = pagedProducts;
            ViewData["TotalProducts"] = totalProducts;
            ViewData["Limit"] = limit;
            ViewData["Page"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["SearchString"] = searchString;
            ViewData["CategoryId"] = categoryId;
            ViewData["SelectedPrice"] = price;
            ViewData["SortBy"] = sortBy;

            return View("Index");
        }


        // Similarly update FilterByPrice and Sort methods to include pagination handling


        //Hien thi san pham theo danh muc
        public async Task<IActionResult> ProductsByCategory(int categoryId, int page = 1, int limit = 9)
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

            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            var totalProducts = products.Count();

            // Pagination logic
            var totalPages = (int)Math.Ceiling((double)totalProducts / limit);
            var pagedProducts = products.Skip((page - 1) * limit).Take(limit).ToList();

            ViewData["Products"] = pagedProducts;
            ViewData["TotalProducts"] = totalProducts;
            ViewData["Limit"] = limit;
            ViewData["Page"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["CategoryId"] = categoryId;

            return View("Index");
        }


        //Loc san pham theo gia
        [HttpGet("Category/FilterByPriceRange")]
        public async Task<IActionResult> FilterByPrice(string price, int? categoryId, int limit = 9)
        {
            double minPrice = 0, maxPrice = double.MaxValue;

            // Xử lý khoảng giá từ các radio button
            if (!string.IsNullOrEmpty(price))
            {
                var parts = price.Split('-');
                // Kiểm tra và gán giá trị minPrice từ phần đầu của khoảng giá
                if (parts.Length > 0 && double.TryParse(parts[0], out double min))
                    minPrice = min;

                // Kiểm tra và gán giá trị maxPrice từ phần sau của khoảng giá
                if (parts.Length > 1 && double.TryParse(parts[1], out double max))
                    maxPrice = max;
            }

            var viewModel = new CategoriesList
            {
                CategoriesParent = await _categoryRepository.GetCatgoryParent()
            };

            // Lấy danh mục con cho từng danh mục cha
            var childCategoriesArray = await Task.WhenAll(
                viewModel.CategoriesParent.Select(async parent =>
                {
                    using (var context = new OnlineShoppingContext()) // Kết nối với cơ sở dữ liệu
                    {
                        return await context.Categories
                            .Where(c => c.ParentId == parent.Id) // Tìm danh mục con theo ParentId
                            .ToListAsync();
                    }
                })
            );
            viewModel.CategoriesChild = childCategoriesArray.ToList(); // Gán danh mục con vào viewModel
            ViewData["CategoriesList"] = viewModel; // Lưu danh mục vào ViewData để hiển thị

            List<Product> products;

            // Kiểm tra xem categoryId có được truyền vào không
            if (categoryId.HasValue)
            {
                products = await _productRepository.GetProductsByCategoryIdAsync(categoryId.Value);
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
            }
            else
            {
                products = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            }

            // Lưu tổng số sản phẩm và limit để xử lý "see more"
            ViewData["TotalProducts"] = products.Count;
            ViewData["Limit"] = limit;
            
            // Chỉ lấy số lượng sản phẩm theo limit
            ViewData["Products"] = products.Take(limit).ToList();
            
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;
            ViewData["CategoryId"] = categoryId;
            ViewData["SelectedPrice"] = price; // Lưu giá trị price đã chọn

            return View("Index");
        }

        // Sap xep san pham theo tieu chi
        [HttpGet("Category/Sort")]
        public async Task<IActionResult> Sort(string sortBy, int? categoryId, string price, int limit = 9)
        {
            double minPrice = 0, maxPrice = double.MaxValue;

            // Xử lý khoảng giá nếu có
            if (!string.IsNullOrEmpty(price))
            {
                var parts = price.Split('-');
                if (parts.Length > 0 && double.TryParse(parts[0], out double min))
                    minPrice = min;
                if (parts.Length > 1 && double.TryParse(parts[1], out double max))
                    maxPrice = max;
            }

            // Lấy danh mục như cũ
            var viewModel = new CategoriesList
            {
                CategoriesParent = await _categoryRepository.GetCatgoryParent()
            };

            // Lấy danh muc con cua tung danh muc cha
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

            // Lấy sản phẩm theo category (nếu có)
            var products = categoryId.HasValue
                ? await _productRepository.GetProductsByCategoryIdAsync(categoryId.Value)
                : await _productRepository.GetProductsAsync();

            // Áp dụng lọc giá nếu có
            if (!string.IsNullOrEmpty(price))
            {
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
            }

            // Áp dụng sắp xếp
            products = sortBy switch
            {
                "price-asc" => products.OrderBy(p => p.Price).ToList(),
                "price-desc" => products.OrderByDescending(p => p.Price).ToList(),
                "name-asc" => products.OrderBy(p => p.Name).ToList(),
                "name-desc" => products.OrderByDescending(p => p.Name).ToList(),
                _ => products
            };

            ViewData["TotalProducts"] = products.Count;
            ViewData["Limit"] = limit;
            ViewData["Products"] = products.Take(limit).ToList();
            ViewData["SortBy"] = sortBy;
            ViewData["CategoryId"] = categoryId;
            ViewData["SelectedPrice"] = price;

            return View("Index");
        }
    }
}
