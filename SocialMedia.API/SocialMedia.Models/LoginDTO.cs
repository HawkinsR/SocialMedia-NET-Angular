namespace SocialMedia.Models
{
    public class LoginDTO
    {
        public string email { get; set; }
        public string password { get; set; }

        public LoginDTO() { }
        public LoginDTO(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
