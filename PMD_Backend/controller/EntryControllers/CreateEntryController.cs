using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using PMD_Backend.util;
using PMD_Backend.util.model_retrievers;
using System.Data.Common;

namespace PMD_Backend.controller.EntryControllers
{
    public class CreateEntryController : Auditable, Message
    {
        private CreateEntryForm? createEntryForm = null!;
        private string? token;

        public CreateEntryController(string token, CreateEntryForm? createEntryForm)
        {
            this.createEntryForm = createEntryForm;
            this.token = token;
        }

        public string Create()
        {
            string message = "";

            //validate if createEntryForm injection is not null
            if(createEntryForm == null)
            {
                return Message.EMPTY_FORM;
            }

            //validate form
            if(!(createEntryForm.CompleteFields(out message)))
            {
                return message;
            }

            //validate token
            if(token == null)
            {
                return Message.UNAUTHORIZED;
            }

            var verifier = new TokenVerifyer(token);
            message = verifier.Message;
            if(verifier.Message != Message.OK) return message;

            var retrievedAdmin = verifier.getUser();

            //validate park out date time
            if (!createEntryForm.ValidateParkOut(out message))
            {
                return message;
            }

            //validate if license plate is already parked
            if (createEntryForm.LicensePlate == null) return Message.EMPTY_FORM;
            message = new ModelRetriever().RetrieveVehicle(token, VehicleRetriever.PARKED, createEntryForm.LicensePlate, out Vehicle? retrievedVehicle);
            if (message != Message.OK && message != Message.VEHICLE_NOT_FOUND) return message;

            //if there is a retrieved vehicle
            if(retrievedVehicle != null)
            {
                return Message.VEHICLE_ALREADY_PARKED;
            }

            //validate floor level
            if (createEntryForm.FloorLevel == null) return Message.EMPTY_FORM;
            message = new FloorLevelValidator(createEntryForm.FloorLevel).Validate();
            if( (message != Message.OK && message == Message.FLOOR_DOES_NOT_EXIST) || message == Message.PARKING_SPACE_IS_FULL) return message;

            //save entry to database
            message = SaveEntry(retrievedAdmin, out string? creationCode);
            if(message != Message.OK) return message;
            if (creationCode == null) return message;

            //log audit
            message = new ModelRetriever().RetrieveVehicle(token, VehicleRetriever.BY_CREATION_CODE, creationCode, out Vehicle? savedVehicle);
            if(message != Message.OK) return message;

            if(retrievedAdmin != null && savedVehicle != null)
                CreateAudit(Auditable.CREATED_AN_ENTRY, retrievedAdmin, savedVehicle);

            return message;
        }

        public string CreateAudit(string auditAction, Admin admin, Vehicle vehicle)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            { 
                try
                {
                    string table = "audits";
                    connection.Open();
                    string query = $"INSERT INTO {table}(`admin_FK`, `audit_action`, `vehicle_FK`) VALUES ({admin.Id}, \"{auditAction}\", {vehicle.Id})";
                    using(var command = new MySqlCommand(query, connection))
                    {
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine($"rows affected: {rows} at table {table}");
                    }
                    return Message.OK;
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
            }
        }



        private string SaveEntry(Admin? retrievedAdmin, out string? creationCode)
        {
            creationCode = TokenGenerator.GenerateToken();                     //generate unique creation 
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string procedure = "CreateNewEntry";
                    using(var command = new MySqlCommand(procedure, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        if(createEntryForm != null)
                        {
                            command.Parameters.AddWithValue("@vehicle_type_name", createEntryForm.VehicleTypeName);
                            command.Parameters.AddWithValue("@vehicle_brand_name", createEntryForm.VehicleBrand);
                            command.Parameters.AddWithValue("@license_plate", createEntryForm.LicensePlate);
                            command.Parameters.AddWithValue("@park_out_date_time", createEntryForm.ParkOutDateTime);
                            command.Parameters.AddWithValue("@owner_first_name", createEntryForm.OwnerFirstName);
                            command.Parameters.AddWithValue("@owner_last_name", createEntryForm.OwnerLastName);
                            command.Parameters.AddWithValue("@floor_level", createEntryForm.FloorLevel);
                            command.Parameters.AddWithValue("@creationCode", creationCode);
                            int rows = command.ExecuteNonQuery();
                            Console.WriteLine($"rows affected : {rows} at creation");
                        }
                        else
                        {
                            return Message.EMPTY_FORM;
                        }
                    }
                    
                    return Message.OK;
                }
                catch (Exception e)
                {
                    return Parse(e.Message);
                }
            }
        }

        public string Parse(string message)
        {
            if (message == Message.INVALID_TYPE_AND_BRAND)
            {
                message = "Invalid type and brand";
            }

            //override exception
            if(message == Message.RESULT_CONSISTED_OF_MORE_THAN_ONE_ROW)
            {
                message = Message.OK;
            }

            return message;
        }
    }
}
