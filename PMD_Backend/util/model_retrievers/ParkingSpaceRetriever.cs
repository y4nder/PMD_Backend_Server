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
    public class ParkingSpaceRetriever
    {
        public static readonly int ALL = 1;
        public static readonly int AVAILABLE = 2;

        public string Retrieve(string token, int toRetrieve, out ICollection<ParkingSpaces>? allParkingSpaces)
        {
            string message = "";

            //verify parameters
            if(toRetrieve > 2 || toRetrieve <= 0) {
                allParkingSpaces = null;
                return Message.INVALID_PARAMETERS;
            }

            //verify token
            var verifyer = new TokenVerifyer(token);
            message = verifyer.Message;
            if (message != Message.OK)
            {
                allParkingSpaces = null;
                return message;
            }

            message = RetrieveAllParkingSpaces(out allParkingSpaces, toRetrieve);
            if (message != Message.OK)
            {
                allParkingSpaces = null;
                return message;

            }
            return message;
        }

        private string RetrieveAllParkingSpaces(out ICollection<ParkingSpaces> allParkingSpaces, int toRetrieve)
        {
            allParkingSpaces = new List<ParkingSpaces>();
            using (var sqlConnection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    sqlConnection.Open();
                    string table = "parkingspaces";

                    string query = "";
                    //determine if only available or all parking spaces are to be returned
                    if (toRetrieve == ALL)
                    {
                        query = $"SELECT * FROM {table}";
                    }

                    if (toRetrieve == AVAILABLE)
                    {
                        query = $"SELECT * FROM {table} WHERE availability = 1";
                    }

                    using (var command = new MySqlCommand(query, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var parkingSpace = new ParkingSpaces
                                {
                                    Id = (int)reader["parkingSpacePK"],
                                    ParkingSpaceName = (string)reader["parkingSpaceName"],
                                    VehicleCount = (int)reader["vehicle_count"],
                                    VehicleLimit = (int)reader["vehicle_limit"],
                                    Availability = (int)reader["availability"]
                                };
                                allParkingSpaces.Add(parkingSpace);
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
    }
}
