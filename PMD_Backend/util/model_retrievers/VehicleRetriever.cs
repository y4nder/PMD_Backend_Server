﻿using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.util.model_retrievers
{
    public class VehicleRetriever
    {
        public static readonly int ALL = 1;
        public static readonly int PARKED = 2;
        public static readonly int BY_CREATION_CODE = 3;
        public static readonly int BY_ID = 4;

        public string Retrieve(string token, int toRetreive, string paramater, out Vehicle? vehicle)
        {
            if(toRetreive == ALL)
            {
                return Retrieve(token, toRetreive, "all_vehicles_view", "creation_code", paramater, out vehicle);
            } 
            else if(toRetreive == PARKED) 
            {
                return Retrieve(token, toRetreive, "parked_vehicles_view", "license_plate", paramater, out vehicle);
            }
            else if(toRetreive == BY_CREATION_CODE)
            {
                return Retrieve(token, toRetreive, "all_vehicles_view", "creation_code", paramater, out vehicle);
            }
            else if(toRetreive == BY_ID)
            {
                return Retrieve(token, toRetreive, "all_vehicles_view", "ID", paramater, out vehicle);
            }
            else
            {
                vehicle = null;
                return Message.INVALID_PARAMETERS;
            }
 
        }


        private string Retrieve(string token, int toRetrieve, string fromTable, string where, string equals, out Vehicle? vehicle)
        {
            string message = string.Empty;
            //verify token
            var verifyer = new TokenVerifyer(token);
            message = verifyer.Message;
            if (message != Message.OK)
            {
                vehicle = null;
                return message;
            }

            using (var sqlConnection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string query = $"SELECT * FROM {fromTable} WHERE {where} = \"{equals}\"";
                    sqlConnection.Open();
                    using (var command = new MySqlCommand(query, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                vehicle = new Vehicle
                                {
                                    Id = (int)reader["ID"],
                                    LicensePlate = (string)reader["license_plate"],
                                    VehicleType = (string)reader["type"],
                                    VehicleBrand = (string)reader["brand"],
                                    ParkInDateTime = (DateTime)reader["park_in_date_time"],
                                    ParkOutDateTime = (DateTime)reader["park_out_date_time"],
                                    ParkingSpace = (string)reader["parking_space_name"],
                                    OwnerFirstName = (string)reader["owner_first_name"],
                                    OwnerLastName = (string)reader["owner_last_name"],
                                    CreationCode = (string)reader["creation_code"]
                                };

                                if(toRetrieve == PARKED)
                                {
                                    vehicle.ParkedVehicleID = (int)reader["parking_ID"];
                                    vehicle.TypeID = (int)reader["type Id"];
                                    vehicle.BrandId = (int)reader["brand Id"];
                                }
                            }
                            else
                            {
                                vehicle = null;
                                return Message.VEHICLE_NOT_FOUND;
                            }
                        }
                    }
                    return Message.OK;
                }
                catch (Exception ex)
                {
                    vehicle = null;
                    return ex.Message;
                }
            }
        }
    }
}
