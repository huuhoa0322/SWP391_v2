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

            List<Cart> cart = await cartRepository.GetCartbyId(id); // get all cart id 

            List<CartViewModel> groupedShops = new List<CartViewModel>();
            List<int> shops = new List<int>();
            foreach (var item in cart)
            {

                if(shops.Contains(item.Product.SellerId))
                {
                    foreach (var shop in groupedShops)
                    {
                        if (shop.Shop.Id == item.Product.SellerId)
                        {
                            shop.Products.Add(item.Product);
                            shop.Quantity.Add(item.Quantity);
                        }
                    }
                }
                else
                {
                    groupedShops.Add(new CartViewModel(item.Product.Seller, new List<Product> { item.Product }, new List<int> { item.Quantity}));
                    shops.Add(item.Product.SellerId);
                }
                
            }

            foreach(var shop in groupedShops)
            {
                Console.WriteLine("testing: ");
                Console.WriteLine(shop.Shop.Name);
                for (int i = 0; i < shop.Products.Count; i++)
                {
                    Console.WriteLine(shop.Products[i].Name);
                    Console.WriteLine(shop.Quantity[i]);
                }
            }

            ViewData["viewcart"] = groupedShops;

            return View();
        }
    }
}
