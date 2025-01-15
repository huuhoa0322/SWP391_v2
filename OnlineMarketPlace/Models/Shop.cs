using System;
using System.Collections.Generic;

namespace OnlineMarketPlace.Models;

public partial class Shop
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly CreatedAt { get; set; }

    public string Logo { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
