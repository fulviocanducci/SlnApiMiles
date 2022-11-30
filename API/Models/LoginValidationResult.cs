namespace API.Models
{
    public class LoginValidationResult
    {
        public LoginValidationResult()
        {
            User = null;
            Status = false;
        }

        public LoginValidationResult(User user, bool status)
        {
            User = user;
            Status = status;
        }

        public User User { get; set; }
        public bool Status { get; set; }

        public static LoginValidationResult Default => new LoginValidationResult();
    }
}
