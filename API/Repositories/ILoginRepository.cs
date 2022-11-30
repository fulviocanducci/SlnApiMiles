using API.Models;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface ILoginRepository
    {
        Task<LoginValidationResult> IsValidateAsync(Login login);
        Task<User> GetUserAsync(string email);
    }
}