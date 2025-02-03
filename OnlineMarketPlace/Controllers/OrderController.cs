using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Repository;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
       // private readonly OrderRepository _orderRepository ;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
            //_orderRepository = orderRepository;
        }

        public IActionResult Order()
        {
            return View();
        }

        //public async Task<IActionResult> GetOrders(int userId)
        //{
        //    var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
        //    return View("MyOrder", orders);
        //}
    }
}


//using Microsoft.EntityFrameworkCore;
//using OnlineMarketPlace.Models;


//namespace OnlineMarketPlace.Repository
//{
//    public class OrderRepository
//    {
//        private readonly OnlineShoppingContext _context;

//        public OrderRepository(OnlineShoppingContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
//        {
//            return await _context.Orders
//                .Where(o => o.UserId == userId)
//                .Include(o => o.OrderDetails)
//                .ThenInclude(od => od.Product)
//                .ToListAsync();
//        }
//    }
//}
