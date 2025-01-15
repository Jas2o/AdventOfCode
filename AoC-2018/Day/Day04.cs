namespace AoC.Day {
    public class Day04
    {
        public static void Run(string file) {
            Console.WriteLine("Day 4: Repose Record" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            Array.Sort(lines);

            Dictionary<int, List<(DateTime, DateTime)>> guards = new Dictionary<int, List<(DateTime, DateTime)>>();
            if (true) {
                int lastGuard = 0;
                DateTime dtSleep = DateTime.MinValue;
                DateTime dtWake = DateTime.MinValue;
                foreach (string line in lines) {
                    DateTime dt = DateTime.Parse(line.Substring(1, 16));
                    int indGuardNum = line.IndexOf('#');
                    bool fallsAsleep = line.Contains("falls");
                    bool wakesUp = line.Contains("wakes");

                    if (indGuardNum > -1) {
                        lastGuard = int.Parse(line.Substring(indGuardNum + 1).Split(' ').First());
                        if (!guards.ContainsKey(lastGuard))
                            guards[lastGuard] = new List<(DateTime, DateTime)>();
                    } else if (fallsAsleep) {
                        dtSleep = dt;
                    } else if (wakesUp) {
                        dtWake = dt;
                        guards[lastGuard].Add((dtSleep, dtWake));
                    }
                }
            }

            TimeSpan aSingleMinute = TimeSpan.FromMinutes(1);

            int part1_SleepyGuard = -1;
            int part1_SleepyMinutesTotal = -1;
            int part1_SleepyMinute = -1;

            int part2_SleepyGuard = -1;
            int part2_SleepyMinutesTotal = -1;
            int part2_SleepyMinute = -1;

            foreach (KeyValuePair<int, List<(DateTime, DateTime)>> guard in guards) {
                if (guard.Value.Count == 0)
                    continue;

                Dictionary<int, int> dMinTimes = new Dictionary<int, int>();
                TimeSpan earliestSleep = guard.Value.Min(x => x.Item1.TimeOfDay);
                TimeSpan latestAwake = guard.Value.Max(x => x.Item2.TimeOfDay);
                for (TimeSpan now = earliestSleep; now < latestAwake; now = now.Add(aSingleMinute)) {
                    dMinTimes[now.Minutes] = 0;
                    foreach (var dt in guard.Value) {
                        if (now >= dt.Item1.TimeOfDay && now < dt.Item2.TimeOfDay)
                            dMinTimes[now.Minutes]++;
                    }
                }

                int sleepMinutesTotal = (int)guard.Value.Sum(x => (x.Item2 - x.Item1).TotalMinutes);
                KeyValuePair<int,int> sleepyMinute = dMinTimes.MaxBy(x => x.Value);

                if (sleepMinutesTotal > part1_SleepyMinutesTotal) {
                    part1_SleepyGuard = guard.Key;
                    part1_SleepyMinutesTotal = sleepMinutesTotal;
                    part1_SleepyMinute = sleepyMinute.Key;
                }

                if (sleepyMinute.Value > part2_SleepyMinutesTotal) {
                    part2_SleepyGuard = guard.Key;
                    part2_SleepyMinutesTotal = sleepyMinute.Value;
                    part2_SleepyMinute = sleepyMinute.Key;
                }
            }
            int partA = part1_SleepyGuard * part1_SleepyMinute;
            int partB = part2_SleepyGuard * part2_SleepyMinute;

            Console.WriteLine("Part 1: {0} (guard #{1} at minute {2})", partA, part1_SleepyGuard, part1_SleepyMinute);
            //Answer: 35623
            Console.WriteLine("Part 2: {0} (guard #{1} at minute {2})", partB, part2_SleepyGuard, part2_SleepyMinute);
            //Answer: 23037
        }
    }
}
