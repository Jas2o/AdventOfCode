namespace AoC.Day
{
    public class Day13
    {
        public static void Run(string file) {
            Console.WriteLine("Day 13: Shuttle Search" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            int estimate = int.Parse(lines[0]);
            Console.WriteLine("Given estimate: {0}", estimate);

            int partA = 0;
            int partA_earliest = int.MaxValue;
            int partA_wait = 0;

            string[] services = lines[1].Split(',');
            Dictionary<int, int> buses = new Dictionary<int, int>();
            for (int i = 0; i < services.Length; i++) {
                if (services[i] == "x")
                    continue;
                int id = int.Parse(services[i]);
                buses.Add(i, id);

                int ceiling = (int)Math.Ceiling((double)estimate / id);
                int next = ceiling * id;
                int wait = next - estimate;

                Console.WriteLine("{0,3} = {1}, waiting {2,3}, result {3}", id, next, wait, id * wait);

                if (next < partA_earliest) {
                    partA = id;
                    partA_earliest = next;
                    partA_wait = wait;
                }
            }

            partA = partA * partA_wait;

            long partB = buses.First().Value;
            long partB_loop = partB;
            foreach (KeyValuePair<int, int> bus in buses.Skip(1)) {
                while ((partB + bus.Key) % bus.Value != 0)
                    partB += partB_loop;
                partB_loop = partB_loop * bus.Value;
            }

            Console.WriteLine();
            foreach (KeyValuePair<int, int> bus in buses)
                Console.WriteLine("{0,3} (+{1,2}) = {2}", bus.Value, bus.Key, partB + bus.Key);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 2095
            Console.WriteLine("Part 2: " + partB);
            //Answer: 598411311431841
        }
    }
}
