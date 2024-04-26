using System.Security.Cryptography;

namespace PMD_Backend.util
{
    public class TokenGenerator
    {
        public static string GenerateToken(int length = 32)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] tokenChars = new char[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[length];
                rng.GetBytes(tokenData);

                for (int i = 0; i < length; i++)
                {
                    int index = tokenData[i] % chars.Length;
                    tokenChars[i] = chars[index];
                }
            }

            return new string(tokenChars);
        }
    }
}
