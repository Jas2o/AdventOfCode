namespace AoC.Day
{
    public class Day23
    {
        //My thanks goes to u/Dioxy for freeing me from Part 2.
        //No idea if this works for inputs other than what I was given.

        private const int X = 0, Y = 1, Z = 2;

        public static void Run(string file) {
            Console.WriteLine("Day 23: Experimental Emergency Teleportation" + Environment.NewLine);

            List<Nanobot> nanobots = new List<Nanobot>();
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                //Kinda dumb but was copied from 2017 Day 10
                string pos = line.Split('<').Skip(1).First();
                pos = pos.Split('>').First();
                int[] p = Array.ConvertAll(pos.Split(','), int.Parse);
                int r = int.Parse(line.Substring(line.IndexOf("r=") + 2));
                Nanobot n = new Nanobot(nanobots.Count + 1, p, r);
                nanobots.Add(n);
            }

            //Part 1
            int partA = 0;
            Nanobot strongest = nanobots.MaxBy(n => n.Radius);
            foreach(Nanobot n in nanobots) {
                int manhattan = Math.Abs(strongest.Position[X] - n.Position[X]) + Math.Abs(strongest.Position[Y] - n.Position[Y]) + Math.Abs(strongest.Position[Z] - n.Position[Z]);
                if (manhattan <= strongest.Radius)
                    partA++;
            }

            //Part 2
            int minX = nanobots.Min(n => n.Position[X]);
            int minY = nanobots.Min(n => n.Position[Y]);
            int minZ = nanobots.Min(n => n.Position[Z]);
            (int x, int y, int z) start = (minX, minY, minZ);

            int maxX = nanobots.Max(n => n.Position[X]);
            int maxY = nanobots.Max(n => n.Position[Y]);
            int maxZ = nanobots.Max(n => n.Position[Z]);
            (int x, int y, int z) end = (maxX, maxY, maxZ);

            int tenMillion = 10_000_000;
            int factor = (maxX > tenMillion || maxY > tenMillion || maxZ > tenMillion) ? tenMillion : 1;
            (int x, int y, int z) best = (0, 0, 0);

            Dictionary<(int x, int y, int z), int> coverage = new Dictionary<(int x, int y, int z), int>();
            while (factor >= 1) {
                coverage.Clear();

                for (int x = start.x; x <= end.x; x += factor) {
                    for (int y = start.y; y <= end.y; y += factor) {
                        for (int z = start.z; z <= end.z; z += factor) {
                            int count = 0;
                            foreach (Nanobot n in nanobots) {
                                int manhattan = Math.Abs(n.Position[X] - x) + Math.Abs(n.Position[Y] - y) + Math.Abs(n.Position[Z] - z);
                                if (manhattan <= n.Radius)
                                    count++;
                            }
                            if(count > 0)
                                coverage.Add((x, y, z), count);
                        }
                    }
                }

                best = coverage.MaxBy(c => c.Value).Key;
                factor /= 10;
                start.x = best.x - (factor * 10);
                start.y = best.y - (factor * 10);
                start.z = best.z - (factor * 10);
                end.x = best.x + (factor * 10);
                end.y = best.y + (factor * 10);
                end.z = best.z + (factor * 10);
            }

            int partB = Math.Abs(best.x) + Math.Abs(best.y) + Math.Abs(best.z);
            
            Console.WriteLine("Best spot at {0}\r\n", best);
            Console.WriteLine("Part 1: " + partA);
            //Answer: 588
            Console.WriteLine("Part 2: " + partB);
            //Answer: 111227643 at (37741725, 23367434, 50118484)
        }

        private class Nanobot {
            public int Num;
            public int[] Position;
            public int Radius;

            public Nanobot(int num, int[] p, int r) {
                Num = num;
                Position = p;
                Radius = r;
            }

            public override string ToString() {
                return string.Format("#{0} : {1},{2},{3} : r={4}", Num, Position[X], Position[Y], Position[Z], Radius);
            }
        }
    }
}
