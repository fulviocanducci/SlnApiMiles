using API.DataAccess;
using API.Models;
using Canducci.GeneratePassword;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace API.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DatabaseAccess _databaseAccess;
        private readonly BCrypt _crypt;

        public LoginRepository(DatabaseAccess databaseAccess, BCrypt crypt)
        {
            _databaseAccess = databaseAccess;
            _crypt = crypt;
        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _databaseAccess
                  .Users
                  .AsNoTracking()
                  .Where(x => x.Email == email)
                  .FirstOrDefaultAsync();
            if (user != null)
            {
                user.PasswordHash = "";
                user.PasswordSalt = "";
                return user;
            }
            return null;
        }

        public async Task<LoginValidationResult> IsValidateAsync(Login login)
        {
            User user = await _databaseAccess.Users.FirstOrDefaultAsync(x => x.Email == login.Email);
            if (user != null && _crypt.Valid(login.Password, user.PasswordSalt, user.PasswordHash))
            {
                return new LoginValidationResult(user, true);
            }
            return LoginValidationResult.Default;
        }
    }
}
