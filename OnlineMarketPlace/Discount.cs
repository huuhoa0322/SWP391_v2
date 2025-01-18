using System;
using System.Collections.Generic;

namespace OnlineMarketPlace;

public partial class Discount
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Value { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Product Product { get; set; } = null!;
}
