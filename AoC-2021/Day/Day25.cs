namespace AoC.Day
{
    public class Day25
    {
        public static void Run(string file) {
            Console.WriteLine("Day 25: Sea Cucumber" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            Dictionary<(int,int), char> seaCucumbers = new Dictionary<(int, int), char>();
            
            int limitY = lines.Length;
            int limitX = lines[0].Length;
            for (int y = 0; y < limitY; y++) {
                for(int x = 0;  x < limitX; x++) {
                    char c = lines[y][x];
                    if(c != '.')
                        seaCucumbers[(x, y)] = c;
                }
            }

            bool drawAll = (limitY < 10);

            Draw("Initial state:", seaCucumbers, limitX, limitY);

            int steps = 0;
            while(true) {
                steps++;
                int countMoved = 0;
                IEnumerable<KeyValuePair<(int, int), char>> east = seaCucumbers.Where(s => s.Value == '>');
                IEnumerable<KeyValuePair<(int, int), char>> south = seaCucumbers.Where(s => s.Value == 'v');

                Dictionary<(int, int), char> unmoved = seaCucumbers;
                Dictionary<(int, int), char> movedE = new Dictionary<(int, int), char>();
                Dictionary<(int, int), char> movedS = new Dictionary<(int, int), char>();

                foreach(KeyValuePair<(int, int), char> s in south)
                    movedE.Add(s.Key, s.Value);
                foreach(KeyValuePair<(int, int), char> e in east) {
                    (int, int) key = e.Key;
                    (int, int) next = ((key.Item1 + 1) % limitX, key.Item2);
                    if (!unmoved.ContainsKey(next) && !movedE.ContainsKey(next)) {
                            movedE[next] = '>';
                            movedS[next] = '>';
                            countMoved++;
                    } else {
                        movedE[key] = '>';
                        movedS[key] = '>';
                    }
                }

                foreach (KeyValuePair<(int, int), char> s in south) {
                    (int, int) key = s.Key;
                    (int, int) next = ((key.Item1, (key.Item2 + 1) % limitY));
                    if(!movedE.ContainsKey(next) && !movedS.ContainsKey(next)) {
                        movedS[next] = 'v';
                        countMoved++;
                    } else
                        movedS[key] = 'v';
                }

                if (drawAll)
                    Draw(string.Format("After step {0}:", steps), seaCucumbers, limitX, limitY);

                if (countMoved == 0)
                    break;
                seaCucumbers = movedS;
            }

            if(!drawAll)
                Draw(string.Format("After step {0}:", steps), seaCucumbers, limitX, limitY);

            Console.WriteLine("Part 1: " + steps);
            //Answer: 378
            Console.WriteLine("Part 2: (N/A)");
			//Answer: (there is no Part 2).
        }

        private static void Draw(string heading, Dictionary<(int, int), char> nodes, int limitX, int limitY) {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(heading);
            Console.ResetColor();

            if (Console.WindowWidth < limitX) {
                Console.WriteLine("Unable to fit preview.\r\n");
                return;
            }

            for (int y = 0; y < limitY; y++) {
                for (int x = 0; x < limitX; x++) {
                    (int, int) key = (x, y);
                    if (nodes.ContainsKey(key)) {
                        char c = nodes[key];
                        Console.Write(c);
                    } else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
