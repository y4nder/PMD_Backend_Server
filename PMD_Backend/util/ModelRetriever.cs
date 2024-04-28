using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using PMD_Backend.util.model_retrievers;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace PMD_Backend.util
{
    public class ModelRetriever
    {

        //retrieve admin details
        public string RetrieveAdmin(out Admin? admin, string username)
        {
            return new AdminRetriever().Retrieve(username, out admin);
        }

        //retrieve all vehicle types
        public string RetrieveAllVehicleTypes(string token, out ICollection<VehicleType>? allVehicleTypes)
        {
            return new VehicleTypesRetriever().Retrieve(token, out allVehicleTypes);
        }

        public string RetrieveAllBrands(string vehicleName, string token, out ICollection<Brand>? allBrands)
        {
            return new BrandsRetriever().Retrieve(token, vehicleName, out allBrands);
        }

        public string RetrieveAllParkingSpaces(string token, int toRetrieve, out ICollection<ParkingSpaces>? allParkingSpaces)
        {
            return new ParkingSpaceRetriever().Retrieve(token, toRetrieve, out allParkingSpaces);
        }

        //retrieve vehicle using license plate
        public string RetrieveVehicle(string token, int toRetrieve, string parameter, out Vehicle? vehicle)
        {
            return new VehicleRetriever().Retrieve(token, toRetrieve, parameter, out vehicle);
        }

        


    }
}
