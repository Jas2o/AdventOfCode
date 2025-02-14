namespace AoC.Day
{
    public class Day10
    {
        public static void Run(string file) {
            Console.WriteLine("Day 10: Monitoring Station" + Environment.NewLine);
            
            Dictionary<(int, int), Asteroid> asteroids = new Dictionary<(int, int), Asteroid>();
            string[] lines = File.ReadAllLines(file);
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    if(lines[y][x] != '.')
                        asteroids.Add((x, y), new Asteroid(x, y));
                }
            }

            foreach (KeyValuePair<(int, int), Asteroid> main in asteroids) {
                Asteroid m = main.Value;
                Dictionary<(int, int), bool> factors = new Dictionary<(int, int), bool>();
                foreach (KeyValuePair<(int, int), Asteroid> other in asteroids) {
                    if (other.Key == main.Key)
                        continue;
                    
                    Asteroid o = other.Value;
                    int manhattan = Math.Abs(o.X - m.X) + Math.Abs(o.Y - m.Y);
                    double angle = Math.Atan2(o.Y - m.Y, o.X - m.X) * (180d / Math.PI);
                    if (angle < 0d)
                        angle += 360d;

                    int dx = o.X - m.X;
                    int dy = o.Y - m.Y;
                    int gcd = GCD(dx, dy);
                    dx /= gcd;
                    dy /= gcd;

                    bool add = factors.TryAdd((dx, dy), true);
                    if (add)
                        m.InLine[angle] = new Dictionary<int, Asteroid>();
                    m.InLine[angle].Add(manhattan, o);
                }
                m.LineOfSight = factors.Count;
                Console.WriteLine("{0},{1} can see {2}", m.X, m.Y, m.LineOfSight);
            }

            //Part 1
            Asteroid best = asteroids.MaxBy(a => a.Value.LineOfSight).Value;
            Console.WriteLine("\r\nStation deployed to: {0},{1}", best.X, best.Y);

            //Part 2
            best.InLine = best.InLine.OrderBy(i => i.Key).ToDictionary();
            int vaporise_limit = 200;
            if (asteroids.Count < vaporise_limit)
                vaporise_limit = asteroids.Count - 2;

            int partB = -1;
            Asteroid partB_asteroid = best; //Just to make sure it's not null.
            int vaporised = 0;
            bool laserOn = false;
            while (vaporised < vaporise_limit) {
                foreach (KeyValuePair<double, Dictionary<int, Asteroid>> pair in best.InLine) {
                    if (!laserOn && pair.Key == 270) {
                        //Assumes there's always at least one at this position
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Activating the laser!\r\n");
                        Console.ResetColor();
                        laserOn = true;
                    }

                    if(laserOn && pair.Value.Count > 0) {
                        KeyValuePair<int, Asteroid> toBlowUp = pair.Value.MinBy(o => o.Key);
                        best.InLine[pair.Key].Remove(toBlowUp.Key);
                        vaporised++;

                        Console.WriteLine("#{0} at {1},{2}", vaporised, toBlowUp.Value.X, toBlowUp.Value.Y);
                        if (vaporised == vaporise_limit) {
                            partB_asteroid = toBlowUp.Value;
                            partB = partB_asteroid.X * 100 + partB_asteroid.Y;
                            break;
                        }
                    }
                }

                if (vaporised == 0) {
                    Console.WriteLine("Nothing got vaporised...");
                    break;
                } else if (vaporised == vaporise_limit) {
                    //The laser never got turned off and might continue to vaporise anything that drifts into its path...
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: {0} ({1},{2})", best.LineOfSight, best.X, best.Y);
            //Answer: 227 (11,13)
            Console.WriteLine("Part 2: {0} ({1},{2})", partB, partB_asteroid.X, partB_asteroid.Y);
            //Answer: 604 (6,4)
        }

        //Greatest Common Divisor aka Highest Common Factor
        private static int GCD(int a, int b) {
            return (int)GCD((uint)Math.Abs(a), (uint)Math.Abs(b));
        }

        private static uint GCD(uint a, uint b) {
            while (a != 0 && b != 0) {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        private class Asteroid {
            public int X;
            public int Y;
            public int LineOfSight;
            public Dictionary<double, Dictionary<int, Asteroid>> InLine;

            public Asteroid(int x, int y) {
                X = x;
                Y = y;
                LineOfSight = 0;
                InLine = new Dictionary<double, Dictionary<int, Asteroid>>();
            }
        }
    }
}
