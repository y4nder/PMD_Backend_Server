using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;

namespace PMD_Backend.util.model_retrievers
{
    public class AdminRetriever
    {
        public string Retrieve(string username, out Admin? admin)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string table = "admins";
                    string query = $"SELECT * FROM {table} WHERE username = @username";
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
                                    Password = (byte[])reader["password"],
                                    Email = (string)reader["email"],
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
