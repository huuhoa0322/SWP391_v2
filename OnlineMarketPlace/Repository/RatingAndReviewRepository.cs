using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class RatingAndReviewRepository
    {
        private OnlineShoppingContext _context;


        public async Task<List<RatingAndReview>> GetReviewsByProductIdAsync(int productId)
        {
            _context = new();
            return await _context.RatingAndReviews
                                 .Where(r => r.ProductId == productId)
                                 .ToListAsync();
        }

        public async Task<double?> GetAverageRatingByShopAsync(int shopId)
        {
            _context = new();
            var ratings = await _context.RatingAndReviews
                .Where(r => r.Product.SellerId == shopId)
                .Select(r => r.Rating)
                .ToListAsync();

            if (!ratings.Any())
            {
                return null;
            }

            return ratings.Average();
        }

        //add
        public async Task AddAsync(RatingAndReview review)
        {
            _context = new();
            await _context.RatingAndReviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

    }
}