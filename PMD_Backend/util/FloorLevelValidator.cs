using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.util
{
    public class FloorLevelValidator
    {
        private string? floorLevel = null!;
        public FloorLevelValidator(string floorLevel) 
        {
            this.floorLevel = floorLevel;
        }

        public string Validate()
        {
            using(var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                { 
                    connection.Open();
                    string query = $"SELECT * FROM `parkingspaces` WHERE `parkingSpaceName` = \"{floorLevel}\"";
                    using(var commmand = new  MySqlCommand(query, connection))
                    {
                        using(var reader = commmand.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                return Message.OK;
                            }
                            else
                            {
                                return Message.FLOOR_DOES_NOT_EXIST;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
        }
    }
}
