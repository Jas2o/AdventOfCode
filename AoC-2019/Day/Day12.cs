namespace AoC.Day {
    public class Day12
    {
        private static char[] separators = ['<', '=', ',', '>'];

        public static void Run(string file) {
            Console.WriteLine("Day 12: The N-Body Problem" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            if(lines.Length < 4) {
                Console.WriteLine("The input file does not contain 4 lines.");
                return;
            }

            List<Moon> moons = new List<Moon>();
            for (int i = 0; i < 4; i++) {
                string[] fields = lines[i].Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                moons.Add(new Moon(fields));
            }
            bool verbose = false;
            uint timeLast = 1000;
            if (lines.Length > 4) {
                //I would leave "Test" on the 5th line as a reminder when I have both text files open.
                if (moons[0].PosX == -1) {
                    verbose = true;
                    timeLast = 10;
                } else if (moons[0].PosX == -8)
                    timeLast = 100;
            }

            ulong time = 0;
            for (; time <= timeLast; time++) {
                Sim(moons, time, verbose);
            }

            int partA = moons.Sum(m => m.Pot * m.Kin);

            ulong partB_X = 0, partB_Y = 0, partB_Z = 0;
            while(partB_X == 0 || partB_Y == 0 || partB_Z == 0) {
                Sim(moons, time, false);

                bool validateX = true, validateY = true, validateZ = true;
                foreach (Moon moon in moons) {
                    if(!moon.IsInitialX) validateX = false;
                    if (!moon.IsInitialY) validateY = false;
                    if (!moon.IsInitialZ) validateZ = false;
                }

                if (validateX && partB_X == 0) partB_X = time;
                if (validateY && partB_Y == 0) partB_Y = time;
                if (validateZ && partB_Z == 0) partB_Z = time;

                time++;
            }

            ulong partB_XY = (partB_X * partB_Y) / GCD(partB_X, partB_Y);
            ulong partB = (partB_XY * partB_Z) / GCD(partB_XY, partB_Z);

            Console.WriteLine("Part 1: {0}", partA);
            //Answer: 9493
            Console.WriteLine("Part 2: {0} (x:{1}, y:{2}, z:{3})", partB, partB_X, partB_Y, partB_Z);
            //Answer: 326365108375488
        }

        private static int Pos2toV(int one, int two) {
            if (one == two)
                return 0;
            else if (one > two)
                return -1;
            else
                return 1;
        }

        //Greatest Common Divisor aka Highest Common Factor
        private static ulong GCD(ulong a, ulong b) {
            while (a != 0 && b != 0) {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        private static void Sim(List<Moon> moons, ulong time, bool verbose) {
            if (verbose)
                Console.WriteLine("After {0} step{1}:", time, (time == 1 ? "" : 's'));

            if (time > 0) {
                foreach (Moon moon in moons) {
                    foreach (Moon other in moons) {
                        if (other == moon)
                            continue;
                        moon.VelocityX += Pos2toV(moon.PosX, other.PosX);
                        moon.VelocityY += Pos2toV(moon.PosY, other.PosY);
                        moon.VelocityZ += Pos2toV(moon.PosZ, other.PosZ);
                    }
                }
            }

            foreach (Moon moon in moons) {
                moon.PosX += moon.VelocityX;
                moon.PosY += moon.VelocityY;
                moon.PosZ += moon.VelocityZ;
                if (verbose)
                    Console.WriteLine(moon.ToString());
            }

            if (verbose)
                Console.WriteLine();
        }

        private class Moon {
            public int InitialPosX;
            public int InitialPosY;
            public int InitialPosZ;
            public int PosX;
            public int PosY;
            public int PosZ;
            public int VelocityX;
            public int VelocityY;
            public int VelocityZ;

            public Moon(string[] fields) {
                PosX = InitialPosX = int.Parse(fields[1]);
                PosY = InitialPosY = int.Parse(fields[3]);
                PosZ = InitialPosZ = int.Parse(fields[5]);
                VelocityX = 0;
                VelocityY = 0;
                VelocityZ = 0;
            }

            public int Pot {
                get { return Math.Abs(PosX) + Math.Abs(PosY) + Math.Abs(PosZ); }
            }

            public int Kin {
                get { return Math.Abs(VelocityX) + Math.Abs(VelocityY) + Math.Abs(VelocityZ); }
            }

            public bool IsInitialX {
                get { return (PosX == InitialPosX && VelocityX == 0); }
            }

            public bool IsInitialY {
                get { return (PosY == InitialPosY && VelocityY == 0); }
            }

            public bool IsInitialZ {
                get { return (PosZ == InitialPosZ && VelocityZ == 0); }
            }

            public override string ToString() {
                return string.Format("pos=<x= {0}, y= {1}, z= {2}>, vel=<x= {3}, y= {4}, z= {5}>", PosX, PosY, PosZ, VelocityX, VelocityY, VelocityZ);
            }
        }
    }
}
