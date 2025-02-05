namespace AoC.Day {
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: Crossed Wires" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            string[] steps1 = lines[0].Split(',');
            string[] steps2 = lines[1].Split(',');

            Dictionary<(int, int), char> locations = new Dictionary<(int, int), char>() { {(0, 0), 'o' } };
            Dictionary<(int, int), int> locations1 = new Dictionary<(int, int), int>();
            Dictionary<(int, int), int> locations2 = new Dictionary<(int, int), int>();

            Simulate(locations, locations1, steps1, '1');
            Simulate(locations, locations2, steps2, '2');
            int maxX = locations.Max(n => n.Key.Item1);
            if (maxX < Console.WindowWidth)
                DrawMap(locations);

            int partA = int.MaxValue;
            int partB = int.MaxValue;
            KeyValuePair<(int, int), char>[] crosses = locations.Where(n => n.Value == 'X').ToArray();
            foreach(KeyValuePair<(int, int), char> cross in crosses) {
                int manhattan = Math.Abs(cross.Key.Item1) + Math.Abs(cross.Key.Item2);
                if (manhattan < partA)
                    partA = manhattan;

                int one = locations1[cross.Key];
                int two = locations2[cross.Key];
                int add = one + two;
                if (add < partB)
                    partB = add;
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 721
            Console.WriteLine("Part 2: " + partB);
            //Answer: 7388
        }

        private static void Simulate(Dictionary<(int, int), char> locations, Dictionary<(int, int), int> locationsD, string[] steps, char display) {
            int x = 0;
            int y = 0;

            int totalDistance = 1;
            foreach (string step in steps) {
                char dir = step[0];
                int distance = int.Parse(step.Substring(1));

                for (int i = 0; i < distance; i++) {
                    switch (dir) {
                        case 'U': y--; break;
                        case 'R': x++; break;
                        case 'D': y++; break;
                        case 'L': x--; break;
                    }

                    locationsD.TryAdd((x, y), totalDistance + i);
                    bool added = locations.TryAdd((x, y), display);
                    if (!added) {
                        if (locations[(x, y)] != display)
                            locations[(x, y)] = 'X';
                    }
                }

                totalDistance += distance;
            }
        }

        private static void DrawMap(Dictionary<(int, int), char> nodes) {
            int minY = nodes.Min(n => n.Key.Item2);
            int minX = nodes.Min(n => n.Key.Item1);
            int maxY = nodes.Max(n => n.Key.Item2);
            int maxX = nodes.Max(n => n.Key.Item1);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (nodes.ContainsKey((x, y))) {
                        char c = nodes[(x, y)];
                        if (c == '1')
                            Console.ForegroundColor = ConsoleColor.Red;
                        else if (c == '2')
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(c);
                        Console.ResetColor();
                    } else
                        Console.Write(" ");
                }
                //Console.WriteLine("   " + y);
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
