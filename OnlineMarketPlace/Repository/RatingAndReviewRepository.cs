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

        public async Task<RatingAndReview?> GetRatingByProductAndUserAsync(int productId, int userId)
        {
            _context = new();
            return await _context.RatingAndReviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.CreatedBy == userId);
        }

        public async Task<bool> UpdateRatingAndReview(RatingAndReview updateRating, int rating, string review)
        {
            try
            {

                // Cập nhật thông tin
                updateRating.Rating = rating;
                updateRating.Review = review;
                updateRating.CreatedAt = DateTime.UtcNow;

                // Lưu thay đổi
                await _context.SaveChangesAsync();
                return true; // Thành công
            }
            catch (DbUpdateException ex)
            {
                // Ghi log lỗi chi tiết
                Console.WriteLine($"Lỗi khi cập nhật bản ghi: {ex.Message}");
                return false; // Thất bại
            }
        }


    }
}