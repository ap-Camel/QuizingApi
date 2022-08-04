using System.Security.Cryptography;
using System.Text;


namespace QuizingApi.Helpers {
    public class Hashing {

        public static async Task<string> hash(string password) {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 myHash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = await myHash.ComputeHashAsync(new MemoryStream(enc.GetBytes(password)));

                foreach (Byte b in result)
                {
                    Sb.Append(b.ToString("x2"));
                }
            }

            return Sb.ToString();
        }
    }
}