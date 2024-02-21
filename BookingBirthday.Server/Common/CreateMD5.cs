using System.Security.Cryptography;
using System.Text;

namespace BookingBirthday.Server.Common
{
    public class CreateMD5
    {
        public static string MD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            byte[] result = md5.Hash!;

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
