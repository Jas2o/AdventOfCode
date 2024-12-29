using System;
using System.Globalization;
using System.Text;

namespace AoC.Day
{
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: Perfectly Spherical Houses in a Vacuum" + Environment.NewLine);

            List<string> listDeliver1 = new List<string>();
            List<string> listDeliver2 = new List<string>();

            int[] pos0 = { 0, 0 }; //Part 1
            int[] pos1 = { 0, 0 }; //Part 2
            int[] pos2 = { 0, 0 };
            bool swap = false;

            listDeliver1.Add(string.Format("{0},{1}", pos0[0], pos0[1])); //Part 1
            listDeliver2.Add(string.Format("{0},{1}", pos1[0], pos1[1])); //Part 2
            listDeliver2.Add(string.Format("{0},{1}", pos2[0], pos2[1]));

            string text = File.ReadAllText(file);
            for (int i = 0; i < text.Length; i++) {
                char c = text[i];

                if (c == '^') {
                    pos0[1]++;
                    if (swap)
                        pos2[1]++;
                    else
                        pos1[1]++;
                } else if (c == '>') {
                    pos0[0]++;
                    if (swap)
                        pos2[0]++;
                    else
                        pos1[0]++;
                } else if (c == 'v') {
                    pos0[1]--;
                    if (swap)
                        pos2[1]--;
                    else
                        pos1[1]--;
                } else if (c == '<') {
                    pos0[0]--;
                    if (swap)
                        pos2[0]--;
                    else
                        pos1[0]--;
                }

                listDeliver1.Add(string.Format("{0},{1}", pos0[0], pos0[1]));
                listDeliver2.Add(string.Format("{0},{1}", pos1[0], pos1[1]));
                listDeliver2.Add(string.Format("{0},{1}", pos2[0], pos2[1]));
                swap = !swap;
            }

            int part1 = listDeliver1.Distinct().Count();
            int part2 = listDeliver2.Distinct().Count();

            Console.WriteLine("Part 1: " + part1);
            //Answer: 2565
            Console.WriteLine("Part 2: " + part2);
            //Answer: 2639
        }
    }
}
