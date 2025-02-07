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

        //Long
        //Lay danh sach san pham theo categoryId
        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            _context = new(); 

            return await _context.Products
                .Where(p => p.CategoryId == categoryId && p.IsDeleted == false) //Loc san pham theo categoryId và IsDeleted = false
                .ToListAsync(); //tra ve danh sach san pham
        }
        //Tim kiem san pham theo ten(khong phan biet hoa thuong)
        public async Task<List<Product>> SearchProductsByNameAsync(string searchString)
        {
            using var context = new OnlineShoppingContext();

            return await context.Products
                .Where(p => p.Name.Contains(searchString) && p.IsDeleted == false) // Tim kiem san pham trung voi gia tri nhap vao va chua bi xoa
                .ToListAsync();
        }
        //Lay danh sach san pham theo khoang gia
        public async Task<List<Product>> GetProductsByPriceRangeAsync(double minPrice, double maxPrice)
        {
            using (var context = new OnlineShoppingContext())
            {
                return await context.Products
                    .Where(p => p.Price >= minPrice && p.Price <= maxPrice) //Loc san pham theo khoang gia minPrice va maxPrice
                    .ToListAsync(); //tra ve danh sach san pham thoa man
            }
        }
    }
}
