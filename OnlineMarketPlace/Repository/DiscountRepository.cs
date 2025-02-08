using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class DiscountRepository
    {
        private static OnlineShoppingContext _context;

        public async Task<List<Discount>> GetProductDiscount()
        {
            _context = new OnlineShoppingContext();
            ProductRepository product = new();

            return await _context.Discounts.ToListAsync();
        }
    }
}
