using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class ProductRepository
    {
        private OnlineShoppingContext _context;

        public async Task<List<Product>> GetProductsAsync()
        {
            _context = new();
            return await _context.Products.Where(p => p.IsDeleted == false).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            _context = new();
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            _context = new();
            return await _context.Products
                .Where(p => p.CategoryId == categoryId && p.IsDeleted == false)
                .ToListAsync();
        }
        public async Task<List<Product>> SearchProductsByNameAsync(string searchString, int pageNumber, int pageSize)
        {
            _context = new();
            return await _context.Products
                .Where(p => p.Name.Contains(searchString) && p.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalProductsCountAsync(string searchString)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(searchString))
                .CountAsync();
        }
        //public async Task<List<Product>> GetProductsByPriceRangeAsync(double minPrice, double? maxPrice = null)
        //{
        //    using var context = new OnlineShoppingContext();

        //    var query = context.Products
        //        .Where(p => p.Price >= minPrice && p.IsDeleted == false);

        //    if (maxPrice.HasValue)
        //    {
        //        query = query.Where(p => p.Price <= maxPrice.Value); // Không còn lỗi kiểu dữ liệu
        //    }

        //    return await query.ToListAsync();
        //}
    
    public async Task<List<Product>> GetProductsByPriceRangeAsync(double minPrice, double maxPrice)
        {
            using (var context = new OnlineShoppingContext())
            {
                return await context.Products
                    .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                    .ToListAsync();
            }
        }
    }
}
