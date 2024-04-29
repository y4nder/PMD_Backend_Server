using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.interfaces
{
    internal interface Message
    {
        public string Parse(string message);

        public static readonly string OK = "OK";
        public static readonly string USER_NOT_FOUND = "User not found";
        public static readonly string INCOMPLETE_FIELDS = "Incomplete Fields";
        public static readonly string USERNAME_NOT_DOES_NOT_EXIST = "Username does not exits";
        public static readonly string INCORRECT_PASSWORD = "Incorrect password";
        public static readonly string USER_IS_ALREADY_LOGGED_IN = "User is already logged in";
        public static readonly string UNAUTHORIZED = "Unauthorized, valid token is required";
        public static readonly string INVALID_VEHICLE_TYPE = "Invalid vehicle type name";
        public static readonly string VEHICLE_TYPE_NOT_FOUND = "Vehicle type not found";
        public static readonly string SECURITY_KEY_NOT_FOUND = "Security key not found";
        public static readonly string EMPTY_FORM = "Form is Empty";
        public static readonly string INVALID_PARK_OUT_DATE_TIME = "Invalid Park Out Date Time";
        public static readonly string VEHICLE_NOT_FOUND = "Vehicle was not found";
        public static readonly string VEHICLE_ALREADY_PARKED = "Vehicle is already parked";

        public static readonly string INVALID_TYPE_AND_BRAND = "Column 'type_and_brand_FK' cannot be null";
        public static readonly string FLOOR_DOES_NOT_EXIST = "Floor does not exist";
        public static readonly string PARKING_SPACE_IS_FULL = "This Parking Space is at full capacity";
        public static readonly string INVALID_PARAMETERS = "Invalid Parameters";
        public static readonly string RESULT_CONSISTED_OF_MORE_THAN_ONE_ROW = "Result consisted of more than one row";
    }
}
