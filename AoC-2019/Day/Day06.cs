namespace AoC.Day
{
    public class Day06
    {
        public static void Run(string file) {
            Console.WriteLine("Day 6: Universal Orbit Map" + Environment.NewLine);

            Dictionary<string, UniverseObject> all = new Dictionary<string, UniverseObject>();
            //List<UniverseObject> all = new List<UniverseObject>();
            UniverseObject com = new UniverseObject("COM");
            all["COM"] = com;

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] fields = line.Split(')');

                UniverseObject left = null;
                if (all.ContainsKey(fields[0]))
                    left = all[fields[0]];
                else {
                    left = new UniverseObject(fields[0]);
                    all.Add(fields[0], left);
                }

                UniverseObject right = null;
                if (all.ContainsKey(fields[1]))
                    right = all[fields[1]];
                else {
                    right = new UniverseObject(fields[1]);
                    all.Add(fields[1], right);
                }

                left.Children.Add(right);
                right.Parent = left;
            }

            int partA = 0;
            Recursive(com, 0, ref partA);

            int partB = -1;
            string partB_info = string.Empty;
            all.TryGetValue("YOU", out UniverseObject you);
            all.TryGetValue("SAN", out UniverseObject san);
            if(you != null && san != null) {
                partB = 0;
                List<string> pathYou = new List<string>();
                you = you.Parent;
                while(you != null) {
                    pathYou.Add(you.Name);
                    you = you.Parent;
                }

                List<string> pathSan = new List<string>();
                san = san.Parent;
                while (san != null) {
                    pathSan.Add(san.Name);
                    san = san.Parent;
                }

                IEnumerable<string> pathCommon = pathYou.Intersect(pathSan);
                //Console.WriteLine(string.Join(" - ", pathYou));
                //Console.WriteLine(string.Join(" - ", pathSan));
                //Console.WriteLine(string.Join(" - ", pathCommon));

                int countCommon = pathCommon.Count();
                int countYou = pathYou.Count() - countCommon;
                int countSan = pathSan.Count() - countCommon;
                partB = countYou + countSan;
                partB_info = string.Format("({0} + {1})", countYou, countSan);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 110190
            Console.WriteLine("Part 2: {0} {1}", partB, partB_info);
            //Answer: 343
        }

        private static void Recursive(UniverseObject from, int offset, ref int count) {
            foreach (UniverseObject child in from.Children) {
                if (child.Name == "YOU")
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (child.Name == "SAN")
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{2}{0}:{1}", from.Name, child.Name, " ".PadLeft(offset+1));
                Console.ResetColor();

                count += offset + 1;
                Recursive(child, offset+1, ref count);
            }
        }

        private class UniverseObject {
            public string Name;
            public UniverseObject? Parent;
            public List<UniverseObject> Children;

            public UniverseObject(string name) {
                Name = name;
                Children = new List<UniverseObject>();
            }
        }
    }
}
