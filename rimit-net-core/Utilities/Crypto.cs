using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RimitNetCore.Utilities
{
    public class EncryptResult
    {
        public string cipher_text { get; set; }        
        public string iv { get; set; }
        public string hash { get; set; }
    }

    public class Crypto
    {
        public static EncryptResult EncryptRimitData(string data, string key)
        {
            string encrypted = "";
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] iv_bytes = new byte[8];
                rng.GetBytes(iv_bytes);
                string iv = BitConverter.ToString(iv_bytes).Replace("-", "").ToLowerInvariant();

                Debug.WriteLine("---------------------");
                Debug.WriteLine("*** ENCRYPT - KEY *** " + key);
                Debug.WriteLine("*** DECENCRYPTRYPT - IV *** " + iv);
                Debug.WriteLine("*** ENCRYPT - DATA *** " + data);
                Debug.WriteLine("---------------------");
                
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = Encoding.UTF8.GetBytes(iv);
                    aes.Mode = CipherMode.CBC;

                    using (ICryptoTransform cipher = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, cipher, CryptoStreamMode.Write))
                            {
                                using (StreamWriter sw = new StreamWriter(cs))
                                {
                                    sw.Write(data);
                                }
                            }
                            encrypted = Convert.ToBase64String(ms.ToArray());
                        }                        
                    }

                }
                // CREATE SALT FROM cipher_text
                string salt = iv + iv;
                string hash = Hashing.HashData(data, salt);

                EncryptResult encriptedData = new EncryptResult() { cipher_text = encrypted, iv = iv, hash = hash };

                Debug.WriteLine("*** ENCRYPTED DATA ***");
                Debug.WriteLine(JsonSerializer.Serialize(encriptedData));
                Debug.WriteLine("---------------------");

                return encriptedData;
            }
        }

        public static JsonElement? DecryptRimitData(JsonElement data, string key)
        {
            try
            {
                string iv = data.GetProperty("iv").GetString();
                string encrypted = data.GetProperty("cipher_text").GetString();

                Debug.WriteLine("---------------------");
                Debug.WriteLine("*** DECRYPT - KEY *** " + key);
                Debug.WriteLine("*** DECRYPT - IV *** " + iv);
                Debug.WriteLine("*** DECRYPT - DATA *** " + encrypted);
                Debug.WriteLine("---------------------");

                string decriptedString = "";

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = Encoding.UTF8.GetBytes(iv);
                    aes.Mode = CipherMode.CBC;

                    using (ICryptoTransform decipher = aes.CreateDecryptor(aes.Key, aes.IV))
                    {

                        byte[] cipherText = Convert.FromBase64String(encrypted);

                        using (MemoryStream ms = new MemoryStream(cipherText))
                        {
                            using (CryptoStream cs = new CryptoStream(ms, decipher, CryptoStreamMode.Read))
                            {
                                using (StreamReader sr = new StreamReader(cs))
                                {
                                    decriptedString = sr.ReadToEnd();
                                }
                            }
                        }
                    }

                }

                JsonElement decriptedData = JsonDocument.Parse(decriptedString).RootElement;

                Debug.WriteLine("*** DECRYPTED DATA ***");
                Debug.WriteLine(decriptedString);
                Debug.WriteLine(decriptedData);
                Debug.WriteLine("---------------------");

                // CHECK THE cipher_text IS CORRECT
                string salt = iv + iv;
                bool validateHash = Hashing.HashVerify(decriptedString, data.GetProperty("hash").GetString(), salt);
                if (!validateHash)
                {
                    Debug.WriteLine("Valid Hash");
                    Debug.WriteLine(validateHash);
                    return null;
                }

                return decriptedData;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
