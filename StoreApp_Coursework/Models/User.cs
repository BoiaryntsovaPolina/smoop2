namespace StoreApp.Models
{
    public enum UserRole { Admin, Director, Loader, HomeUser }

    public class User
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.HomeUser;
    }
}
