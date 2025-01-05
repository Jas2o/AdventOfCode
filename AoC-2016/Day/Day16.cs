using System.Text;

namespace AoC.Day
{
    public class Day16
    {
        public static void Run(string file) {
            Console.WriteLine("Day 16: Dragon Checksum" + Environment.NewLine);

            string input = File.ReadAllText(file);
            int diskLen = 272;
            if (input == "10000")
                diskLen = 20;

            string filled = GetFilled(input, diskLen);
            string checksum = GetChecksum(filled);

            string partB = GetChecksum(GetFilled(input, 35651584));

            Console.WriteLine("Part 1: " + checksum.ToString());
            //Answer: 00000100100001100
            Console.WriteLine("Part 2: " + partB);
            //Answer: 00011010100010010
        }

        private static string GetFilled(string input, int length) {
            StringBuilder sb = new StringBuilder();
            sb.Append(input);
            
            while (sb.Length < length) {
                string a = sb.ToString();
                StringBuilder b = new StringBuilder();
                for (int i = a.Length - 1; i >= 0; i--) {
                    if (a[i] == '0')
                        b.Append('1');
                    else
                        b.Append('0');
                }
                string res = "0" + b.ToString();
                sb.Append(res);
            }

            return sb.ToString().Substring(0, length);
        }

        private static string GetChecksum(string filled) {
            string checksum = filled;
            do {
                StringBuilder cs = new StringBuilder();
                for (int i = 1; i < checksum.Length; i += 2) {
                    char left = checksum[i - 1];
                    char right = checksum[i];

                    if (left == right)
                        cs.Append("1");
                    else
                        cs.Append("0");
                }
                checksum = cs.ToString();
            } while (checksum.Length % 2 == 0);

            return checksum;
        }
    }
}
