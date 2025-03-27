namespace AoC.Day
{
    public class Day06
    {
        public static void Run(string file) {
            Console.WriteLine("Day 6: Custom Customs" + Environment.NewLine);

            List<string> lines = File.ReadAllLines(file).ToList();
            lines.Add(string.Empty);

            List<Dictionary<char, int>> groups = new List<Dictionary<char, int>>();
            Dictionary<char, int> inprogress = new Dictionary<char, int>();
            int count = 0;
            Dictionary<Dictionary<char, int>, int> groupMemberCount = new Dictionary<Dictionary<char, int>, int>();
            foreach (string line in lines) {
                if (line.Length == 0) {
                    if (inprogress.Any()) {
                        groups.Add(inprogress);
                        groupMemberCount.Add(inprogress, count);
                        inprogress = new Dictionary<char, int>();
                        count = 0;
                    }
                } else {
                    foreach(char c in line) {
                        if (inprogress.ContainsKey(c))
                            inprogress[c]++;
                        else
                            inprogress.Add(c, 1);
                    }
                    count++;
                }
            }

            int partA = 0;
            int partB = 0;
            foreach(KeyValuePair<Dictionary<char, int>, int> group in groupMemberCount) {
                partA += group.Key.Keys.Count();
                int b = 0;
                foreach(KeyValuePair<char, int> thing in group.Key) {
                    if (thing.Value == group.Value)
                        b++;
                }
                partB += b;
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 6583
            Console.WriteLine("Part 2: " + partB);
            //Answer: 3290
        }
    }
}
