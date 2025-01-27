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


    }
}
