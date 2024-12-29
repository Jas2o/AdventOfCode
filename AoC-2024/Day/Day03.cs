using System.Text;

namespace AoC.Day
{
    public class Day03
    {

        private static char[] acceptable = {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',
            ',', 'm', 'u', 'l', 'd', 'o', 'n', '\'', 't', '(', ')' //mul, do, do'nt
        };

        public static void Run(string file) {
            Console.WriteLine("Day 3: Mull It Over" + Environment.NewLine);

            List<string> list = new List<string>();

            string text = File.ReadAllText(file);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (acceptable.Contains(c))
                {
                    if (c == 'm' || c == 'd')
                    {
                        sb.Clear();
                    }
                    sb.Append(c);
                    if (c == ')')
                    {
                        string temp = sb.ToString();
                        if (temp.StartsWith("do(") || temp.StartsWith("don't("))
                        {
                            list.Add(temp);
                            //Console.WriteLine(temp);
                        }
                        else if (temp.StartsWith("mul("))
                        {
                            list.Add(temp.Substring(4, temp.Length - 5));
                            //Console.WriteLine(temp);
                        }
                        sb.Clear();
                    }
                }
                else
                    sb.Clear();
            }

            //--

            int total_Part1 = 0;
            int total_Part2 = 0;
            bool enabled_Part2 = true;
            foreach (string instruction in list)
            {
                if (instruction == "do()")
                    enabled_Part2 = true;
                else if (instruction == "don't()")
                    enabled_Part2 = false;
                else
                {
                    string[] fields = instruction.Split(',');
                    if (fields.Length != 2)
                        continue;

                    int num1 = int.Parse(fields[0]);
                    int num2 = int.Parse(fields[1]);
                    int multi = num1 * num2;
                    total_Part1 += multi;

                    if (enabled_Part2)
                        total_Part2 += multi;
                }
            }

            //--

            Console.WriteLine("Part 1: " + total_Part1);
            //Answer: 183380722
            Console.WriteLine("Part 2: " + total_Part2);
            //Asnwer: 82733683
        }
    }
}
