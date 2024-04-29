using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.util.model_retrievers
{
    public class VehicleTypesRetriever
    {
        public string Retrieve(string token, out ICollection<VehicleType>? allVehicleTypes)
        {
            string message = "";

            //verify token
            var verifyer = new TokenVerifyer(token);
            message = verifyer.Message;
            if (message != Message.OK)
            {
                allVehicleTypes = null;
                return message;
            }

            //begin retrieval
            allVehicleTypes = new HashSet<VehicleType>();
            using (var sqlConnection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    sqlConnection.Open();
                    string table = "vehicle_types";
                    string query = $"SELECT * FROM {table}";
                    using (var command = new MySqlCommand(query, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var vehicleType = new VehicleType
                                {
                                    Id = (int)reader["vehicle_type_PK"],
                                    Name = (string)reader["vehicle_type_name"],
                                    AdditionalFee = (double)reader["additional_fee"],
                                    Flagdown = (double)reader["flagdown"],
                                };

                                allVehicleTypes.Add(vehicleType);
                            }
                        }
                    }
                    return Message.OK;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
        }

        public string Retrieve(string token, int id, out VehicleType? vehicleType)
        {
            string message = "";

            //verify token
            var verifyer = new TokenVerifyer(token);
            message = verifyer.Message;
            if (message != Message.OK)
            {
                vehicleType = null;
                return message;
            }

            using(var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT * FROM vehicle_types WHERE vehicle_type_PK = {id}";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using(var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                vehicleType = new VehicleType
                                {
                                    Id = (int)reader["vehicle_type_PK"],
                                    Name = (string)reader["name"],
                                    Flagdown = (double)reader["flagdown"],
                                    AdditionalFee = (double)reader["additional_fee"]
                                };
                            }
                            else
                            {
                                vehicleType = null;
                                return Message.VEHICLE_TYPE_NOT_FOUND;
                            }
                        }
                    }
                    return Message.OK;
                }
                catch(Exception ex)
                {
                    vehicleType = null;
                    return ex.Message;
                }

            }
        }
    }
}
