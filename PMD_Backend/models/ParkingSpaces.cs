using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.models
{
    public class ParkingSpaces
    {
        public int Id { get; set; }
        public string? ParkingSpaceName {  get; set; }
        public int VehicleCount { get; set; }
        public int VehicleLimit { get; set; }
        public int Availability { get; set; }

        public override string ToString()
        {
            string parsed = $"Id : {Id} \n" +
                            $"Parking Space Name: {ParkingSpaceName}\n" +
                            $"Vehicle Count : {VehicleCount}\n" +
                            $"Vehicle Limit : {VehicleLimit}\n" +
                             "Availabilty : " + (Availability == 1 ? "true" : "false");
            return parsed;
        }
    }
}
