using System.Text;

namespace AoC.MD5 {
    public class MD5Worker {
        private System.Security.Cryptography.MD5 md5;

        public MD5Worker() {
            md5 = System.Security.Cryptography.MD5.Create();
            //Using create a lot slows it down.
        }

        public string GetUpper(string input) {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }

        public string GetLower(string input) {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes).ToLower();
        }
    }
}
