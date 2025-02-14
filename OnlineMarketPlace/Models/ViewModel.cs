namespace OnlineMarketPlace.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public List<ShopGroup> ShopGroups { get; set; }
    }

    public class ShopGroup
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public double TotalAmount { get; set; } // Tổng tiền của tất cả sản phẩm trong shop
        public List<ProductDetails> Products { get; set; }
    }

    public class ProductDetails
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalProductAmount { get; set; } // Tổng tiền cho từng sản phẩm (Quantity * Price)
    }

}
