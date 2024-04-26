using MySql.Data.MySqlClient;
using PMD_Backend.interfaces;
using PMD_Backend.models;
using System.Security.Cryptography;
namespace PMD_Backend.util
{
    public  class Security
    {
        public byte[] Encrypt(string data, byte[] key, byte[] iv)
        {
            byte[] cipheredData;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(data);
                        }

                        cipheredData = memoryStream.ToArray();
                    }
                }
            }
            return cipheredData;
        }

        public string Decrypt(byte[] cipheredData, byte[] key, byte[] iv)
        {
            string data = String.Empty;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream(cipheredData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            data = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return data;
        }

        public SecurityKey CreateSecurityKey()
        {
            var securityKey = new SecurityKey();
            GenerateKeyAndIV(out byte[] key, out byte[] iv);
            securityKey.Key = key;
            securityKey.Iv = iv;    
            return securityKey;
        }

        private void GenerateKeyAndIV(out byte[] key, out byte[] iv)
        {
            key = new byte[16];
            iv = new byte[16];  

            using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }
        }

        public bool ValidatePassword(string password, Admin admin)
        {
            string message = "";

            //retrieve security keys 
            message = RetrieveSecurityKeys(admin.Id, out SecurityKey? securityKey);

            if (securityKey == null) return false;

            if (message != Message.OK) return false;

            //decrypt password from database
            string decryptedPassword = Decrypt(admin.Password, securityKey.Key, securityKey.Iv);

            //compare passwords
            return password.Equals(decryptedPassword);
        }

        private string RetrieveSecurityKeys(int id, out SecurityKey? securityKey)
        {
            using (var connection = new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
            {
                try
                {
                    connection.Open();
                    string table = "security_keys";
                    string query = $"SELECT * FROM {table} WHERE admin_FK = {id}";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using(var reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                securityKey = new SecurityKey
                                {
                                    Id = (int)reader["admin_FK"],
                                    Key = (byte[])reader["security_key"],
                                    Iv = (byte[])reader["security_iv"]
                                };
                            } 
                            else
                            {
                                securityKey = null;
                                return Message.SECURITY_KEY_NOT_FOUND;
                            }

                        }
                    }

                    return Message.OK;
                }
                catch (Exception ex)
                {
                    securityKey = null;
                    return ex.Message;
                }
            }
        }
    }
}
