using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace PMD_Backend.util.model_retrievers
{
    public class BrandsRetriever
    {
        public string Retrieve(string token, string vehicleName, out ICollection<Brand>? allBrands)
        {
            string message = "";
            //check if token exists
            message = new TokenVerifyer(token).Message;
            if (message != Message.OK)
            {
                allBrands = null;
                return message;
            }

            allBrands = new HashSet<Brand>();
            using (var sqlConnection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    sqlConnection.Open();
                    string storedProdureName = "GetBrandsByVehicleType";
                    using (var command = new MySqlCommand(storedProdureName, sqlConnection))
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
                                allBrands.Add(brand);
                            }
                        }

                    }

                    return Message.OK;
                }
                catch (Exception ex)
                {
                    allBrands = null;
                    return ex.Message;
                }
            }
        }
    }
}
