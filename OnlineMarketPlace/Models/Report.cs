using System;
using System.Collections.Generic;

namespace OnlineMarketPlace.Models;

public partial class Report
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public DateTime CreateAt { get; set; }

    public string Detail { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
