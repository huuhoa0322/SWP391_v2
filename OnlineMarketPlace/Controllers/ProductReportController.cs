using Microsoft.AspNetCore.Mvc;
using OnlineMarketPlace.Repository;
using OnlineMarketPlace.Models;
namespace OnlineMarketPlace.Controllers
{
    public class ProductReportController : Controller
    {
        private readonly ILogger<ProductReportController> _logger;
        private readonly ReportRepository _reportRepository = new();
        public ProductReportController(ILogger<ProductReportController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReport(int productId, string detail)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("Id"));
                var existReport = await _reportRepository.GetReportByProductAndUserAsync(productId, userId);
                if (existReport == null)
                {
                    var report = new Report
                    {
                        ProductId = productId,
                        Detail = detail,
                        UserId = userId,
                        CreateAt = DateTime.Now
                    };
                    await _reportRepository.AddAsync(report);
                }
                else
                {
                    await _reportRepository.UpdateReport(existReport, detail);
                }
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
    }
}
