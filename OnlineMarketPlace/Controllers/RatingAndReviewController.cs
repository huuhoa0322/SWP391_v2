using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;
using OnlineMarketPlace.Repository;

namespace OnlineMarketPlace.Controllers
{
    public class RatingAndReviewController : Controller
    {
        private readonly ILogger<RatingAndReviewController> _logger;

        private readonly RatingAndReviewRepository _ratingAndReviewRepository = new();

        public RatingAndReviewController(ILogger<RatingAndReviewController> logger)
        {
            _logger = logger;
        } 

        [HttpPost]
        public async Task<IActionResult> SubmitReview(int ProductId, int Rating, string Review)
        {
            try
            {
                // Tạo và lưu đánh giá
                var ratingAndReview = new RatingAndReview
                {
                    ProductId = ProductId,
                    Rating = Rating,
                    Review = Review,
                    CreatedBy = 1, // Ví dụ
                    CreatedAt = DateTime.UtcNow
                };

                
                // Phản hồi JSON thành công
                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                // Phản hồi JSON khi lỗi
                return Json(new { success = false});
            }
        }


    }
}
