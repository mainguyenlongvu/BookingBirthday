using System.Security.Cryptography;
using System.Text;

namespace BookingBirthday.Server.Common
{
    public class CreateMD5Short
    {
        public static string MD5Short(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convert the byte array to a shorter hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 4; i++) // Chỉ lấy 4 byte đầu tiên
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
