using PMD_Backend.controller.adminControllers;
using PMD_Backend.models;
using PMD_Backend.util;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;

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
            return new ModelRetriever().GetAllVehicleTypes(token, out vehicleTypes);
        }

        //retrive all brands with parameter vehicle type name
        public string GetAllBrandsOf(string token, string vehicleTypeName, out ICollection<Brand>? brands)
        {
            return new ModelRetriever().RetrieveAllBrands(vehicleTypeName, token, out brands);
        }

        //retrieve all parking spaces
        public string GetAllParkingSpaces(string token, out ICollection<ParkingSpaces>? allParkingSpaces)
        {
            return new ModelRetriever().RetrieveAllParkingSpaces(token, out allParkingSpaces);
        }

        //retrieve all available parkingspaces
        public string GetAllAvailableParkingSpaces(string token, out ICollection<ParkingSpaces>? allParkingSpaces)
        {
            return new ModelRetriever().RetrieveAllAvailableParkingSpaces(token, out allParkingSpaces);
        }


        //tester method
        public static void Main(string[] args)
        {
            var message = new Server().LogInAdmin(new LoginFormData
            {
                Username = "admin",
                Password = "admin",
            }, out Admin? admin);

            Console.WriteLine(message);
            Console.WriteLine(admin);
        }


        
    }
}
