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
    }
}
