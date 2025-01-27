using System;
using System.Collections.Generic;

namespace OnlineMarketPlace.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public double Total { get; set; }

    public DateTime CreateAt { get; set; }

    public string Status { get; set; } = null!;

    public int DirectId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public virtual Direct Direct { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User User { get; set; } = null!;
}
