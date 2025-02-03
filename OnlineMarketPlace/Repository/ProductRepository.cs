using Microsoft.AspNetCore.Mvc;
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

        public async Task<int> GetTotalProductsCountAsync(string searchString)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(searchString))
                .CountAsync();
        }

        public async Task<List<Product>> SearchProductsByNameAsync(string searchString)
        {
            using var context = new OnlineShoppingContext();

            return await context.Products
                .Where(p => p.Name.Contains(searchString) && p.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<List<Product>> GetSortedProductsAsync(string sortBy)
        {
            using var context = new OnlineShoppingContext();

            IQueryable<Product> query = context.Products.Where(p => p.IsDeleted == false);

            switch (sortBy)
            {
                case "price-asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "name-asc":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "name-desc":
                    query = query.OrderByDescending(p => p.Name);
                    break;
                default:
                    query = query.OrderBy(p => p.Id); // Mặc định theo ID
                    break;
            }

            return await query.ToListAsync();
        }

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
