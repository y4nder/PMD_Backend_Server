using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
namespace PMD_Backend.controller.adminControllers
{
    internal class RegisterController : Message
    {
        private Admin admin;

        public RegisterController(Admin admin)
        {
            this.admin = admin;
        }
        public string Register()
        {
            //create connection
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string query = "INSERT INTO `admins`(`username`, `password`, `email`) VALUES (@username,@password,@email)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", admin.Username);
                        command.Parameters.AddWithValue("@password", admin.Password);
                        command.Parameters.AddWithValue("@email", admin.Email);
                        connection.Open();
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine("rows affected: " + rows);
                    }
                    return Message.OK;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Parse(ex.Message);
                }

            }
        }

        public string Parse(string message)
        {
            //Duplicate entry 'john' for key 'username'
            string parsed = "";

            if (message.Contains("Duplicate"))
            {
                if (message.Contains("username"))
                {
                    parsed = "The username \"" + admin.Username + "\"";
                }
                else if (message.Contains("email"))
                {
                    parsed = "The email \"" + admin.Email + "\"";
                }
                parsed += " is already used";
            }
            else if (message.Contains("null"))
            {
                if (message.Contains("username"))
                {
                    parsed = "username";
                }
                else if (message.Contains("email"))
                {
                    parsed = "email";
                }
                else
                {
                    parsed = "password";
                }
                parsed += " is empty";
            }

            return parsed;
        }

    }
}
