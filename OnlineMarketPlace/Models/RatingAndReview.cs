using System;
using System.Collections.Generic;

namespace OnlineMarketPlace.Models;

public partial class RatingAndReview
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Rating { get; set; }

    public string Review { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

}
