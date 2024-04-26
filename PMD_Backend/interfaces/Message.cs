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
    }
}
