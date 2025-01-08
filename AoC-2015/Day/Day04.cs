using AoC.MD5;

namespace AoC.Day {
    public class Day04
    {
        public static void Run(string file) {
            Console.WriteLine("Day 4: The Ideal Stocking Stuffer" + Environment.NewLine);

            MD5Worker md5worker = new MD5Worker();

            string input = File.ReadAllText(file);
            string answer1 = string.Empty;
            string answer2 = string.Empty;
            int num = 0;
            while (true) {
                string secret = num.ToString();
                string output = md5worker.GetUpper(input + secret);

                if (answer1 == string.Empty) {
                    if (output.Substring(0, 5) == "00000")
                        answer1 = secret;
                }
                if (answer2 == string.Empty) {
                    if (output.Substring(0, 6) == "000000") {
                        answer2 = secret;
                        break;
                    }
                }

                num++;
            }

            Console.WriteLine("Part 1: " + answer1);
            //Answer: 282749
            Console.WriteLine("Part 2: " + answer2);
            //Answer: 9962624
        }
    }
}
