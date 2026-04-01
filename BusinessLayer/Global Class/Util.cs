using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Global_Class
{
    public static class Util
    {


        private static readonly Random random = new Random();

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string password = "123456";
        public static string GenerateAccountNumber(int length = 12)
        {
            const string digits = "0123456789";
            char[] result = new char[length];

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result[i] = digits[random.Next(digits.Length)];
            }
            return new string(result);
        }

        public static string Encrypt(string plainText, string publicKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);


                    byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
                    return Convert.ToBase64String(encryptedData);
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Encryption error: {ex.Message}");
                throw; // Rethrow the exception to be caught in the Main method
            }
        }

        public static string Decrypt(string cipherText, string privateKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);


                    byte[] encryptedData = Convert.FromBase64String(cipherText);
                    byte[] decryptedData = rsa.Decrypt(encryptedData, false);


                    return Encoding.UTF8.GetString(decryptedData);
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Decryption error: {ex.Message}");
                throw; // Rethrow the exception to be caught in the Main method
            }
        }







    }
}