using Newtonsoft.Json;

namespace PMD_Backend.models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int ParkedVehicleID { get; set; }
        public string? LicensePlate { get; set; } = null!;
        public int TypeID { get; set; }
        public string? VehicleType { get; set; } = null!;
        public int BrandId {get; set; }
        public string? VehicleBrand { get; set; } = null!;
        public DateTime? ParkInDateTime { get; set; }
        public DateTime? ParkOutDateTime { get; set; }
        public string? ParkingSpace { get; set; } = null!;
        public string? OwnerFirstName { get; set; } = null!;
        public string? OwnerLastName { get; set; } = null!;
        public string? CreationCode { get; set; } = null!;

        //from history logs
        public TimeSpan? Duration {  get; set; }
        public int DurationInSeconds { get; set; }
        public string? ParsedDuration {  get; set; }
        public string? Notes { get; set; }
        public double? ParkingFee { get; set; }

        public override string ToString()
        {
            return $"ID : {Id}\n" +
                    $"License Plate: {LicensePlate}\n" +
                    $"type id: {TypeID}\n" +
                    $"Vehicle Type: {VehicleType}\n" +
                    $"brand id: {BrandId}\n" +
                    $"Vehicle Brand {VehicleBrand}\n" +
                    $"Park In Date and Time: {ParkInDateTime}\n" +
                    $"Park Out Date and Time: {ParkOutDateTime}\n" +
                    $"Floor level : {ParkingSpace}\n" +
                    $"Owner First Name : {OwnerFirstName}\n" +
                    $"Owner Last Name : {OwnerLastName}";
        }

        public string PrintJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
