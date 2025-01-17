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

        public User? GetUser(string username, string password)
        {
            _context = new();
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

    }
}
