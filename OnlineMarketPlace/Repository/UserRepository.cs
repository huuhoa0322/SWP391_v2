using Microsoft.EntityFrameworkCore;
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
        public async Task AddAsync(User user)
        {
            _context = new();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            _context = new();
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
