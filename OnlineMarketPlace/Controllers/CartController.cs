using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class CartController : Controller
    {
        public async Task<IActionResult> Cart()
        {
            CartRepository cartRepository = new();
            int id = int.Parse(HttpContext.Session.GetString("Id"));
            List<Cart> cart = await cartRepository.GetCartbyId(id);
            ViewData["Cart"] = cart;
            Console.WriteLine(id);
            Console.WriteLine(cart[0].Product.Name);
            
            return View();
        }
    }
}
