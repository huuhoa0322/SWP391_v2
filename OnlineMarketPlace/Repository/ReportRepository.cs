using Microsoft.EntityFrameworkCore;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class ReportRepository
    {
        private OnlineShoppingContext _context;

        public async Task<List<Report>> GetReportByProductIdAsync(int productId)
        {
            _context = new();
            return await _context.Reports
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }
        public async Task AddAsync(Report report)
        {
            _context = new();
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }
        public async Task<Report?> GetReportByProductAndUserAsync(int productId, int userId)
        {
            _context = new();
            return await _context.Reports
                .Where(r => r.ProductId == productId && r.UserId == userId)
                .FirstOrDefaultAsync();
        }
        
        public async Task<bool> UpdateReport(Report updateReport, string detail)
        {
            try
            {
                updateReport.Detail = detail;
                updateReport.CreateAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
