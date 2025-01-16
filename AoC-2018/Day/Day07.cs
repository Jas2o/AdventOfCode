using System.Text;

namespace AoC.Day
{
    public class Day07
    {
        public static void Run(string file) {
            Console.WriteLine("Day 7: The Sum of Its Parts" + Environment.NewLine);

            //Setup
            Dictionary<char, List<char>> stepsA = new Dictionary<char, List<char>>();
            Dictionary<char, List<char>> stepsB = new Dictionary<char, List<char>>();
			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] fields = line.Split(' ');
                char before = fields[1][0];
                char after = fields[7][0];

                if (!stepsA.ContainsKey(before)) {
                    stepsA.Add(before, new List<char>());
                    stepsB.Add(before, new List<char>());
                }
                if (!stepsA.ContainsKey(after)) {
                    stepsA.Add(after, new List<char>());
                    stepsB.Add(after, new List<char>());
                }

                stepsA[after].Add(before);
                stepsB[after].Add(before);
            }

            //Part 1
            StringBuilder partA = new StringBuilder();
            while (stepsA.Count > 0) {
                List<char> options = stepsA.Where(d => d.Value.Count == 0).Select(d => d.Key).Order().ToList();
                char selected = options.First();
                partA.Append(selected);
                stepsA.Remove(selected);

                foreach (KeyValuePair<char, List<char>> pair in stepsA) {
                    pair.Value.Remove(selected);
                }
            }

            //Part 2
            int workers = (lines.Length < 10 ? 2 : 5);
            int minTime = (lines.Length < 10 ? 0 : 60);
            Dictionary<char, int> dWorkers = new Dictionary<char, int>();

            int partB = 0;
            while (stepsB.Count > 0) {
                //Find what tasks are available, give to a worker if there's one free.
                List<char> options = stepsB.Where(d => d.Value.Count == 0).Select(d => d.Key).Order().ToList();
                foreach (char selected in options) {
                    if (!dWorkers.ContainsKey(selected) && dWorkers.Count < workers) {
                        int timeNeeded = (int)selected - 64 + minTime;
                        dWorkers[selected] = timeNeeded;
                    }
                }

                //The workers with a task make progress.
                foreach (KeyValuePair<char, int> pair in dWorkers) {
                    dWorkers[pair.Key]--;
                }

                //Output debugging text
                List<string> has = new List<string>();
                foreach (KeyValuePair<char, int> pair in dWorkers) {
                    has.Add(string.Format("{0}:{1}", pair.Key, pair.Value));
                }
                Console.WriteLine("{0} has {1}", partB, string.Join(", ", has));

                //Check for any workers with completed tasks and free them up.
                List<char> removals = dWorkers.Where(w => w.Value == 0).Select(w => w.Key).ToList();
                foreach (char selected in removals) {
                    dWorkers.Remove(selected);
                    stepsB.Remove(selected);
                    foreach (KeyValuePair<char, List<char>> pair in stepsB)
                        pair.Value.Remove(selected);
                }

                partB++;
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA.ToString());
            //Answer: DFOQPTELAYRVUMXHKWSGZBCJIN
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1036
        }
    }
}
