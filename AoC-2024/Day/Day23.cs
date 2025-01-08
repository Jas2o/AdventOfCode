using AoC.Graph;

namespace AoC.Day
{
    public class Day23
    {
        //I'm not happy with Part 1 running slower than Part 2.
        //Wonder if possible to use Part 2's cliques list to get the answer for Part 1, or run it again with a size limit.

        public static void Run(string file) {
            Console.WriteLine("Day 23: LAN Party" + Environment.NewLine);

            Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] parts = line.Split('-');

                if (graph.ContainsKey(parts[0]))
                    graph[parts[0]].Add(parts[1]);
                else
                    graph.Add(parts[0], new List<string>() { parts[1] });

                if (graph.ContainsKey(parts[1]))
                    graph[parts[1]].Add(parts[0]);
                else
                    graph.Add(parts[1], new List<string>() { parts[0] });
            }

            //Really slow
            Console.WriteLine("Working on Part 1...");
            List<List<string>> listLAN = new List<List<string>>();
            foreach (KeyValuePair<string, List<string>> comp0 in graph) {
                foreach (string comp1 in comp0.Value) {
                    List<string> conn1 = graph[comp1];
                    foreach (string comp2 in conn1) {
                        List<string> conn2 = graph[comp2];
                        if (conn2.Contains(comp0.Key)) {
                            List<string> lan = new List<string>() { comp0.Key, comp1, comp2 };
                            lan.Sort();
                            if (!listLAN.Any(list => list.SequenceEqual(lan)))
                                listLAN.Add(lan);
                        }
                    }
                }
            }

            int countT = 0;
            foreach (List<string> lan in listLAN) {
                if (lan.Any(x => x.StartsWith('t'))) {
                    countT++;
                    //Console.WriteLine(string.Join(',', lan));
                }
            }

            //Part 2
            Console.WriteLine("Working on Part 2...");
            List<List<string>> cliques = new List<List<string>>();
            BronKerbosch.GetCliques(graph, cliques, new List<string>(graph.Keys));

            List<string> biggest = cliques.MaxBy(c => c.Count);
            biggest.Sort();
            string password = string.Join(',', biggest);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + countT);
            //Answer: 1170
            Console.WriteLine("Part 2: " + password);
            //Answer: bo,dd,eq,ik,lo,lu,ph,ro,rr,rw,uo,wx,yg
        }
    }
}
