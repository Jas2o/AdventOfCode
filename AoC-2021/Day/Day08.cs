using System.Text;

namespace AoC.Day
{
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: Seven Segment Search" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            Dictionary<int, int> uniqueToNum = new Dictionary<int, int>() {
                { 2, 1 },
                { 4, 4 },
                { 3, 7 },
                { 7, 8 }
            };

            int partA = 0;
            int partB = 0;
            foreach (string line in lines) {
                string[] side = line.Split(" | ");
                string[] left = side[0].Split(' ');
                string[] right = side[1].Split(' ');

                Dictionary<int, char[]> solved = new Dictionary<int, char[]>();
                List<char[]> five = new List<char[]>();
                List<char[]> six = new List<char[]>();
                char[] horizontal = [];

                // 1, 4, 7 and 8
                foreach (string le in left) {
                    char[] letters = le.ToCharArray();
                    if (uniqueToNum.ContainsKey(le.Length))
                        solved.Add(uniqueToNum[le.Length], letters);
                    else if(le.Length == 5)
                        five.Add(letters);
                    else
                        six.Add(letters);
                }

                // 3 (from 7)
                foreach (char[] c in five) {
                    IEnumerable<char> test3 = c.Except(solved[7]);
                    if(test3.Count() == 2) {
                        solved.Add(3, c);
                        five.Remove(c); //Leaving 2 and 5
                        horizontal = c.Except(solved[1]).ToArray();
                        break;
                    }
                }

                // 0 (from 3), 9 (from 2), and 6 (remainder)
                foreach (char[] c in six) {
                    IEnumerable<char> test0 = c.Except(horizontal);
                    IEnumerable<char> test9 = c.Except(solved[4]);
                    if (test0.Count() == 4)
                        solved.Add(0, c);
                    else if (test9.Count() == 2)
                        solved.Add(9, c);
                    else
                        solved.Add(6, c);
                }

                // 5 (from 9) and 2 (remainder)
                foreach (char[] c in five) {
                    IEnumerable<char> test5 = c.Except(solved[9]);
                    if(test5.Count() == 0)
                        solved.Add(5, c);
                    else
                        solved.Add(2, c);
                }

                Dictionary<string, int> ordered = new Dictionary<string, int>();
                foreach (KeyValuePair<int, char[]> pair in solved) {
                    string str = new string(pair.Value.Order().ToArray());
                    ordered.Add(str, pair.Key);
                }

                StringBuilder output = new StringBuilder();
                foreach (string ri in right) {
                    if (uniqueToNum.ContainsKey(ri.Length))
                        partA++;

                    string sorted = new string(ri.ToCharArray().Order().ToArray());
                    int num = ordered[sorted];
                    output.Append(num);
                }
                string outputS = output.ToString();
                Console.WriteLine("{0}: {1}", side[1], outputS);
                partB += int.Parse(outputS);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 288
            Console.WriteLine("Part 2: " + partB);
            //Answer: 940724
        }
    }
}
