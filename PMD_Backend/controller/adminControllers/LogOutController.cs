﻿using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using PMD_Backend.util;

namespace PMD_Backend.controller.adminControllers
{
    internal class LogOutController
    {
        private string token;
        public LogOutController(string token)
        {
            this.token = token;
        }

        public string LogOut()
        {
            string message = "";
            Admin? admin;

            //retreive admin token if exists
            var verifyer = new TokenVerifyer(token);
            
            message = verifyer.Message;
            if (message != Message.OK) return message;

            //get details of user
            admin = verifyer.getUser();

            //if admin is logged in then remove its token on usertokens table
            if (admin != null)
            {
                //remove its token
                message = removeToken(admin.Token);
                if (message != Message.OK) return message;

                //create log for action
                message = new AdminLogCreator().CreateLog(admin, "logged out");
                if (message != Message.OK) return message;
            }

            return message;
        }

        private string removeToken(string adminToken)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string table = "usertokens";
                    string query = $"DELETE FROM {table} WHERE `token` = @token";
                    using (var sqlCommand = new MySqlCommand(query, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@token", adminToken);
                        connection.Open();
                        int rows = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine("rows affected: " + rows + " at table " + table);

                        if (rows == 0)
                        {
                            //if token was not found
                            return Message.UNAUTHORIZED;
                        }
                    }
                    return Message.OK;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
