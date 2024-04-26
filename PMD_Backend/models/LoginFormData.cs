using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.models
{
    public class LoginFormData
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public bool CompleteFields(out string message)
        {
            bool flag = true;
            message = "";
            if(Username == null)
            {
                message += "username must not be empty\n";
                flag = false;
            }

            if(Password == null)
            {
                message += "password must not be empty\n";
                flag = false;
            }
            
            return flag;
        }
    }
}
