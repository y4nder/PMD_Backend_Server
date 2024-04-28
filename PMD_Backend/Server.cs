using PMD_Backend.controller.adminControllers;
using PMD_Backend.controller.CreateEntryControllers;
using PMD_Backend.models;
using PMD_Backend.util;
using PMD_Backend.util.model_retrievers;
using System.Net.WebSockets;

namespace PMD_Backend
{
    public class Server
    {
        //registering an admin
        public string RegisterAdmin(RegisterFormData registerFormData)
        {
            return new RegisterController(registerFormData).Register();
        }

        //log in for admins
        public string LogInAdmin(LoginFormData loginFormData, out Admin? admin)
        {
            return new LogInController(loginFormData).LogIn(out admin);
        }

        //log out for admins
        public string LogOutAdmin(string token)
        {
            return new LogOutController(token).LogOut();
        }

        //retrieve admin details
        public string GetAdminDetails(string token, out Admin? admin)
        {
            return new GetDetailsController(token).GetDetails(out admin);
        }

        //retrive all Vehicle Types
        public string GetAllVehicleTypes(string token, out ICollection<VehicleType>? vehicleTypes)
        {
            return new ModelRetriever().RetrieveAllVehicleTypes(token, out vehicleTypes);
        }

        //retrive all brands with parameter vehicle type name
        public string GetAllBrandsOf(string token, string vehicleTypeName, out ICollection<Brand>? brands)
        {
            return new ModelRetriever().RetrieveAllBrands(vehicleTypeName, token, out brands);
        }

        //retrieve all parking spaces (all or available only)
        public string GetAllParkingSpaces(string token, int toRetrieve, out ICollection<ParkingSpaces>? allParkingSpaces)
        {
            return new ModelRetriever().RetrieveAllParkingSpaces(token, toRetrieve, out allParkingSpaces);
        }

        //creating an entry
        public string CreateAnEntry(string token, CreateEntryForm createEntryForm)
        {
            return new CreateEntryController(token, createEntryForm).Create();
        }

        //retrieve vehicle using license palte
        public string GetVehicle(string token, string licensePlate, out Vehicle? vehicle)
        {
            return new ModelRetriever().RetrieveVehicle(token, VehicleRetriever.ALL, licensePlate, out vehicle);
        }



        //tester method
        public static void Main(string[] args)
        {
            
        }
        
    }
}
