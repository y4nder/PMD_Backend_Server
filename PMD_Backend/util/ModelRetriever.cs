using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.util
{
    public class ModelRetriever
    {
        public string RetrieveAdmin(out Admin? admin, string username)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM admins WHERE username = @username";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                admin = new Admin
                                {
                                    Id = (int)reader["admin_PK"],
                                    Username = (string)reader["username"],
                                    Password = (string)reader["password"],
                                    Email = (string)reader["email"]
                                };
                            }
                            else
                            {
                                admin = null;
                                return Message.USER_NOT_FOUND;
                            }
                        }
                    }

                    return Message.OK;
                }
                catch (Exception ex)
                {
                    admin = null;
                    Console.WriteLine(ex.Message);
                    return ex.Message;
                }
            }

        }
    }
}
