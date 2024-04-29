using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;

namespace PMD_Backend.util
{
    public class AuditCreator
    {
        public static readonly string CREATED_AN_ENTRY = "created an entry";
        public static readonly string REMOVED_AN_ENTRY = "removed an entry";

        public string CreateAudit(string auditAction, Admin? admin, Vehicle? vehicle)
        {
            if (admin == null || vehicle == null) return Message.INVALID_PARAMETERS;

            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string table = "audits";
                    connection.Open();
                    string query = $"INSERT INTO {table}(`admin_FK`, `audit_action`, `vehicle_FK`) VALUES ({admin.Id}, \"{auditAction}\", {vehicle.Id})";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine($"rows affected: {rows} at table {table}");
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
