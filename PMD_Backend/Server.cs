using PMD_Backend.controller.adminControllers;
using PMD_Backend.models;
using PMD_Backend.util;
using System.Net.WebSockets;

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
        public string GetAllBrandsOf(string vehicleTypeName, string token, out ICollection<Brand>? brands)
        {
            return new ModelRetriever().RetrieveAllBrands(vehicleTypeName, token, out brands);
        }








        //tester method
        public static void Main(string[] args)
        {
            string token = "uJ1e7dnLMIy2xDbC6QUfRpl5FQyGBrF5";
            string message = new Server().LogOutAdmin(token);
            Console.WriteLine(message);
        }


        
    }
}
