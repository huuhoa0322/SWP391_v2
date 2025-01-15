﻿using System;
using System.Collections.Generic;

namespace OnlineMarketPlace.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public string Description { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int SellerId { get; set; }

    public int CategoryId { get; set; }

    public int QuantitySold { get; set; }

    public int Inventory { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<RatingAndReview> RatingAndReviews { get; set; } = new List<RatingAndReview>();

    public virtual Shop Seller { get; set; } = null!;
}
