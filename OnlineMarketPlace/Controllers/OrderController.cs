using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository = new();
        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
            
        }

        public IActionResult Order()
        {
            return View();
        }
        
        public async Task<IActionResult> GetOrders()
        {
            int userId = 1;
            var orders = await _userRepository.GetOrdersByUserIdAsync(userId) ; 
            return View("Order", orders);
        }

    }
}
