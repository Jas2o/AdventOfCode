using AoC.Shared;

namespace AoC.Day {
    public class Day13
    {
        public static void Run(string file) {
            Console.WriteLine("Day 13: Knights of the Dinner Table" + Environment.NewLine);

            Dictionary<string, char> people = new Dictionary<string, char>();
            Dictionary<char, string> peopleR = new Dictionary<char, string>();
            Dictionary<string, int> peoplePoints = new Dictionary<string, int>();

            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                string[] fields = line.Split(' ');
                string p1 = fields[0];
                string p2 = fields[10].Replace(".", "");

                if (!people.ContainsKey(p1)) {
                    char c = people.Count().ToString("X")[0];
                    people.Add(p1, c);
                    peopleR.Add(c, p1);
                }
                if (!people.ContainsKey(p2)) {
                    char c = people.Count().ToString("X")[0];
                    people.Add(p2, c);
                    peopleR.Add(c, p2);
                }

                int points = int.Parse(fields[3]);
                if (fields[2] == "lose")
                    points *= -1;

                peoplePoints.Add(string.Format("{0}:{1}", p1, p2), points);
            }

            //--

            int partA = MaxHappiness(ref people, ref peopleR, ref peoplePoints, true);

            Console.WriteLine();
            Console.WriteLine("* this might take a little while *");

            string me = "Jas2o";
            char meID = (char)((peopleR.Last()).Key + 1);
            foreach (KeyValuePair<string, char> other in people) {
                peoplePoints.Add(string.Format("{0}:{1}", other.Key, me), 0);
                peoplePoints.Add(string.Format("{0}:{1}", me, other.Key), 0);
            }
            people.Add(me, meID);            
            peopleR.Add(meID, me);

            int partB = MaxHappiness(ref people, ref peopleR, ref peoplePoints, false);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 709
            Console.WriteLine("Part 2: " + partB);
            //Answer: 668
        }

        public static int MaxHappiness(ref Dictionary<string, char> people, ref Dictionary<char, string> peopleR, ref Dictionary<string, int> peoplePoints, bool verbose) {
            int maxHappiness = 0;
            Dictionary<string, int> arrangement = new Dictionary<string, int>();

            int bitplaces = people.Count;
            int possible = (int)Math.Pow(people.Count, bitplaces);
            for (int ops = 0; ops < possible; ops++) {
                string mask = Number.IntToString(ops, people.Count).PadLeft(bitplaces, '0');

                bool valid = true;
                string[] chain = new string[bitplaces + 1];
                for (int y = 0; y < bitplaces; y++) {
                    char c = mask[y];
                    if (mask.Count(x => x == mask[y]) > 1) {
                        valid = false;
                        break;
                    } else {
                        string p = peopleR[c];
                        chain[y] = peopleR[c];
                    }
                }

                if (valid) {
                    chain[bitplaces] = chain[0];
                    arrangement.Add(mask, 0);
                    if (verbose) {
                        //Console.WriteLine(mask);
                        Console.Write(string.Join(':', chain));
                    }
                    int happiness = 0;
                    for (int y = 0; y < bitplaces; y++) {
                        string key1 = string.Format("{0}:{1}", chain[y], chain[y + 1]);
                        string key2 = string.Format("{0}:{1}", chain[y + 1], chain[y]);
                        happiness += peoplePoints[key1];
                        happiness += peoplePoints[key2];
                    }

                    if (verbose) {
                        Console.Write(" = " + happiness);
                        Console.WriteLine();
                    }

                    if (happiness > maxHappiness)
                        maxHappiness = happiness;
                }

                if (mask[0] != '0') {
                    //We only need to check one loop of seating arrangements
                    break;
                }
            }

            return maxHappiness;
        }

    }
}
