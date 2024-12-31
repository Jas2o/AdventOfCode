using System.Text;

namespace AoC.Day
{
    public class Day04
    {
        private static string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public static void Run(string file) {
            Console.WriteLine("Day 4: Security Through Obscurity" + Environment.NewLine);

            int sum = 0;
            int north = 0;
            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                int sector = CheckThis(line, out string name);
                if (sector > 0) {
                    sum += sector;
                    StringBuilder sb = new StringBuilder();
                    foreach(char c in name) {
                        if (c == '-')
                            sb.Append(' ');
                        else {
                            int num = (((c - 'a') + sector) % 26) + 'a';
                            sb.Append((char)num);
                        }
                    }
                    string nameDecrypted = sb.ToString();
                    if (nameDecrypted.Contains("north")) {
                        north = sector;
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.WriteLine("[{0}] is [{1}]", name, nameDecrypted);
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + sum);
            //Answer: 185371
            Console.WriteLine("Part 2: " + north);
            //Answer: 984
        }

        private static int CheckThis(string line, out string name) {
            int indexSquare = line.IndexOf('[');
            string checksum = line.Substring(indexSquare + 1, 5);
            name = line.Substring(0, indexSquare);
            int indexLastDash = line.LastIndexOf('-');
            int sector = int.Parse(name.Substring(indexLastDash + 1));
            name = name.Substring(0, indexLastDash);

            //Console.WriteLine("{0} // {1} // {2}", name, sector, checksum);

            Dictionary<char, int> check = new Dictionary<char, int>();
            foreach (char c in alphabet) {
                int count = name.Count(x => x == c);
                if(count > 0)
                    check.Add(c, count);
            }

            KeyValuePair<char,int>[] checks = check.OrderByDescending(x => x.Value).ToArray();
            //foreach(KeyValuePair<char, int> pair in checks) {
                //Console.WriteLine("CHECK {0} was {1} times", pair.Key, pair.Value);
            //}

            for(int i = 0; i < 5; i++) {
                char c = checksum[i];
                int count = name.Count(x => x == c);
                
                //Console.WriteLine("      {0} was {1} times", c, count);

                if (checks[i].Key != c)
                    return 0;
            }

            return sector;
        }
    }
}
