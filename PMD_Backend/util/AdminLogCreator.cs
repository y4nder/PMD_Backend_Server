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
    public class AdminLogCreator
    {
        public string CreateLog(Admin admin, string action)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string table = "adminlogs";
                    string query = $"INSERT INTO {table}(`adminFK`, `action`) VALUES (@adminFK, @action)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@adminFK", admin.Id);
                        command.Parameters.AddWithValue("@action", action);
                        connection.Open();
                        int rows = command.ExecuteNonQuery();
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
    }
}
