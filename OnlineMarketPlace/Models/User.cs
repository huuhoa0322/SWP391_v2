using System;
using System.Collections.Generic;

namespace OnlineMarketPlace.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Gender { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<RatingAndReview> RatingAndReviews { get; set; } = new List<RatingAndReview>();

    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
}
