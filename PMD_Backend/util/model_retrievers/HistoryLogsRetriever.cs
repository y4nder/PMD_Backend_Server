using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.util.model_retrievers
{
    public class HistoryLogsRetriever
    {
        public string Retrieve(string token, out ICollection<HistoryLog>? allLogs)
        {
            string message = string.Empty;

            //verify token 
            if(token == null)
            {
                allLogs = null;
                return Message.UNAUTHORIZED;
            }

            var verifier = new TokenVerifyer(token);
            if (verifier.Message != Message.OK)
            {
                allLogs = null;
                return verifier.Message;
            }

            using(var connection  = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    allLogs = new List<HistoryLog>();
                    connection.Open();
                    string table = "history_view";
                    string query = $"SELECT * FROM {table}";
                    using(var command = new MySqlCommand(query, connection))
                    {
                        using(var reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                var historyLog = new HistoryLog
                                {
                                    HistoryPK = (int)reader["history_PK"],
                                    Duration = TimeSpan.FromSeconds((int)reader["duration"]),
                                    ParsedDuration = (string)reader["parsed_duration"],
                                    TotalAmount = (double)reader["totalAmount"],
                                    TransactionDateTime = (DateTime)reader["transactionDateTime"],
                                    IssuerID = (int)reader["IssuerID"],
                                    IssuerEmail = (string)reader["IssuerEmail"],
                                    IssuerName = (string)reader["IssuerName"],
                                    Notes = (string)reader["Notes"],
                                    VehiclePK = (int)reader["vehiclePK"],
                                    LicensePlate = (string)reader["licensePlate"],
                                    FloorLevel = (string)reader["floor_level"],
                                    ParkInDateTime = (DateTime)reader["parkInDateTime"],
                                    ParkOutDateTime = (DateTime)reader["parkOutDateTime"],
                                    OwnerFirstName = (string)reader["ownerFirstName"],
                                    OwnerLastName = (string)reader["ownerLastName"],
                                    CreationCode = (string)reader["creation_code"],
                                    VehicleTypeName = (string)reader["vehicle_type_name"],
                                    BrandName = (string)reader["brand_name"]
                                };
                                allLogs.Add(historyLog);                                
                            }
                        }
                    }


                    return Message.OK;
                }
                catch(Exception ex)
                {
                    allLogs = null;
                    return ex.Message;
                }
            }
        }
    }
}
