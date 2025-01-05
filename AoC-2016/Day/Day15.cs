namespace AoC.Day
{
    public class Day15
    {
        public static void Run(string file) {
            Console.WriteLine("Day 15: Timing is Everything" + Environment.NewLine);

            List<Disc> listDisc = new List<Disc>();
            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                Disc d = new Disc(line);
                listDisc.Add(d);
            }

            int startTimeA = Solve(listDisc, 0);
            listDisc.Add(new Disc(11, 0));
            int startTimeB = Solve(listDisc, startTimeA);

            Console.WriteLine("Part 1: " + startTimeA);
            //Answer: 317371
            Console.WriteLine("Part 2: " + startTimeB);
            //Answer: 2080951
        }

        private static int Solve(List<Disc> listDisc, int startAt) {
            int startTime = startAt;
            while (true) {
                int timeNow = startTime;
                bool success = true;
                foreach (Disc d in listDisc) {
                    timeNow++;
                    int math = (d.Offset + timeNow) % d.Positions;
                    if (math != 0) {
                        success = false;
                        break;
                    }
                }
                if (success)
                    return startTime;
                startTime++;
            }
        }

        private class Disc {
            //public int ID;
            public int Positions;
            public int Offset;

            public Disc(string input) {
                string[] fields = input.Replace(".", "").Split(' ');
                //ID = int.Parse(fields[1].Substring(1));
                Positions = int.Parse(fields[3]);
                Offset = int.Parse(fields[11]);
            }

            public Disc(int positions, int offset) {
                Positions = positions;
                Offset = offset;
            }
        }
    }
}
