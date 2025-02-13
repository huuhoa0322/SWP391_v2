using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineMarketPlace.Models;
namespace OnlineMarketPlace.Repository
{
    public class UserRepository
    {
        private OnlineShoppingContext _context;


        public List<User> GetUsers()
        {
            _context = new();
            return _context.Users.ToList();

        }

        public async Task<User?> GetUser(string username, string password)
        {
            _context = new();
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == ncryptpasswordmd5.HashPasswordMD5(password) && u.IsDeleted == false);
        }
        

        public async Task<User?> GetUserByEmail(string email)
        {
            _context = new();
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }

        //public async Task<User?> GetUserByEmailAndToken(string email, string token)
        //{
        //    _context = new();
        //    return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false && u.Token == token);
        //}

        public async Task<User?> GetUserById(int id)
        {
            _context = new();
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id );
        }
        //add user
        public async Task AddAsync(User user)
        {
            _context = new();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        //check username
        public async Task<User?> checkUserName(string username)
        {
            _context = new();
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        //checkEmail
        public async Task<User?> checkEmail(string email)
        {
            _context = new();
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }
        
        //Update user
        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            _context = new();

            var existingUser = await _context.Users.FindAsync(updatedUser.Id);

            if (existingUser == null)
            {
                return false; 
            }

            existingUser.Name = updatedUser.Name ?? existingUser.Name;
            existingUser.Username = updatedUser.Username ?? existingUser.Username;
            existingUser.Password = updatedUser.Password ?? existingUser.Password;
            existingUser.Email = updatedUser.Email ?? existingUser.Email;
            existingUser.Gender = updatedUser.Gender;
            existingUser.Dob = updatedUser.Dob;
            existingUser.Role = updatedUser.Role ?? existingUser.Role;
            existingUser.IsDeleted = updatedUser.IsDeleted;
            existingUser.DeletedBy = updatedUser.DeletedBy ?? existingUser.DeletedBy;
            existingUser.DeletedAt = updatedUser.DeletedAt ?? existingUser.DeletedAt;
            //existingUser.Token = updatedUser.Token ?? existingUser.Token;

            try
            {
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (DbUpdateException)
            {
                return false; 
            }
        }

        public async Task<List<OrderViewModel>> GetOrdersByUserIdAsync(int userId)
        {
            _context = new();
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Seller)
                .ToListAsync();

            var orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                // Nhóm các sản phẩm theo Shop
                var groupedByShop = order.OrderDetails
                    .GroupBy(od => od.Product.Seller.Name)  // Nhóm theo tên Shop
                    .Select(g => new ShopGroup
                    {
                        ShopId = g.First().Product.Seller.Id,
                        ShopName = g.Key,
                        TotalAmount = g.Sum(od => od.Quantity * od.Product.Price), // Tính tổng tiền cho Shop
                        Products = g.Select(od => new ProductDetails
                        {
                            ProductName = od.Product.Name,
                            Quantity = od.Quantity,
                            Price = od.Product.Price,
                            TotalProductAmount = od.Quantity * od.Product.Price // Tính tổng tiền cho sản phẩm
                        }).ToList()
                    }).ToList();

                orderViewModels.Add(new OrderViewModel
                {
                    OrderId = order.Id,
                    TotalAmount = order.Total, // Tổng tiền của đơn hàng
                    OrderDate = order.CreateAt,
                    Status = order.Status,
                    PaymentMethod = order.PaymentMethod,
                    ShopGroups = groupedByShop
                });
            }

            return orderViewModels;
        }


    }
}
