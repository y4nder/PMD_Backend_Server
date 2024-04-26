namespace PMD_Backend.models
{
    public class Admin
    {
        public int Id { get; set; }

        public String Username { get; set; } = null!;
        public String? Password { get; set; } = null!;
        public String? Email { get; set; } = null!;
        public String Token { get; set; } = null!;

        public override string ToString()
        {
            string parsed = "";
            parsed += "username : " + Username + "\n";
            parsed += "password : " + Password + "\n";
            parsed += "email : " + Email + "\n";
            parsed += "token : " + Token + "\n";
            return parsed;
        }

    }
}