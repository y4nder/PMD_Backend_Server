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
        public string RegisterAdmin(Admin admin)
        {
            return new RegisterController(admin).Register();
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
            string token = "MPCDbWQEHYGaX1Iq9IzvHdpyW1qoUkqv";
            var message = new Server().GetAllParkingSpaces(token, out ICollection<ParkingSpaces>? allParkingSpaces);
            Console.WriteLine(message);
            if(allParkingSpaces != null)
            {
                foreach(var parkingSpace in allParkingSpaces)
                {
                    Console.WriteLine(parkingSpace);
                    Console.WriteLine();
                }
            }
        }


        
    }
}
