using PMD_Backend.controller.adminControllers;
using PMD_Backend.controller.EntryControllers;
using PMD_Backend.models;
using PMD_Backend.util;
using PMD_Backend.util.model_retrievers;

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

        //retrieve all Vehicle Types
        public string GetAllVehicleTypes(string token, out ICollection<VehicleType>? vehicleTypes)
        {
            return new ModelRetriever().RetrieveAllVehicleTypes(token, out vehicleTypes);
        }

        //retrieve Vehicle Type by id
        public string GetVehicleType(string token, int id, out VehicleType? vehicleType)
        {
            return new ModelRetriever().GetVehicleType(token, id, out vehicleType);
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

        //retrieve vehicle using license plate
        public string GetVehicle(string token, string licensePlate, out Vehicle? vehicle)
        {
            return new ModelRetriever().RetrieveVehicle(token, VehicleRetriever.ALL, licensePlate, out vehicle);
        }

        //retrieve parked vehicle using license plate
        public string GetParkedVehicle(string token, string licensePlate, out Vehicle? vehicle)
        {
            return new ModelRetriever().RetrieveVehicle(token, VehicleRetriever.PARKED, licensePlate, out vehicle);
        }

        //remove an entry
        public string RemoveEntry(string token, string licensePlate, string? note)
        {
            return new RemoveEntryController(token).Remove(licensePlate, note);
        }

        //retriev all history logs
        public string GetAllHistoryLogs(string token, out ICollection<HistoryLog>? allLogs)
        {
            return new ModelRetriever().RetrieveAllLogs(token, out allLogs);
        }

        
        public static void Main(string[] args)
        {
            string token = "Y4SzBdRwu1XdI00SwtYE5cKToz720JOi";
            string message = string.Empty;
            message = new Server().GetAllHistoryLogs(token, out ICollection<HistoryLog>? h);

            Console.WriteLine(message);
            if(h!= null) foreach(var n in h) Console.WriteLine(n);
        }
    } 
}
