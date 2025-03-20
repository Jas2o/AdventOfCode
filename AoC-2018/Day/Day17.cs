namespace AoC.Day
{
    public class Day17
    {
        public static void Run(string file) {
            Console.WriteLine("Day 17: Reservoir Research" + Environment.NewLine);

            Dictionary<(int, int), char> grid = new Dictionary<(int, int), char>();

            string[] lines = File.ReadAllLines(file);
            string[] separators = new string[] { "x=", "y=", ", ", ".." };
            foreach (string line in lines) {
                string[] fields = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                int[] nums = Array.ConvertAll(fields, int.Parse);

                if (line.StartsWith("y=")) {
                    int y = nums[0];
                    for (int x = nums[1]; x <= nums[2]; x++) {
                        bool add = grid.TryAdd((x, y), 'X');
                    }
                } else if (line.StartsWith("x=")) {
                    int x = nums[0];
                    for (int y = nums[1]; y <= nums[2]; y++) {
                        grid[(x, y)] = 'Y';
                    }
                } else
                    throw new Exception();
            }

            int minY = grid.Keys.Min(k => k.Item2);
            int maxY = grid.Keys.Max(k => k.Item2);
            int springX = 500;
            grid.Add((springX, 0), '+');

            (int, int) spring = (springX, 0);
            Dictionary<(int, int), bool> usedSources = new Dictionary<(int, int), bool>(); //The value bool is unused
            Queue<(int, int)> queueWaterSource = new Queue<(int, int)>(); //x, y
            usedSources.Add(spring, true);
            queueWaterSource.Enqueue(spring);

            while(queueWaterSource.Any()) {
                (int, int) source = queueWaterSource.Dequeue();

                int sX = source.Item1;
                int sY = source.Item2;
                char current = grid[(sX, sY)];

                bool goingDown = true;
                while (goingDown) {
                    sY++;
                    if (sY > maxY)
                        break;

                    grid.TryGetValue((sX, sY), out current);
                    if (current == 'V')
                        break;
                    else if (current == 'X' || current == 'Y') {
                        goingDown = false;
                        break;
                    }
                    grid[(sX, sY)] = '|';
                }

                if (goingDown)
                    continue;

                int left = 0;
                int right = 0;
                bool flowLeft = false;
                bool flowRight = false;
                if (current == 'X') {
                    //We've reached a floor, we should work out how far we could go in either direction.
                    for (; ; left++) {
                        if (grid[(sX - left, sY)] == 'Y')
                            break;
                    }
                    for (; ; right++) {
                        if (grid[(sX + right, sY)] == 'Y')
                            break;
                    }

                    //Now we go up until one/both isn't a Y.
                    while (true) {
                        sY--;
                        grid.TryGetValue((sX - left, sY), out char sideLeft);
                        grid.TryGetValue((sX + right, sY), out char sideRight);

                        flowLeft = (sideLeft != 'Y');
                        flowRight = (sideRight != 'Y');

                        //Fill in
                        for (int fillLeft = 1; fillLeft < left; fillLeft++) {
                            var k = (sX - fillLeft, sY);
                            grid.TryGetValue(k, out sideLeft);
                            if (sideLeft == 'Y') {
                                flowLeft = false;
                                break;
                            }
                            grid[k] = '<';

                            //Fill down if needed.
                            var kBelow = (sX - fillLeft, sY + 1);
                            grid.TryGetValue(kBelow, out char sideLeftDown);
                            if(sideLeftDown == '\0') {
                                grid[kBelow] = 'V';
                                if (usedSources.TryAdd(kBelow, true))
                                    queueWaterSource.Enqueue(kBelow);
                            }
                        }
                        for (int fillRight = 1; fillRight < right; fillRight++) {
                            var k = (sX + fillRight, sY);
                            grid.TryGetValue(k, out sideRight);
                            if (sideRight == 'Y') {
                                flowRight = false;
                                break;
                            }
                            grid[k] = '>';

                            //Fill down if needed.
                            var kBelow = (sX + fillRight, sY + 1);
                            grid.TryGetValue(kBelow, out char sideRightDown);
                            if (sideRightDown == '\0') {
                                grid[kBelow] = 'V';
                                if (usedSources.TryAdd(kBelow, true))
                                    queueWaterSource.Enqueue(kBelow);
                            }
                        }

                        if (flowLeft || flowRight)
                            break;
                    }
                } else if (current == 'Y') {
                    //We've reached an edge and will be flowing down to both sides.
                    flowLeft = flowRight = true;
                    sY--;
                }

                //Add new flows.
                if (flowLeft) {
                    (int, int) flowKey = (sX - left - 1, sY);
                    grid[flowKey] = '/';
                    grid[(sX - left, sY)] = '<';
                    if (usedSources.TryAdd(flowKey, true))
                        queueWaterSource.Enqueue(flowKey);
                }
                if (flowRight) {
                    (int, int) flowKey = (sX + right + 1, sY);
                    grid[flowKey] = '\\';
                    grid[(sX + right, sY)] = '>';
                    if (usedSources.TryAdd(flowKey, true))
                        queueWaterSource.Enqueue(flowKey);
                }
            }

            Dictionary<(int, int), char> gridA = grid.ToDictionary();

            int partA = CountWater(grid, minY, maxY);

            int minX = grid.Keys.Min(k => k.Item1) - 1;
            int maxX = grid.Keys.Max(k => k.Item1) + 1;
            int width = maxX - minX;

            if (width > Console.WindowWidth) {
                Console.WriteLine("Part 1: " + partA);
                Console.WriteLine("Part 2: " + 0);
                Console.WriteLine();
                Console.WriteLine("Please resize then press any key to draw visual (or ESC to not).");

                while (width > Console.WindowWidth) {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                        return;
                }
            }

            DrawMap(gridA);
            Console.WriteLine("Part 1: " + partA);
            //Answer: 30384
            Console.WriteLine("Part 2: " + 0);
            //Answer: 
        }

        private static void DrawMap(Dictionary<(int, int), char> grid, int reqMaxY = -1, int reqMinY = -1) {
            int minX = grid.Keys.Min(k => k.Item1) - 1;
            int minY = grid.Keys.Min(k => k.Item2);
            int maxX = grid.Keys.Max(k => k.Item1) + 1;
            int maxY = grid.Keys.Max(k => k.Item2);

            if (reqMaxY != -1)
                maxY = Math.Min(maxY, reqMaxY);
            if (reqMinY != -1)
                minY = reqMinY;

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (grid.ContainsKey((x, y))) {
                        char c = grid[(x, y)];
                        if(c == 'X' || c == 'Y')
                            Console.Write(c);
                        else {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write(c);
                            Console.ResetColor();
                        }
                    } else
                        Console.Write(" ");
                }
                Console.WriteLine(" " + y);
            }
            Console.WriteLine();
        }

        private static int CountWater(Dictionary<(int, int), char> grid, int minY, int maxY) {
            int minX = grid.Keys.Min(k => k.Item1) - 1;
            //int minY = grid.Keys.Min(k => k.Item2);
            int maxX = grid.Keys.Max(k => k.Item1) + 1;
            //int maxY = grid.Keys.Max(k => k.Item2);

            int water = 0;
            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (grid.ContainsKey((x, y))) {
                        char c = grid[(x, y)];
                        if (c != '#' && c != 'X' && c != 'Y')
                            water++;
                    }
                }
            }
            return water;
        }
    }
}
