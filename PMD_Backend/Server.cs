using PMD_Backend.controller;
using PMD_Backend.models;

namespace PMD_Backend
{
    internal class Server
    {
        public void RegisterAdmin(Admin admin)
        {
            new RegisterController().Register(admin);
        }

        public static void Main(string[] args)
        {
            //test
            Server server = new Server();
            server.RegisterAdmin(new Admin { Username = "john", Email = "@gmail.com", Password = "dsfafdsf" });
        }
    }
}
