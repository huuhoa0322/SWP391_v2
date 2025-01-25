using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class ProductRepository
    {
        private OnlineShoppingContext _context;


        //public List<Product> GetProducts()
        //{
        //    _context = new();
        //    return _context.Products.ToList();

        //}

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


        //public async Task<List<Product>> GetProductsNumberAsync(int n)
        //{
        //    _context = new();
        //    return await _context.Products.Take(n).ToListAsync();
        //}


    }
}
