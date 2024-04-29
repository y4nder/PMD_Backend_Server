using Google.Protobuf.WellKnownTypes;
using PMD_Backend.interfaces;
using PMD_Backend.models;

namespace PMD_Backend.util
{
    public class PaymentCalculator
    {
        public string Calculate(Vehicle vehicle, VehicleType vehicleType)
        {
            //calculate timespan using datetime now - parkin time, then store to duration property
            vehicle.Duration = DateTime.Now - vehicle.ParkInDateTime;
            if (vehicle.Duration.HasValue)
            {
                vehicle.ParsedDuration = $"{vehicle.Duration.Value.Days}days, {vehicle.Duration.Value.Hours}hours, {vehicle.Duration.Value.Minutes}minutes and {vehicle.Duration.Value.Seconds}seconds";
                vehicle.DurationInSeconds = (int)vehicle.Duration.Value.TotalSeconds;
            }


            if (vehicle.Duration == null) return Message.INCOMPLETE_FIELDS;

            //get total hours from calculated timespan
            double totalHours = vehicle.Duration.Value.TotalHours;
            double tempFee = vehicleType.Flagdown;
            tempFee += totalHours * vehicleType.AdditionalFee;

            //add temp parking fee to vehicle property parking fee
            vehicle.ParkingFee = tempFee;

            return Message.OK;
        }
    }
}
