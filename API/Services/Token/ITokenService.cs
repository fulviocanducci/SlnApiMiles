using API.Models;
namespace API.Services.Token
{
    public interface ITokenService
    {
        LoginResult GenerateToken(User user, bool status);
        LoginResult GenerateToken(LoginValidationResult validation);
    }
}
