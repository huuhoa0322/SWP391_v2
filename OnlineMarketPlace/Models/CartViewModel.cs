namespace OnlineMarketPlace.Models
{
    public class CartViewModel
    {
        public Shop Shop { get; set; }
        public List<Product> Products { get; set; }
        public List<int> Quantity { get; set; }

        public CartViewModel(Shop shop, List<Product> products, List<int> quantity)
        {
            Shop = shop;
            Products = products;
            Quantity = quantity;
        }
    }
}
