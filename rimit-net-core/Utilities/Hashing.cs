using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RimitNetCore.Utilities
{
    public class Hashing
    {
        public static string HashData(string data, string salt)
        {                        
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(salt), 2048, HashAlgorithmName. SHA512))
            {
                return BitConverter.ToString(pbkdf2.GetBytes(32)).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool HashVerify(string data, string hash, string salt)
        {
            try
            {
                string newHash = HashData(data, salt);
                return newHash.Equals(hash);
            }
            catch(Exception)//
            {
                return false;
            }
        }
    }
}
