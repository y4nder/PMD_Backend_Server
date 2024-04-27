using PMD_Backend.controller.adminControllers;
using PMD_Backend.controller.CreateEntryControllers;
using PMD_Backend.models;
using PMD_Backend.util;

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

        //creating an entry
        public string CreateAnEntry(string token, CreateEntryForm createEntryForm)
        {
            return new CreateEntryController(token, createEntryForm).Create();
        }



        //tester method
        public static void Main(string[] args)
        {
            string token = "n0BskRBOaGiAcUaVIj8qhe34N6Jp2PBG";
            var form = new CreateEntryForm
            {
                VehicleTypeName = "SUV",
                VehicleBrand = "Toyota",
                LicensePlate = "ABC123",
                ParkOutDateTime = DateTime.Now.AddDays(30),
                OwnerFirstName = "John",
                OwnerLastName = "Doe",   
                FloorLevel = "A"
        };

            var message = new Server().CreateAnEntry(token, form);

            Console.WriteLine(message);
        }


        
    }
}
