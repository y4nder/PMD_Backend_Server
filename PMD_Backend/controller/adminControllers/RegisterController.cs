using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using PMD_Backend.util;
namespace PMD_Backend.controller.adminControllers
{
    internal class RegisterController : Message
    {
        private RegisterFormData registerFormData;

        public RegisterController(RegisterFormData registerFormData)
        {
            this.registerFormData = registerFormData;

        }
        public string Register()
        {
            string message = string.Empty;

            //create security keys
            SecurityKey securityKeys = new Security().CreateSecurityKey();

            //encrypt password
            byte[] encryptedPassword = new Security().Encrypt(registerFormData.Password, securityKeys.Key, securityKeys.Iv);
            
            //upload to database
            message = SaveRegistration(encryptedPassword);

            //get primary key of registerd user
            message = RetrieveRegisteredData(registerFormData.Username, out Admin? admin);

            //upload to security keys to database
            if (admin != null) 
                message = SaveKeys(admin.Id, securityKeys);
            

            return message;
        }

        private string SaveKeys(int? id, SecurityKey securityKeys)
        {
            using(var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string table = "security_keys";
                    string query = $"INSERT INTO {table}(`admin_FK`, `security_key`, `security_iv`) VALUES (@id, @key, @iv)";
                    using (var mySqlCommand = new MySqlCommand(query, connection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@id", id);
                        mySqlCommand.Parameters.AddWithValue("@key", securityKeys.Key);
                        mySqlCommand.Parameters.AddWithValue("@iv", securityKeys.Iv);
                        connection.Open();
                        int rows = mySqlCommand.ExecuteNonQuery();
                        Console.WriteLine("rows affected: " + rows + " at table " + table);
                    }
                    return Message.OK;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        private string RetrieveRegisteredData(string username, out Admin? admin)
        {
            admin = null;
            using (var connection = new  MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT * FROM admins WHERE username = \"{username}\"";
                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                    {
                        using(var reader = mySqlCommand.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                admin = new Admin
                                {
                                    Id = (int)reader["admin_PK"],
                                    Username = (string)reader["username"],
                                    Password = (byte[])reader["password"],
                                    Email = (string)reader["email"]
                                };
                            } 
                            else
                            {
                                return Message.USER_NOT_FOUND;
                            }
                        }
                        
                    }

                    return Message.OK;
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        private string SaveRegistration(byte[] encryptedPassword)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string table = "admins";
                    string query = $"INSERT INTO {table}(`username`, `password`, `email`) VALUES (@username,@password,@email)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", registerFormData.Username);
                        command.Parameters.AddWithValue("@password", encryptedPassword);
                        command.Parameters.AddWithValue("@email", registerFormData.Email);
                        connection.Open();
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine("rows affected: " + rows + " at table " + table);
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
                    parsed = "The username \"" + registerFormData.Username + "\"";
                }
                else if (message.Contains("email"))
                {
                    parsed = "The email \"" + registerFormData.Email + "\"";
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
