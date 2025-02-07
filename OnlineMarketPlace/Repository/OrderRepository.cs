using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;
namespace OnlineMarketPlace.Repository
{
    public class OrderRepository 
    {
        private OnlineShoppingContext _context;

        public OrderRepository(OnlineShoppingContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();
        }
    }
}
