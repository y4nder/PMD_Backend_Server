using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string? LicensePlate { get; set; } = null!;
        public string? VehicleType { get; set; } = null!;
        public string? VehicleBrand { get; set; } = null!;
        public DateTime? ParkInDateTime { get; set; }
        public DateTime? ParkOutDateTime { get; set; }
        public string? ParkingSpace { get; set; } = null!;
        public string? OwnerFirstName { get; set; } = null!;
        public string? OwnerLastName { get; set;} = null!;

        public override string ToString()
        {
            return $"ID : {Id}\n" +
                    $"License Plate: {LicensePlate}\n" +
                    $"Vehicle Type: {VehicleType}\n" +
                    $"Vehicle Brand {VehicleBrand}\n" +
                    $"Park In Date and Time: {ParkInDateTime}\n" +
                    $"Park Out Date and Time: {ParkOutDateTime}\n" +
                    $"Floor level : {ParkingSpace}\n" +
                    $"Owner First Name : {OwnerFirstName}\n" +
                    $"Owner Last Name : {OwnerLastName}";
        }

    }
}
