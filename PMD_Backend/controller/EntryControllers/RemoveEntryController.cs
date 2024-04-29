using PMD_Backend.interfaces;
using PMD_Backend.models;
using PMD_Backend.util;
using PMD_Backend.util.model_retrievers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.controller.EntryControllers
{
    internal class RemoveEntryController
    {
        private string? token;

        public RemoveEntryController(string? token)
        {
            this.token = token;
        }

        public string Remove(string licensePlate, string? note)
        {
            if (licensePlate == null) return Message.INVALID_PARAMETERS;

            string message = string.Empty;

            //verify token
            if (token == null) return Message.UNAUTHORIZED;
            var verifyer = new TokenVerifyer(token);
            if(verifyer.Message != Message.OK) return verifyer.Message;

            //retrieve parked vehicle using license plate
            message = new VehicleRetriever().Retrieve(token, VehicleRetriever.PARKED, licensePlate, out Vehicle? vehicle);
            if(message != Message.OK) return message;
            if (vehicle == null) return Message.VEHICLE_NOT_FOUND;
                
            //retrieve vehicle types flag down and additional fee
            message = new VehicleTypesRetriever().Retrieve(token, vehicle.TypeID, out VehicleType? vehicleType);
            if(message != Message.OK ) return message;
            if(vehicleType == null) return Message.VEHICLE_TYPE_NOT_FOUND;

            //Create calculation for vehicle
            message = new PaymentCalculator().Calculate(vehicle, vehicleType);

            //---- Perform database modification ------

            //remove vehicle from parked vehicles
            if(vehicle.LicensePlate != null)
                message = new VehicleRemover().Remove(vehicle.LicensePlate);

            //add audit “removed an entry”             
            message = new AuditCreator().CreateAudit(AuditCreator.REMOVED_AN_ENTRY, verifyer.getUser(), vehicle);
            if (message != Message.OK) return message;

            //add to history logs (create history logs controller)
            message = new HistoryLogsCreator().CreateLog(vehicle, verifyer.getUser(), note);

            return message;
        }
    }
}
