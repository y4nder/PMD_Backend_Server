using MySql.Data.MySqlClient;
using PMD_Backend.controller.adminControllers;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.util
{
    public class ModelRetriever
    {
        public string RetrieveAdminWithToken(out Admin? admin, string username)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string table = "admin_details_view";
                    string query = $"SELECT * FROM {table} WHERE username = @username";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                admin = new Admin
                                {
                                    Id = (int)reader["admin_PK"],
                                    Username = (string)reader["username"],
                                    Password = (byte[])reader["password"],
                                    Email = (string)reader["email"],
                                    Token = (string)reader["token"],
                                };
                            }
                            else
                            {
                                admin = null;
                                return Message.USER_NOT_FOUND;
                            }
                        }
                    }

                    return Message.OK;
                }
                catch (Exception ex)
                {
                    admin = null;
                    Console.WriteLine(ex.Message);
                    return ex.Message;
                }
            }

        }

        public string RetrieveAdmin(out Admin? admin, string username)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string table = "admins";
                    string query = $"SELECT * FROM {table} WHERE username = @username";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                admin = new Admin
                                {
                                    Id = (int)reader["admin_PK"],
                                    Username = (string)reader["username"],
                                    Password = (byte[])reader["password"],
                                    Email = (string)reader["email"],
                                };
                            }
                            else
                            {
                                admin = null;
                                return Message.USER_NOT_FOUND;
                            }
                        }
                    }

                    return Message.OK;
                }
                catch (Exception ex)
                {
                    admin = null;
                    Console.WriteLine(ex.Message);
                    return ex.Message;
                }
            }

        }

        //retrieve all vehicle types
        public string GetAllVehicleTypes(string token, out ICollection<VehicleType>? allVehicleTypes)
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

            //populate hashset list
            message = RetrieveAllTypes(out allVehicleTypes);

            return message;
        }

        private string RetrieveAllTypes(out ICollection<VehicleType> allVehicleTypes)
        {
            allVehicleTypes = new HashSet<VehicleType>();
            using(var sqlConnection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    sqlConnection.Open();
                    string table = "vehicle_types";
                    string query = $"SELECT * FROM {table}";
                    using(var command =  new MySqlCommand(query, sqlConnection))
                    {
                        using(var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var vehicleType = new VehicleType
                                {
                                    Id = (int)reader["vehicle_type_PK"],
                                    Name = (string)reader["name"],
                                    AdditionalFee = (double)reader["additional_fee"],
                                    Flagdown = (double)reader["flagdown"],
                                };

                                allVehicleTypes.Add(vehicleType);
                            }
                        }
                    }
                    return Message.OK;
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }

            }
        }

        public string RetrieveAllBrands(string vehicleName, string token, out ICollection<Brand>? allBrands)
        {
            string message = "";
            //check if token exists
            message = new TokenVerifyer(token).Message;
            if(message != Message.OK)
            {
                allBrands = null;
                return message;
            }

            //populate list with all brands
            message = RetrieveBrandOf(vehicleName, out allBrands);
            if(message != Message.OK)
            {
                allBrands = null;
                return message;
            }

            return message;
        }

        private string RetrieveBrandOf(string vehicleName, out ICollection<Brand>? brands)
        {
            brands = new HashSet<Brand>();
            using(var sqlConnection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    sqlConnection.Open();
                    string storedProdureName = "GetBrandsByVehicleType";
                    using(var command = new MySqlCommand(storedProdureName, sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@vehicle_type_name", vehicleName);
                        using (var reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return Message.VEHICLE_TYPE_NOT_FOUND + ": " + vehicleName;
                            }
                            while (reader.Read())
                            {
                                var brand = new Brand
                                {
                                    Id = (int)reader["brand_id"],
                                    Name = (string)reader["vehicle_brand"]
                                };
                                brands.Add(brand);
                            }
                        }

                    }

                    return Message.OK;
                }
                catch (Exception ex)
                {
                    brands = null;
                    return ex.Message;
                }
            }
        }

        //get all parking spaces
        public string RetrieveAllParkingSpaces(string token, out ICollection<ParkingSpaces>? allParkingSpaces)
        {
            string message = "";
            //verify token
            var verifyer = new TokenVerifyer(token);
            message = verifyer.Message;
            if(message != Message.OK)
            {
                allParkingSpaces = null;
                return message;
            }

            //get all parking spaces
            message = RetrieveAllParkingSpaces(out allParkingSpaces, ALL_PARKING_SPACES);
            if(message != Message.OK)
            {
                allParkingSpaces = null;
                return message;

            }
            return message;
        }

        //get all available parkingSpaces
        public string RetrieveAllAvailableParkingSpaces(string token, out ICollection<ParkingSpaces>? allParkingSpaces)
        {
            string message = "";
            //verify token
            var verifyer = new TokenVerifyer(token);
            message = verifyer.Message;
            if (message != Message.OK)
            {
                allParkingSpaces = null;
                return message;
            }

            //get all parking spaces
            message = RetrieveAllParkingSpaces(out allParkingSpaces, ALL_AVAILABLE_PARKING_SPACES);
            if (message != Message.OK)
            {
                allParkingSpaces = null;
                return message;

            }
            return message;
        }


        private static readonly int ALL_PARKING_SPACES = 1;
        private static readonly int ALL_AVAILABLE_PARKING_SPACES = 2;

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
                    if(toRetrieve == ALL_PARKING_SPACES)
                    {
                        query = $"SELECT * FROM {table}";
                    }

                    if(toRetrieve == ALL_AVAILABLE_PARKING_SPACES)
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
