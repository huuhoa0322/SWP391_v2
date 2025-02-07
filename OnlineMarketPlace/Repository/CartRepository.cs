using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class CartRepository
    {
        private static OnlineShoppingContext _context;


        public CartRepository()
        {
            _context = new OnlineShoppingContext();
        }

        public async Task<List<Cart>> GetCartbyId(int id)
        {
            return await _context.Carts
                .Where(c => c.UserId == id)
                .ToListAsync();
        }
    }
}
