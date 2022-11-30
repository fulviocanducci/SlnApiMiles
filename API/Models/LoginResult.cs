using System;
namespace API.Models
{
    public class LoginResult
    {
        public LoginResult()
        {
            Status = false;
        }

        public LoginResult(bool status, UserViewModel user)
        {
            Status = status;            
            User = user;
        }

        public LoginResult(bool status, string token, DateTime createdAt, DateTime validateAt, UserViewModel user)
        {
            Status = status;
            Token = token;
            CreatedAt = createdAt;
            ValidateAt = validateAt;            
            User = user;
        }

        public UserViewModel User { get; set; }
        public bool Status { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ValidateAt { get; set; }
        public static LoginResult Default => new LoginResult();
    }
}
