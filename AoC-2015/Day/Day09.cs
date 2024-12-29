using System;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace AoC.Day
{
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: All in a Single Night" + Environment.NewLine);

            List<string> places = new List<string>();
            List<Tuple<string, string, int>> list = new List<Tuple<string, string, int>>();

            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                string[] fields = line.Split(' ');
                list.Add(new Tuple<string, string, int>(fields[0], fields[2], int.Parse(fields[4])));
                list.Add(new Tuple<string, string, int>(fields[2], fields[0], int.Parse(fields[4])));

                if (!places.Contains(fields[0]))
                    places.Add(fields[0]);
                if (!places.Contains(fields[2]))
                    places.Add(fields[2]);
            }

            /*
            int factorial = 1;
            for (int x = 1; x <= places.Count; x++) {
                factorial *= x;
            }
            Console.WriteLine(factorial);
            */

            string bestLow = string.Empty;
            string bestHigh = string.Empty;
            int distanceHighest = list.OrderBy(x => x.Item3).Last().Item3;
            int distanceLowest = distanceHighest * places.Count;
            distanceHighest = 0; //For part 2

            StringBuilder sbLowest = new StringBuilder();
            for (int k = 0; k < places.Count; k++) {
                sbLowest.Append(k);
            }
            int lowest = int.Parse(sbLowest.ToString());
            int highest = int.Parse(new string(sbLowest.ToString().Reverse().ToArray()));

            List<string> options = new List<string>();

            int numPlaces = places.Count;
            for (int i = lowest; i <= highest; i++)
            {
                string t = i.ToString().PadLeft(numPlaces, '0');
                bool valid = true;
                for (int k = 0; k < numPlaces; k++) {
                    if (!t.Contains(k.ToString())) {
                        valid = false;
                        break;
                    }
                }
                if (valid) {
                    options.Add(t);
                    int dist = 0;
                    int idFirst = int.Parse(t[0].ToString());
                    string nameFirst = places[idFirst];
                    for (int k = 1; k < numPlaces; k++) {
                        int idSecond = int.Parse(t[k].ToString());
                        string nameSecond = places[idSecond];

                        dist += list.Find(x => x.Item1 == nameFirst && x.Item2 == nameSecond).Item3;

                        idFirst = idSecond;
                        nameFirst = nameSecond;
                    }
                    if (dist < distanceLowest) {
                        distanceLowest = dist;
                        bestLow = t;
                    }
                    if (dist > distanceHighest) {
                        distanceHighest = dist;
                        bestHigh = t;
                    }
                }
            }

            Console.WriteLine("Part 1: " + distanceLowest);
            //Answer: 141
            Console.WriteLine("Part 2: " + distanceHighest);
            //Answer: 736
        }

    }
}
