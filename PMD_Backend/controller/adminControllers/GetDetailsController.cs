using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.controller.adminControllers
{
    public class GetDetailsController
    {
        private string token;
        public GetDetailsController(string token)
        {
            this.token = token;
        }

        public string GetDetails(out Admin? admin)
        {
            admin = null;
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string table = "admin_details_view";
                    string query = $"SELECT * FROM {table} WHERE token = @token";
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@token", token);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                admin = new Admin
                                {
                                    Id = (int)reader["admin_PK"],
                                    Username = (string)reader["username"],
                                    Password = (string)reader["password"],
                                    Email = (string)reader["email"],
                                    Token = (string)reader["token"]
                                };
                            }
                            else
                            {
                                admin = null;
                                return Message.UNAUTHORIZED;
                            }
                        }
                    }
                    return Message.OK;
                }
                catch (Exception ex)
                {
                    admin = null;
                    return ex.Message;
                }
            };

        }
    }
}
