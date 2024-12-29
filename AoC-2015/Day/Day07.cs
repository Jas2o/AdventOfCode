using System;
using System.Globalization;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace AoC.Day
{
    public class Day07
    {
        public static void Run(string file) {
            Console.WriteLine("Day 7: Some Assembly Required" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            Queue<string> queue = new Queue<string>();
            Dictionary<string, string> sorted = new Dictionary<string, string>();
            foreach (string line in lines) {
                queue.Enqueue(line);
            }

            //Sort them
            while (queue.Any()) {
                bool resolved = false;
                string line = queue.Dequeue();
                string[] fields = line.Split(' ');
                string wire = fields.Last();

                if (fields.Length == 3) {
                    bool left = ushort.TryParse(fields[0], out ushort nleft);
                    if (!left && sorted.ContainsKey(fields[0]))
                        left = true;
                    if (left) {
                        sorted.Add(wire, line);
                        resolved = true;
                    }
                } else if (fields[0] == "NOT") {
                    bool left = ushort.TryParse(fields[1], out ushort nleft);
                    if (!left && sorted.ContainsKey(fields[1]))
                        left = true;
                    if (left) {
                        sorted.Add(wire, line);
                        resolved = true;
                    }
                } else {
                    bool left = ushort.TryParse(fields[0], out ushort nleft);
                    bool right = ushort.TryParse(fields[2], out ushort nright);
                    if (!left && sorted.ContainsKey(fields[0]))
                        left = true;
                    if (!right && sorted.ContainsKey(fields[2]))
                        right = true;
                    if (left && right) {
                        sorted.Add(wire, line);
                        resolved = true;
                    }
                }

                if (!resolved)
                    queue.Enqueue(line);
            }

            //Resolve them
            Dictionary<string, ushort> values = new Dictionary<string, ushort>();
            ResolveWires(sorted, ref values);
            int partA = values["a"];

            //For Part 2, we replace "b" with the value we got from "a", then recalculate for the new "a"
            sorted["b"] = string.Format("{0} -> b", partA);
            Dictionary<string, ushort> values2 = new Dictionary<string, ushort>();
            ResolveWires(sorted, ref values2);
            int partB = values2["a"];

            Console.WriteLine("Part 1: " + partA);
            //Answer: 3176
            Console.WriteLine("Part 2: " + partB);
            //Answer: 14710
        }

        private static void ResolveWires(Dictionary<string, string> sorted, ref Dictionary<string, ushort> values) {
            foreach (KeyValuePair<string, string> pair in sorted) {
                string line = pair.Value;
                string[] fields = line.Split(' ');
                string wire = fields.Last();

                if (fields.Length == 3) {
                    bool left = ushort.TryParse(fields[0], out ushort nleft);
                    if (!left)
                        nleft = values[fields[0]];

                    values.Add(wire, nleft);
                } else if (fields[0] == "NOT") {
                    bool left = ushort.TryParse(fields[1], out ushort nleft);
                    if (!left)
                        nleft = values[fields[1]];

                    ushort value = (ushort)(~nleft);
                    values.Add(wire, value);
                } else {
                    bool left = ushort.TryParse(fields[0], out ushort nleft);
                    bool right = ushort.TryParse(fields[2], out ushort nright);

                    if (!left)
                        nleft = values[fields[0]];
                    if (!right)
                        nright = values[fields[2]];

                    if (fields[1] == "AND") {
                        ushort value = (ushort)(nleft & nright);
                        values.Add(wire, value);
                    } else if (fields[1] == "OR") {
                        ushort value = (ushort)(nleft | nright);
                        values.Add(wire, value);
                    } else if (fields[1] == "LSHIFT") {
                        ushort value = (ushort)(nleft << nright);
                        values.Add(wire, value);
                    } else if (fields[1] == "RSHIFT") {
                        ushort value = (ushort)(nleft >> nright);
                        values.Add(wire, value);
                    }
                }
            }
        }
    }
}
