using PMD_Backend.controller;
using PMD_Backend.models;
using PMD_Backend.util;

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


        public static void Main(string[] args)
        {
            var mess = new Server().LogInAdmin(new LoginFormData
            {
                Username = "admin",
                Password = "admin"
            }, out Admin? admin);
            Console.WriteLine(mess);
            Console.WriteLine(admin);
        }



    }
}
