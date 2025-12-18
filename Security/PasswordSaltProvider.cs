using System.Security.Cryptography;

namespace myclinic_back.Security
{
    public class PasswordSaltProvider
    {
        public static string GetSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string b64Salt = Convert.ToBase64String(salt);

            return b64Salt;
        }
    }
}
