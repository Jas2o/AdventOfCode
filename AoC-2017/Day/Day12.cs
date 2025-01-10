namespace AoC.Day
{
    public class Day12
    {
        public static void Run(string file) {
            Console.WriteLine("Day 12: Digital Plumber" + Environment.NewLine);

            Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] parts = line.Replace(" ", "").Split("<->");
                List<string> others = parts[1].Split(',').ToList();
                if (graph.ContainsKey(parts[0]))
                    graph[parts[0]].AddRange(others);
                else
                    graph.Add(parts[0], others);
            }

            Dictionary<string, int> groups = new Dictionary<string, int>();
            foreach(string key in graph.Keys) {
                List<string> group = GetGroup(graph, key);
                group.Sort();
                string s = string.Join(',', group);

                groups.TryAdd(s, group.Count);
            }

            Console.WriteLine("Part 1: " + groups.First().Value);
            //Answer: 239
            Console.WriteLine("Part 2: " + groups.Count);
            //Answer: 215
        }

        private static List<string> GetGroup(Dictionary<string, List<string>> graph, string find) {
            List<string> group = new List<string>();

            Queue<string> queue = new Queue<string>();
            queue.Enqueue(find);
            while (queue.Count > 0) {
                string look = queue.Dequeue();
                if (!group.Contains(look)) {
                    group.Add(look);

                    List<string> connected = graph[look];
                    foreach (string c in connected)
                        queue.Enqueue(c);
                }
            }

            return group;
        }
    }
}
