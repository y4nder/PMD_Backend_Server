﻿using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using PMD_Backend.util;

namespace PMD_Backend.controller
{
    public class LogInController : Logger
    {
        private LoginFormData loginFormData;
        
        public LogInController(LoginFormData loginFormData)
        {
            this.loginFormData = loginFormData;
        }

        public string LogIn(out Admin? admin)
        {
            admin = null;
            string message = "";
            if (!loginFormData.CompleteFields(out message))
            {
                message += " " + Message.INCOMPLETE_FIELDS;
                return message;
            }


            //Try to retrieve username
            message = new ModelRetriever().RetrieveAdmin(out admin, loginFormData.Username);

            if(message != Message.OK)
            {
                return message;
            }
            
            //validate if username exists
            if (admin == null)
            {
                return Message.USERNAME_NOT_DOES_NOT_EXIST;
            }

            //check if already logged in
            var logsMessage = new LogsChecker().Check(admin.Id, out bool adminAlreadyLoggedIn);
            if(adminAlreadyLoggedIn)
            {
                return Message.USER_IS_ALREADY_LOGGED_IN;
            }

            //validate if password matches
            if(loginFormData.Password != admin.Password)
            {
                return Message.INCORRECT_PASSWORD;
            }

            //add to logs table if login credentials are correct
            var validationMessage = new LogCreator().CreateLog(admin, "logged in");

            
            if (validationMessage != Message.OK)
            {
                //return exception message
                return validationMessage;
            } 
            else
            {
                //add token to admin model and map the token to usertokens table
                var tokenGenerationMessage = GenerateToken(admin);
                return tokenGenerationMessage;
            }
        }

        public string CreateLog(Admin admin)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string query = "INSERT INTO `adminlogs`(`adminFK`, `action`) VALUES (@adminFK, @action)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@adminFK", admin.Id);
                        command.Parameters.AddWithValue("@action", "logged in");
                        connection.Open();
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine("rows affected: " + rows);
                    }

                    return Message.OK;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        private string GenerateToken(Admin admin)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    string token = TokenGenerator.GenerateToken();
                    string table = "usertokens";
                    string query = $"INSERT INTO {table}(`admin_FK`, `token`) VALUES (@admin_FK, @token)";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@admin_FK", admin.Id);
                        command.Parameters.AddWithValue("@token", token);
                        connection.Open();
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine("rows affected: " + rows + " at table " + table);
                    }

                    //add the token to the model
                    admin.Token = token;    

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
