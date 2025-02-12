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
        public static string GeneratePublicKey()
        {

            string publicKey = "";
            try
            {
                // Generate public and private key pair
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // Get the public key
                    /*
                     When exporting the public key, ToXmlString(false) is used with the argument set 
                     to false to indicate that only the public parameters should be included in the XML string.
                     */
                    publicKey = rsa.ToXmlString(false);
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
                Console.ReadKey();
            }
            return publicKey;
        }

        public static string GeneratPrivatecKey()
        {

            string privateKey = "";
            try
            {
                // Generate public and private key pair
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // Get the public key
                    /*
                     When exporting the public key, ToXmlString(false) is used with the argument set 
                     to false to indicate that only the public parameters should be included in the XML string.
                     */
                    privateKey = rsa.ToXmlString(true);
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
                Console.ReadKey();
            }
            return privateKey;
        }


        private static readonly string _PrivateKey = "<RSAKeyValue><Modulus>9oToS1KK+NTOY96kD3bRyAqe0za7o5LOHCyAh9TRI/oVCsDBWTPhaXsMzr5VOI1xPd2qhq6gtTQIn06zxBUEHEB9gq90MF4o+Sle+zMDznQEbRC9QVQiP9VrVKexVM70K93mwcfSIzh+lFsVgrf5RtTo/8pqLgInW4S9pcCFlc0=</Modulus><Exponent>AQAB</Exponent><P>/ZPzbhbwn42h+MqthZNs75uAXbYSlTWrSOafQKE24PRSiyC4C20ehpqK5sq3sOjfb5PT+Ll/IHpf8ZiCStrhAw==</P><Q>+N+yR20pOF2afpVYW1txI5DsO0NDOoNpXH9kHS0NzpXCKVrX32+NE/h5owLNyDl2AINYe/gbxF3d88ohfCss7w==</Q><DP>1Y724u6a9CT6FmfLP8XrZthVgZbHi7ZJbPodgPbFGytpIRcLKURbAw3AkaKElY9qLbQYP08qC5ZEm1nP0W8JNQ==</DP><DQ>hs1T2ZBtJVS+HBZ0t3c+Pw9+hVMXlRgc97cg2RTQR7eiZZgJGcNoXQrXh846/Frzaa+7O9rV33Ugha5UmNciDQ==</DQ><InverseQ>k8PyV7vS01cBWP7yaIpaOSK/cgiHKBMpQSqZQS9b+Jotm7OeB+iDxBWll0mCG/OPFDjZHo47Z9F+UJ2W+fY0SQ==</InverseQ><D>QfooQaCB2T77TfdUaV8fni5Ze6X2ajzsNNYkQDZRExPndc1I8hrUhWqNbve5zzzrxecBsLIkCNs4Q4rIyf3l7zNJmH68JwsektYAqp+hQwRoSxfCs9HoY8qiYbMotjPyLOjE6797Ytw9I2ybm8ept0mIbstObvE1L3Rq4whnfXE=</D></RSAKeyValue>";
        private static readonly string _PublicKey = "<RSAKeyValue><Modulus>9oToS1KK+NTOY96kD3bRyAqe0za7o5LOHCyAh9TRI/oVCsDBWTPhaXsMzr5VOI1xPd2qhq6gtTQIn06zxBUEHEB9gq90MF4o+Sle+zMDznQEbRC9QVQiP9VrVKexVM70K93mwcfSIzh+lFsVgrf5RtTo/8pqLgInW4S9pcCFlc0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        public static string GetPublicKey()
        {
            return _PublicKey;
        }

        public static string GetPrivateKey()
        {
            return _PrivateKey;
        }

        private static readonly Random random = new Random();

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static  string password = "123456";
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








    }
}