using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class RatingAndReviewRepository
    {
        private OnlineShoppingContext _context;

        public RatingAndReviewRepository()
        {
            _context = new OnlineShoppingContext();
        }

        public async Task<List<RatingAndReview>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.RatingAndReviews
                                 .Where(r => r.ProductId == productId)
                                 .ToListAsync();
        }

        public async Task<double?> GetAverageRatingByShopAsync(int shopId)
        {
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

    }
}