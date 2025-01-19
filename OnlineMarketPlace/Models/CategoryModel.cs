using System;
using System.Collections.Generic;

namespace OnlineMarketPlace.Models;

public  class CategoryModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentId { get; set; }

    public virtual ICollection<CategoryModel> InverseParent { get; set; } = new List<CategoryModel>();

    public virtual CategoryModel? Parent { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public required string Slug { get; set; }
}
