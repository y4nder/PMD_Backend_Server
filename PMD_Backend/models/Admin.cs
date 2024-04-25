namespace PMD_Backend.models
{
    public class Admin
    {
        public String? Username {  get; set; }
        public String? Password { get; set; }
        public String? Email { get; set; }

        public override string ToString()
        {
            return $"username: {Username} email: {Email}";
        }

    }
}