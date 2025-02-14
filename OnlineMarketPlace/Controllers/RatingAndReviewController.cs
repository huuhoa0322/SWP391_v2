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
        public async Task<IActionResult> SubmitReview(int productId, int rating, string review)
        {
            try {
                Console.WriteLine("-----------------");
                Console.WriteLine("kkkk");

                int userId = int.Parse(HttpContext.Session.GetString("Id"));
                Console.WriteLine(HttpContext.Session.GetString("Id"));
                Console.WriteLine(userId);

                // Tạo và lưu đánh giá
                
                var existRating = await _ratingAndReviewRepository.GetRatingByProductAndUserAsync(productId, userId);
                if (existRating == null)
                {
                    Console.WriteLine("hehehe");
                    var ratingAndReview = new RatingAndReview
                    {
                        ProductId = productId,
                        Rating = rating,
                        Review = review,
                        CreatedBy = userId,
                        CreatedAt = DateTime.Now
                    };
                    await _ratingAndReviewRepository.AddAsync(ratingAndReview);
                }
                else
                {
                    await _ratingAndReviewRepository.UpdateRatingAndReview(existRating, rating, review);
                }



                // Phản hồi JSON thành công
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine("loi");
                // Phản hồi JSON khi lỗi
                return Json(new { success = false });
            }


        }


    }
}
