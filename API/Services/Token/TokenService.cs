using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using API.Models;
using Microsoft.IdentityModel.Tokens;
namespace API.Services.Token
{
    public class TokenService : ITokenService
    {
        private (string Token, DateTime CreatedAt, DateTime ValidateAt) GenerateToken(User user)
        {
            DateTime createdAt = DateTime.Now;
            DateTime validateAt = createdAt.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret.Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Email)
                }),
                Expires = validateAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), createdAt, validateAt);
        }

        public LoginResult GenerateToken(User user, bool status)
        {            
            var (token, createdAt, validateAt) = GenerateToken(user);
            UserViewModel userViewModel = user;
            return new LoginResult(status, token, createdAt, validateAt, userViewModel);
        }

        public LoginResult GenerateToken(LoginValidationResult validation)
        {
            return GenerateToken(validation.User, validation.Status);
        }
    }
}
