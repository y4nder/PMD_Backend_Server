using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;

namespace PMD_Backend.util
{
    public class LogsChecker
    {
        public string Check(Admin admin, out bool flag)
        {
            flag = false;

            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM `usertokens` WHERE `admin_FK` = @admin_FK";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@admin_FK", admin.Id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                flag = true;
                                admin.Token = (string)reader["token"];
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }

                    return Message.OK;
                }
                catch (Exception ex)
                {
                    flag = false;
                    Console.WriteLine(ex.Message);
                    return ex.Message;
                }
            }

        }

    }
}
