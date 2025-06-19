namespace AoC.Day
{
    public class Day22
    {
        //Part 2 based on approach of u/4HbQ and u/Boojum

        public static void Run(string file) {
            Console.WriteLine("Day 22: Reactor Reboot" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            long partA = Solve(lines, true);
            long partB = Solve(lines, false);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 587785
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1167985679908143
        }

        private class Cuboid {
            public long X_Start;
            public long X_End;
            public long Y_Start;
            public long Y_End;
            public long Z_Start;
            public long Z_End;

            public long Calc;
            public int Multi;

            public Cuboid(long xStart, long xEnd, long yStart, long yEnd, long zStart, long zEnd) {
                X_Start = xStart;
                X_End = xEnd;
                Y_Start = yStart;
                Y_End = yEnd;
                Z_Start = zStart;
                Z_End = zEnd;

                Calc = (X_End - X_Start + 1) * (Y_End - Y_Start + 1) * (Z_End - Z_Start + 1);
                Multi = 1;
            }

            public override string ToString() {
                return string.Format("{0},{1},{2} to {3},{4},{5} [C:{6}, M:{7}]", X_Start, Y_Start, Z_Start, X_End, Y_End, Z_End, Calc, Multi);
            }
        }

        private static Cuboid? GetIntersect(Cuboid cuboid, Cuboid other) {
            long xStart = Math.Max(cuboid.X_Start, other.X_Start);
            long yStart = Math.Max(cuboid.Y_Start, other.Y_Start);
            long zStart = Math.Max(cuboid.Z_Start, other.Z_Start);

            long xEnd = Math.Min(cuboid.X_End, other.X_End);
            long yEnd = Math.Min(cuboid.Y_End, other.Y_End);
            long zEnd = Math.Min(cuboid.Z_End, other.Z_End);

            if (xStart <= xEnd && yStart <= yEnd && zStart <= zEnd)
                return new Cuboid(xStart, xEnd, yStart, yEnd, zStart, zEnd);
            return null;
        }

        private static long Solve(string[] lines, bool partA) {
            var rangeA = Enumerable.Range(-50, 101);

            List<Cuboid> cuboids = new List<Cuboid>();

            foreach (string line in lines) {
                string[] parts = line.Replace('=', ',').Replace("..", ",").Split(',');
                int xStart = int.Parse(parts[1]);
                int xEnd = int.Parse(parts[2]);
                int yStart = int.Parse(parts[4]);
                int yEnd = int.Parse(parts[5]);
                int zStart = int.Parse(parts[7]);
                int zEnd = int.Parse(parts[8]);
                Cuboid cuboid = new Cuboid(xStart, xEnd, yStart, yEnd, zStart, zEnd);

                if(partA) {
                    if (!rangeA.Contains(xStart) || !rangeA.Contains(xEnd) ||
                        !rangeA.Contains(yStart) || !rangeA.Contains(yEnd) ||
                        !rangeA.Contains(zStart) || !rangeA.Contains(zEnd)
                    ) continue;
                }

                List<Cuboid> others = cuboids.ToList();
                foreach (Cuboid other in others) {
                    Cuboid? intersect = GetIntersect(cuboid, other);
                    if (intersect != null) {
                        if (cuboids.Contains(intersect)) {
                            intersect.Multi -= other.Multi;
                        } else {
                            cuboids.Add(intersect);
                            intersect.Multi = other.Multi * -1;
                        }
                    }
                }

                if (line.StartsWith("on"))
                    cuboids.Add(cuboid);
            }

            return cuboids.Sum(c => c.Calc * c.Multi);
        }
    }
}
