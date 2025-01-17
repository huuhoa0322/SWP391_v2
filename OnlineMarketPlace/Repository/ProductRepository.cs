using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class ProductRepository
    {
        private OnlineShoppingContext _context;


        public List<Product> GetProducts()
        {
            _context = new();
            return _context.Products.ToList();

        }
    }
}
