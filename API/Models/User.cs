namespace API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public static implicit operator UserViewModel(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Active = user.Active
            };
        }
    }
}
