using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;

namespace PMD_Backend.util
{
    public class HistoryLogsCreator
    {
        public string CreateLog(Vehicle? vehicle, Admin? admin, string? note)
        {
            if (vehicle == null || admin == null) return Message.INVALID_PARAMETERS;

            using(var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string table = "history";
                    string query = $"INSERT INTO {table}(`duration`, `parsed_duration`, `vehicle_FK`, `totalAmount`, `IssuerID`, `Notes`) " +
                        $"VALUES ({vehicle.DurationInSeconds}, \"{vehicle.ParsedDuration}\", {vehicle.Id}, {vehicle.ParkingFee}, {admin.Id}, \"{note}\")";
                    using(var command = new MySqlCommand(query, connection))
                    {
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine($"rows affected {rows} at table {table}");
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
