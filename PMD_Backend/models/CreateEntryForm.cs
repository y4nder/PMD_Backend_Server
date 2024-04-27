using PMD_Backend.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.models
{
    public class CreateEntryForm
    {
        public string? VehicleTypeName {  get; set; }
        public string? VehicleBrand { get; set; }
        public string? LicensePlate{ get; set; }

        public DateTime? ParkOutDateTime { get; set; }

        public string? OwnerFirstName { get; set; }

        public string? OwnerLastName { get; set;}

        public string? FloorLevel { get; set; } 

        public bool ValidateParkOut(out string message)
        {
            message = Message.OK;
            bool flag = true;

            if(ParkOutDateTime < DateTime.Now)
            {
                flag = false;
                message = Message.INVALID_PARK_OUT_DATE_TIME;
            }

            return flag;
        }

        public bool CompleteFields(out string message)
        {
            message = "";
            bool flag = true;

            if(VehicleTypeName == null)
            {
                flag = false;
                message += "Vehicle Type Name \n";
            }

            if(VehicleBrand == null)
            {
                flag = false;
                message += "Vehicle Brand \n";
            }

            if(LicensePlate == null)
            {
                flag = false;
                message += "License Plate \n";
            }

            if(OwnerFirstName == null)
            {
                flag = false;
                message += "Owner First name \n";
            }

            if(OwnerLastName == null)
            {
                flag = false;
                message += "Owner Last name \n";
            }

            if(ParkOutDateTime  == null)
            {
                flag = false;
                message += "Park Out Date Time \n";
            }

            if (flag == false) message += " cannot be empty";

            return flag;
        }
    }
}
