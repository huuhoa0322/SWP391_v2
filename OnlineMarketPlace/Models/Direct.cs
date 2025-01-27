using System;
using System.Collections.Generic;

namespace OnlineMarketPlace.Models;

public partial class Direct
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Directer { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
