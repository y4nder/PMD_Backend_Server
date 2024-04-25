using MySql.Data.MySqlClient;
using PMD_Backend.models;
namespace PMD_Backend.controller
{
    internal class RegisterController
    {
        public void Register(Admin admin) 
        {
            //create connection
            MySqlConnection conn = new MySqlConnection(Environment.GetEnvironmentVariable("mysqlCon"));
            try
            {
                conn.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to Connect to Database during admin registration");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
