using System.Security.Cryptography;
using System.Text;
namespace OnlineMarketPlace.Repository
{
    public class ncryptpasswordmd5
    {
        public static string HashPasswordMD5(string password)
        {
            using (var md5 = MD5.Create())
            {
                // Chuyển mật khẩu thành mảng byte
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);

                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển mảng byte thành chuỗi hexa
                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // "x2" để định dạng thành chuỗi hexa
                }

                return sb.ToString(); // Trả về chuỗi
            }
        }
    }
}
