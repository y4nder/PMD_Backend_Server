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
                                    Password = (string)reader["password"],
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
                                    Password = (string)reader["password"],
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
    }
}
