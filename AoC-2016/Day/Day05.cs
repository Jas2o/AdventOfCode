using AoC.MD5;
using System.Text;

namespace AoC.Day {
    public class Day05
    {
        public static void Run(string file) {
            Console.WriteLine("Day 5: How About a Nice Game of Chess?" + Environment.NewLine);

            string input = File.ReadAllText(file);
            
            int passwordLength = 8;
            StringBuilder passA = new StringBuilder();
            char[] passB = new char[passwordLength];

            MD5Worker md5worker = new MD5Worker();

            int num = 0;
            while (true) {
                string secret = num.ToString();
                string output = md5worker.GetUpper(input + secret);

                if (output.Substring(0, 5) == "00000") {
                    Console.WriteLine(output);
                    if(passA.Length < passwordLength)
                        passA.Append(output[5]);

                    if(int.TryParse(output.Substring(5, 1), out int pos)) {
                        if(pos < passwordLength && passB[pos] == 0) {
                            passB[pos] = output[6];
                        }
                    }
                }

                if (passA.Length == passwordLength && !passB.Contains((char)0))
                    break;
                num++;
            }

            string partA = passA.ToString().ToLower();
            string partB = new string(passB).ToLower();

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: c6697b55
            Console.WriteLine("Part 2: " + partB);
            //Answer: 8c35d1ab
        }
    }
}
