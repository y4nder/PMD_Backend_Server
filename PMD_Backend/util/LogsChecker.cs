using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;

namespace PMD_Backend.util
{
    public class LogsChecker
    {
        public string Check(int id, out bool flag)
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
                        command.Parameters.AddWithValue("@admin_FK", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                flag = true;
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
