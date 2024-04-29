using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;

namespace PMD_Backend.util
{
    public class VehicleRemover
    { 
        public string Remove(string? licensePlate)
        {
            string message = string.Empty;

            if (licensePlate == null) return Message.VEHICLE_NOT_FOUND;

            using(var connection = new MySqlConnection(Environment.GetEnvironmentVariable("Connectionstring")))
            {
                try
                {
                    connection.Open();
                    string table = "parkedvehicles";
                    string query = $"DELETE FROM {table} WHERE license_plate = \"{licensePlate}\"";
                    using(var command = new MySqlCommand(query, connection))
                    {
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine($"rows affected {rows} at table {table}");
                    }

                    return Message.OK;
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
