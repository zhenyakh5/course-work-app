using System.Linq;

namespace BookingService.MVVM.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public bool Register(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                return false;
            }

            var newUser = new User
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                IsAdmin = false
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return true;
        }
    }
}