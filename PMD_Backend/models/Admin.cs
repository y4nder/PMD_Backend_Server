using Newtonsoft.Json;

namespace PMD_Backend.models
{
    public class Admin
    {
        public int Id { get; set; }

        public String Username { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public String? Email { get; set; } = null!;
        public String Token { get; set; } = null!;

        public override string ToString()
        {
            string parsed = "";
            parsed += "username : " + Username + "\n";
            parsed += "password : " + Convert.ToBase64String(Password) + "\n";
            parsed += "email : " + Email + "\n";
            parsed += "token : " + Token + "\n";
            return parsed;
        }

        public string PrintAsJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}